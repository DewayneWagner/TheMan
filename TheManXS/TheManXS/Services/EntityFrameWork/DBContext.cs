using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model;
using TheManXS.Model.Financial;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Gameplay;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Rocks;
using TheManXS.Model.Settings;
using TheManXS.Services.EntityFrameWork;

namespace TheManXS.Model.Services.EntityFrameWork
{
    public class DBContext : DbContext
    {
        private static bool _isCreated = false;
        public DBContext()
        {
            if (!_isCreated)
            {
                _isCreated = true;
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        public DbSet<Setting> Settings { get; set; } 
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<GameSpecificParameters> GameSpecificParameters { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<SQ> SQ { get; set; }
        public DbSet<Formation> Formation { get; set; }
        public DbSet<Commodity> Commodity { get; set; }
        public DbSet<SQInfrastructure> SQInfrastructure { get; set; }

        public void DeleteDatabase() => Database.EnsureDeleted();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = App.DataBaseLocation;
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClusterDBConfig());
            modelBuilder.ApplyConfiguration(new SettingsDBConfig());
            modelBuilder.ApplyConfiguration(new GSPDBConfig());
            modelBuilder.ApplyConfiguration(new PlayerDBConfig());
            modelBuilder.ApplyConfiguration(new SQDBConfig());
            modelBuilder.ApplyConfiguration(new FormationDBConfig());
            modelBuilder.ApplyConfiguration(new CommodityDBConfig());
            modelBuilder.ApplyConfiguration(new SQInfrastructureDBConfig());
        }
    }    
}
