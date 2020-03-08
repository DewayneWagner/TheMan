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
        public SettingsVM()
        {
            SettingsVMOC = new ObservableCollection<SettingsVM>();
            LoadSettingsOC();
            SaveChanges = new Command(SaveChangesMethod);
        }       

        public SettingsVM(ParameterConstant pc)
        {
            IsBounded = false;
            LBOrConstant = pc.Constant;
            PrimaryIndexName = Convert.ToString(pc.PrimaryParameter);
            SecondarySubIndexName = pc.SecondaryParameterSubIndex;
            PrimaryIndexNumber = (int)pc.PrimaryParameter;
            SecondaryIndexNumber = pc.SecondaryParameterIndex;
        }
        public SettingsVM(ParameterBounded pb)
        {
            IsBounded = true;
            LBOrConstant = pb.LowerBounds;
            UB = pb.UpperBounds;
            PrimaryIndexName = Convert.ToString(pb.PrimaryParameter);
            SecondarySubIndexName = pb.SecondaryParameterSubIndex;
            PrimaryIndexNumber = (int)pb.PrimaryParameter;
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
            ParameterBoundedList pbl = new ParameterBoundedList();

            foreach (ParameterBounded pb in pbl)
            {
                SettingsVMOC.Add(new SettingsVM(pb));
            }

            ParameterConstantList pcl = new ParameterConstantList();

            foreach (ParameterConstant pc in pcl)
            {
                SettingsVMOC.Add(new SettingsVM(pc));
            }
        }
        private void SaveChangesMethod(object obj)
        {
            ParameterConstantList constantList = new ParameterConstantList();
            ParameterBoundedList boundedList = new ParameterBoundedList();

            foreach (SettingsVM s in SettingsVMOC)
            {
                if (s.IsBounded)
                {
                    boundedList.Add(new ParameterBounded(s.PrimaryIndexNumber, s.SecondaryIndexNumber, s.UB, s.LBOrConstant));
                }
                else
                {
                    constantList.Add(new ParameterConstant(s.PrimaryIndexNumber, s.SecondaryIndexNumber, s.LBOrConstant));
                }
            }
            constantList.WriteDataToBinaryFile();
            boundedList.WriteDataToBinaryFile();
        }
    }
}
