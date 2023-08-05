using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        //startup dan db yolunu verebilmem için DbContextOptions içeren ctor oluşturduk.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            //var p = new Product() { ProductFeature = new ProductFeature() { } }   //product a bağlı ProductFeature üretmek istiyorsam bu şekil yapabilirim
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //Entity konfigirasyonlarını yapıyoruz.
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //konfigüreayonları uygula. tüm assembly i tarar

            base.OnModelCreating(modelBuilder);
        }
    }
}
