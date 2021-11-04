using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using health.Models;
using Microsoft.EntityFrameworkCore;

namespace health.Data
{
    public class SiteContext : DbContext
    {
        public SiteContext(DbContextOptions<SiteContext> options)
            : base(options)
        {
        }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<MyPage> Pages { get; set; }
        public DbSet<PageInfo> PageInfo { get; set; }
        public DbSet<UploadImage> UploadImages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Herb> Herbs { get; set; }
        public DbSet<Contraindication> Contraindications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
