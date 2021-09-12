using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.Services
{
    public interface ICatalogService 
    {
        Task<ModelDto> SaveModelAsync(ModelDto modelRequest, bool IsNestedTransaction = false);
        Task<ModelDto> GetModelAsync(int Id);
        Task<BrandDto> SaveBrandAsync(BrandDto brandRequest);
        Task<BrandDto> GetBrandAsync(int Id, int? limit = null, int? after = null, int? before = null);
    }

    public class CatalogService : ICatalogService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CatalogService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ModelDto> SaveModelAsync(ModelDto modelRequest, bool IsNestedTransaction = false) 
        {
            if (!modelRequest.BrandId.HasValue || modelRequest.BrandId <= 0) 
            {
                throw new ArgumentException("BrandId should be a positive number.");
            }
            var modelEntity = _mapper.Map<Model>(modelRequest);

            using var transaction = IsNestedTransaction ? null : await _context.Database.BeginTransactionAsync();

            var existingEntity = await _context.FindAsync<Model>(modelRequest.Id);
            if (existingEntity == null) _context.Models.Add(modelEntity);
            else _context.Entry(existingEntity).CurrentValues.SetValues(modelEntity);
            await _context.SaveChangesAsync();
            if (!IsNestedTransaction) await transaction.CommitAsync();

            return _mapper.Map<ModelDto>(modelEntity);
        }
        public async Task<ModelDto> GetModelAsync(int Id)
        {
            var modelEntity = await _context.FindAsync<Model>(Id);
            if (modelEntity != null) return _mapper.Map<ModelDto>(modelEntity);
            return null;
        }

        public async Task<BrandDto> SaveBrandAsync(BrandDto brandRequest)
        {
            var brandEntity = _mapper.Map<Brand>(brandRequest);

            using var transaction = await _context.Database.BeginTransactionAsync();
            
            var existingEntity = await _context.FindAsync<Brand>(brandRequest.Id);
            if (existingEntity == null) _context.Brands.Add(brandEntity);
            else _context.Entry(existingEntity).CurrentValues.SetValues(brandEntity);
            await _context.SaveChangesAsync();
            foreach(var model in brandRequest.Models)
            {
                model.BrandId = brandEntity.Id;
                await SaveModelAsync(model, true);
            }
            await transaction.CommitAsync();
            
            var response = _mapper.Map<BrandDto>(brandEntity);
            response.Models.ForEach(m => m.BrandId = null);
            return response;
        }


        public async Task<BrandDto> GetBrandAsync(int Id, int? limit = null, int? modelAfter = null, int? modelBefore = null)
        {
            if(modelAfter.HasValue && modelBefore.HasValue) throw new ArgumentException("Either modelAfter, or modelBefore should be specified.");

            var brandEntity = await _context.FindAsync<Brand>(Id);
            if (brandEntity != null) 
            {
                var models = _context.Models.Where(m => m.BrandId == brandEntity.Id);

                if(modelAfter.HasValue && modelAfter > 0)
                {
                    models = models.Where(m => m.Id > modelAfter).OrderBy(m => m.Id);
                }
                else if (modelBefore.HasValue && modelBefore > 0)
                {
                    models = models.Where(m => m.Id < modelBefore).OrderByDescending(m => m.Id);
                }
                if (limit.HasValue && limit > 0)
                {
                    models = (IOrderedQueryable<Model>)models.Take(limit.Value);
                }
                brandEntity.Models = (await models.ToListAsync()).OrderBy(m => m.Id).ToList();
                var brandDto = _mapper.Map<BrandDto>(brandEntity);
                return brandDto;
            }
            return null;
        }
    }
}
