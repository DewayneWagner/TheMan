using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using TheManXS.Model.Services.EntityFrameWork;
using System.Linq;
using TheManXS.Model.Settings;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.ViewModel.DetailPages
{
    class SettingsVM : BaseViewModel
    {
        Game _game;
        public SettingsVM()
        {
            SetGameReference();
            SettingsVMOC = new ObservableCollection<SettingsVM>();
            LoadSettingsOC();
            SaveChanges = new Command(SaveChangesMethod);
        }       

        public SettingsVM(ParameterConstant pc)
        {
            SetGameReference();
            IsBounded = false;
            LBOrConstant = pc.Constant;
            PrimaryIndexName = Convert.ToString(pc.PrimaryParameter);
            SecondarySubIndexName = pc.SecondaryParameterSubIndex;
            PrimaryIndexNumber = (int)pc.PrimaryParameter;
            SecondaryIndexNumber = pc.SecondaryParameterIndex;
        }
        public SettingsVM(ParameterBounded pb)
        {
            SetGameReference();
            IsBounded = true;
            LBOrConstant = pb.LowerBounds;
            UB = pb.UpperBounds;            
            PrimaryIndexName = Convert.ToString(pb.PrimaryParameter);
            SecondaryIndexNumber = pb.SecondaryParameterIndex;
            SecondarySubIndexName = pb.SecondaryParameterSubIndex;
            PrimaryIndexNumber = pb.PrimaryIndexNumber;
        }

        void SetGameReference()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
        }

        public ICommand SaveChanges { get; set; }
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
        public int PrimaryIndexNumber { get; set; }
        public string PrimaryIndexName { get; set; }
        public string SecondarySubIndexName { get; set; }
        public int SecondaryIndexNumber { get; set; }

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
            foreach (ParameterBounded parameterBounded in _game.ParameterBoundedList)
            {
                SettingsVMOC.Add(new SettingsVM(parameterBounded));
            }
            foreach (ParameterConstant parameterConstant in _game.ParameterConstantList)
            {
                SettingsVMOC.Add(new SettingsVM(parameterConstant));
            }
        }
        private void SaveChangesMethod(object obj)
        {
            ParameterBoundedList pbl = new ParameterBoundedList(true);
            ParameterConstantList pcl = new ParameterConstantList(true);

            foreach (SettingsVM s in SettingsVMOC)
            {
                if (s.IsBounded) { pbl.Add(new ParameterBounded(s.PrimaryIndexNumber, s.SecondarySubIndexName,s.SecondaryIndexNumber, s.UB, s.LBOrConstant)); }
                else { pcl.Add(new ParameterConstant(s.PrimaryIndexNumber, s.SecondaryIndexNumber, s.LBOrConstant)); }
            }

            pbl.WriteDataToBinaryFile();
            pcl.WriteDataToBinaryFile();
        }
    }
}
