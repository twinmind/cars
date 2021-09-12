using Cars.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.Controllers
{
    /// <summary>
    /// Car catalog API
    /// </summary>
    [ApiController]
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        /// <summary>
        /// Car catalog API
        /// </summary>
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Get model
        /// </summary>
        /// <remarks>Retrieves the details of an existing car model. You need only supply the unique model identifier that was returned upon model creation.</remarks>
        /// <param name="Id">model identifier</param>
        /// <returns>Returns a model object if a valid identifier was provided.</returns>
        [HttpGet("models/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModelDto>> GetModel(int Id)
        {
            var model = await _catalogService.GetModelAsync(Id);
            if (model == null) return NotFound();
            return Ok(model);

        }

        /// <summary>
        /// Create or update model
        /// </summary>
        /// <param name="modelDto">Model object</param>
        /// <returns>Returns model object if saving succeeded.</returns>
        [HttpPost("models")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModelDto>> SaveModel(ModelDto modelDto)
        {
            var model = await _catalogService.SaveModelAsync(modelDto);
            return CreatedAtAction(nameof(GetModel), new { Id = model.Id }, model);

        }

        /// <summary>
        /// Get brand
        /// </summary>
        /// <remarks>Retrieves the details of an existing car brand. You need only supply the unique brand identifier that was returned upon brand creation.</remarks>
        /// <param name="Id">brand identifier.</param>
        /// <param name="limit">A limit on the number of models to be returned. By default all models are returned.</param>
        /// <param name="modelAfter">A cursor to use in pagination. It is an model ID that defines your place in the list to fetch the next page.</param>
        /// <param name="modelBefore">A cursor to use in pagination. It is an model ID that defines your place in the list to fetch the previous page.</param>
        /// <returns>Returns a brand object if a valid identifier was provided.</returns>
        [HttpGet("brands/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BrandDto>> GetBrand([FromRoute] int Id, [FromQuery] int? limit = null, [FromQuery] int? modelAfter = null, [FromQuery] int? modelBefore = null)
        {
            var brand = await _catalogService.GetBrandAsync(Id, limit, modelAfter, modelBefore);
            if (brand == null) return NotFound();
            return Ok(brand);

        }

        /// <summary>
        /// Create or update brand
        /// </summary>
        /// <param name="brandDto">Brand object</param>
        /// <returns>Returns brand object if saving succeeded.</returns>
        [HttpPost("brands")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BrandDto>> SaveBrand(BrandDto brandDto)
        {
            var brand = await _catalogService.SaveBrandAsync(brandDto);
            return CreatedAtAction(nameof(GetBrand), new { Id = brand.Id }, brand);

        }
    }
}
