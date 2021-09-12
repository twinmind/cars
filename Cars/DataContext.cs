using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cars
{
    public class DataContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(loggerFactory)
                .UseSnakeCaseNamingConvention();
        }
    }

    public class Brand 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<Model> Models { get; set; }
    }

    public class Model
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DoorsCount { get; set; }

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }


    public class ModelDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DoorsCount { get; set; }
        public int? BrandId { get; set; }
    }

    public class BrandDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<ModelDto> Models { get; set; }
    }
}
