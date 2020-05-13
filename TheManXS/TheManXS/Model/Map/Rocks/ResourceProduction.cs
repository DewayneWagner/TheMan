using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Rocks
{
    public class ResourceProduction
    {
        public ResourceProduction() { }
        public int ID { get; set; }
        public int TurnNumber { get; set; }
        public int PlayerNumber { get; set; }
        public RT ResourceType { get; set; }
        public int Production { get; set; }
        public int SavedGameSlot => QC.CurrentSavedGameSlot;
    }
    public class ResourceProductionList : List<List<ResourceProduction>>
    {
        Game _game;
        public ResourceProductionList(Game game)
        {
            _game = game;
            // outside lists are for each player
            // inside lists are for each resourceTypes
            InitOutsideList();
            WriteToDB();
        }
        private void InitOutsideList()
        {
            foreach (Player player in _game.PlayerList)
            {
                this.Add(new List<ResourceProduction>());
            }
        }
        private void UpdateListAfterTurn()
        {
            for (int p = 0; p < _game.PlayerList.Count; p++)
            {
                for (int r = 0; r < (int)RT.Total; r++)
                {
                    this[p].Add(new ResourceProduction()
                    {
                        PlayerNumber = p,
                        Production = getProduction(p,r),
                        ResourceType = (RT)r,
                        TurnNumber = _game.TurnNumber,
                    });
                }
            }
            int getProduction(int playerNumber, int resourceNumber)
            {
                List<int> prodList = _game.SQList
                                .Where(s => s.ResourceType == (RT)resourceNumber)
                                .Where(s => s.OwnerNumber == playerNumber)
                                .Select(s => s.Production)
                                .ToList();

                return prodList.Sum();
            }
        }
        private void WriteToDB()
        {
            using (DBContext db = new DBContext())
            {
                foreach (List<ResourceProduction> listOfResourceProductionData in this)
                {
                    db.AddRange(listOfResourceProductionData);
                }
            }
        }
    }
    public class ResourceProductionDBConfig : IEntityTypeConfiguration<ResourceProduction>
    {
        public void Configure(EntityTypeBuilder<ResourceProduction> builder)
        {
            builder.Property(p => p.ID).ValueGeneratedOnAdd();

            builder.Property(s => s.ResourceType)
                .HasConversion(new EnumToStringConverter<RT>());
        }
    }
}
