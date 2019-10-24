using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using TheManXS.Model.Services.EntityFrameWork;
using System.Linq;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;

namespace TheManXS.Model.Settings
{
    public class SettingsMaster
    {
        // if any alterations to AllSettings - update SecondaryIndex.cs
        public enum AS
        {
            AbandonTT, AssValST, CityProdCD, DevTT, ExpTT, OPEXTT, ProductionTT, ReactivateSingleP, SusTT,
            TransTT, TerrainBoundedTCB, PoolParams, CashConstant, IntRateCR, MapConstants, PlayerConstants, ResConstant,
            MiscellaneousConstants, Total
        }

        // if there are any enums added to the below - SecondaryIndex.cs needs to be updated.
        public enum PoolParams { PoolLength, PoolWidth, PlStSq, AxisShift, Multiple, Decline, Total }
        public enum CashConstantParameters { StartCash, StartDebt, TheManCut, SquarePrice, CommStartPrice, MaxCommPrice, 
            MinCommPrice, MinCommodityChange, MaxCommodityChange, Total }
        public enum ResConstantParams { DeclineTurnsFactor, ResSqRatio, MaxPoolSQ, Total }
        public enum TerrainConstructConstants { RowQ, ColQ, SqSize, StartRowRatio, Total }
        public enum PlayerConstants { PlayerQ, Total }
        public enum DifficultyLevels { Diff1, Diff2, Diff3, Diff4, Total }
        public enum ResourceTypeE { Oil, Gold, Coal, Iron, Silver, Total, Nada, RealEstate } // Nada second last - so total works for arrays
        public enum StatusTypeE { Nada, Unexplored, Explored, Developing, Producing, Suspended, Total }
        public enum TerrainTypeE { Grassland, Forest, Mountain, City, Total }
        public enum CreditRatingsE { AAA, AA, A, B, C, Junk, Total }
        public enum TerrainConstructBounded { ForestWidthRatio, GrasslandWidthRatio, AxisShift, TerrainOffset, Total }
        public enum CityDensity { Low, Medium, High, Total }
        public enum MiscellaneousStuff { SavedGameSlotsQ, Total }

        public enum Difficulty { Easy, Medium, Hard }
        
        private List<Setting> _settingsThatAreInDB;

        public SettingsMaster(bool dummyBool) { }
        
        public SettingsMaster()
        {
            new SecondaryIndex(); // to initialize static arrays
            LoadListOfSettingsInDB();
            UpdateSettingsInDB();
        }
        private void LoadListOfSettingsInDB()
        {
            using (DBContext db = new DBContext())
            {
                _settingsThatAreInDB = db.Settings.ToList();
            }
        }

        public List<Setting> GetListOfSettingsInDB()
        {
            LoadListOfSettingsInDB();
            return _settingsThatAreInDB;
        }

        private void UpdateSettingsInDB()
        {
            List<Setting> _parametersThatShouldBeInDB = new List<Setting>();
            bool done = false;
            int allParameterIndex = 0, secondaryIndexNumber = 0;
            AS ap;

            do
            {
                ap = (AS)allParameterIndex;
                Setting s = new Setting(ap, secondaryIndexNumber);

                while (s.SecondarySubIndex != "Total")
                {
                    _parametersThatShouldBeInDB.Add(s);
                    secondaryIndexNumber++;
                    s = new Setting(ap, secondaryIndexNumber);
                };

                secondaryIndexNumber = 0;

                allParameterIndex++;

                if (Convert.ToString((AS)allParameterIndex) == "Total")
                    done = true;

            } while (!done);

            if(_settingsThatAreInDB.Count != 0)
            {
                foreach (Setting p in _parametersThatShouldBeInDB)
                {
                    if (_settingsThatAreInDB.Contains(p))
                    {
                        _parametersThatShouldBeInDB.Remove(p);
                        _settingsThatAreInDB.Remove(p);
                    }
                }
                foreach (Setting p in _settingsThatAreInDB)
                {
                    using(var db = new DBContext())
                    {
                        db.Settings.Remove(p);
                        db.SaveChanges();
                    }
                }
            }
            using(var db = new DBContext())
            {
                db.AddRange(_parametersThatShouldBeInDB);
                db.SaveChanges();
            }
        }
    }
}
