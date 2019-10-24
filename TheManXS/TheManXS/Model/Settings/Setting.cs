using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using static TheManXS.Model.Settings.SettingsMaster;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TheManXS.Model.Services.EntityFrameWork;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;

namespace TheManXS.Model.Settings
{
    [Table("Settings")]
    public class Setting
    {
        public Setting() { }
        private int _secondaryIndexNumber;
        
        public Setting(AS ap, int secondaryIndexNumber)
        {
            PrimaryIndex = ap;
            SecondaryIndex s = new SecondaryIndex(ap, secondaryIndexNumber);
            IsBounded = s.IsBounded;
            SecondaryIndexTypeName = s.Type;
            SecondarySubIndex = s.SubIndexName;            
            _secondaryIndexNumber = secondaryIndexNumber;
            Key = (100 + (int)PrimaryIndex) * 1000 + (100 + _secondaryIndexNumber);
        }

        [Key]
        public int Key { get; set; }
        public bool IsBounded { get; set; }
        public AS PrimaryIndex { get; set; }
        public string SecondaryIndexTypeName { get; set; }
        public string SecondarySubIndex { get; set; }
        public double LBOrConstant { get; set; }
        public double UB { get; set; }

        public static int GetKey(AS primaryIndex, int secondaryIndexNumber) =>
            (100 + (int)primaryIndex) * 1000 + (100 + secondaryIndexNumber);
        public static double GetConstant(AS AS, int secondarySubIndex)
        {
            int key = GetKey(AS, secondarySubIndex);
            try
            {
                using (DBContext db = new DBContext())
                {
                    var setting = db.Settings.Find(key);
                    return setting.LBOrConstant;
                }
            }
            catch
            {
                return 0;
            }            
        }
        public static double GetRand(AS AS, int secondarySubIndex)
        {
            int key = GetKey(AS, secondarySubIndex);
            System.Random rnd = new System.Random();
            using(DBContext db = new DBContext())
            {
                var setting = db.Settings.Find(key);
                return rnd.NextDouble() * (setting.UB - setting.LBOrConstant) + setting.LBOrConstant;
            }
        }
    }
}
