using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace WolfPack.EntityFramework
{
    public class WolfContext : DbContext
    {
        public DbSet<Wolf> Wolves { get; set; }
        public DbSet<Pack> Packs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost;Database=wolfpack;User Id=sa;password=Poepopeenstokje1@");
        }
    }

    public class Wolf
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public string GpsLocation { get; set; }
    }

    public class Pack
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Wolf> Wolves { get; set; }
    }
}