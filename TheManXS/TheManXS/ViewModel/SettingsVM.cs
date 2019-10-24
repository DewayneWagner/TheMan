using System;
using System.Collections.Generic;
using System.Text;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using SQLite;
using TheManXS.Model.Services.EntityFrameWork;
using System.Linq;
using static TheManXS.Model.Settings.SettingsMaster;
using TheManXS.Model.Settings;

namespace TheManXS.ViewModel.DetailPages
{
    class SettingsVM : TheManXS.ViewModel.Services.BaseViewModel
    {
        public SettingsVM()
        {
            SettingsVMOC = new ObservableCollection<SettingsVM>();
            LoadSettingsOC();
            SaveChanges = new Command(SaveChangesMethod);
            UpdateSettings = new Command(UpdateSettingsMethod);
            LoadSettingsFromBinaryFileIntoDB = new Command(LoadSettingsFromBinaryFileIntoDBMethod);
        }       

        public SettingsVM(Setting s)
        {
            IsBounded = s.IsBounded;
            LBOrConstant = s.LBOrConstant;
            PrimaryIndex = s.PrimaryIndex;
            UB = s.UB;
            PrimaryIndexName = Convert.ToString(s.PrimaryIndex);
            SecondarySubIndexName = s.SecondarySubIndex;
            Key = s.Key;
        }

        private int Key { get; }
        public ICommand SaveChanges { get; set; }
        public ICommand UpdateSettings { get; set; }
        public ICommand LoadSettingsFromBinaryFileIntoDB { get; set; }
        public bool IsBounded { get; set; }

        private double lbOrConstant;
        public double LBOrConstant
        {
            get => lbOrConstant;
            set
            {
                lbOrConstant = value;
                SetValue(ref lbOrConstant, value);
            }
        }
        private double ub;
        public double UB
        {
            get => ub;
            set
            {
                ub = value;
                SetValue(ref ub, value);
            }
        }
        public string PrimaryIndexName { get; set; }
        public AS PrimaryIndex { get; set; }
        public string SecondarySubIndexName { get; set; }

        private ObservableCollection<SettingsVM> _settingsVMOC;
        public ObservableCollection<SettingsVM> SettingsVMOC
        {
            get => _settingsVMOC;
            set
            {
                _settingsVMOC = value;
                SetValue(ref _settingsVMOC, value);
            }
        }
        private void LoadSettingsOC()
        {
            List<Setting> _sList;
            using (DBContext db = new DBContext())
            {
                _sList = db.Settings.ToList();
            }
            foreach (Setting s in _sList)
            {
                SettingsVMOC.Add(new SettingsVM(s));
            }
        }
        private void SaveChangesMethod(object obj)
        {
            // convert from OC to Settings List
            List<Setting> sList = new List<Setting>();

            foreach (SettingsVM svm in _settingsVMOC)
            {
                sList.Add(new Setting()
                {
                    IsBounded = svm.IsBounded,
                    Key = svm.Key,
                    LBOrConstant = svm.LBOrConstant,
                    PrimaryIndex = svm.PrimaryIndex,
                    SecondaryIndexTypeName = svm.SecondarySubIndexName,
                    SecondarySubIndex = svm.SecondarySubIndexName,
                    UB = svm.ub
                });
            }
            using (DBContext db = new DBContext())
            {
                if (db.Settings.Count() == sList.Count) { db.UpdateRange(sList); }
                else { db.AddRange(sList); }
                db.SaveChanges();
            }
            UpdateSettingsInBinaryFileMethod();
        }
        private void UpdateSettingsMethod(object obj) { new SettingsMaster(); }        
        private void UpdateSettingsInBinaryFileMethod() => BinaryBackup.UpdateBinaryWithSettingsFromDB();
        private void LoadSettingsFromBinaryFileIntoDBMethod()
        {
            List<Setting> sList = BinaryBackup.GetSettingsFromBinary();
            foreach (Setting s in sList)
            {
                SettingsVMOC.Add(new SettingsVM(s));
            }
        }
    }
}
