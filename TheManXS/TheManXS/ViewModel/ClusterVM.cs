using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Gameplay;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.DetailPages
{
    public class ClusterVM : BaseViewModel
    {
        public enum ClusterVMMode { View, Edit, Add }
        public ClusterVM()
        {
            //InitClusterDBWithSampleValues();

            ListOfAllClusters = new List<Cluster>();            
            LoadListFromDB();

            AddNewItem = new Command(AddNewItemMethod);
            SaveItemChanges = new Command(SaveItemChangesMethod);

            ItemReadOnlyMode = true;
            CompressedLayout.SetIsHeadless(this, true);
        }

        private bool _itemReadOnlyMode;
        public bool ItemReadOnlyMode
        {
            get => _itemReadOnlyMode;
            set
            {
                _itemReadOnlyMode = value;
                SetValue(ref _itemReadOnlyMode, value);
            }
        }

        private ApplicabilityE _applicability;
        public ApplicabilityE Applicability
        {
            get => _applicability;
            set
            {
                _applicability = value;
                SetValue(ref _applicability, value);
            }
        }

        private string _headline;
        public string Headline
        {
            get => _headline;
            set
            {
                _headline = value;
                SetValue(ref _headline, value);
            }
        }

        private VariableImpactedE _variableImpacted;
        public VariableImpactedE VariableImpacted
        {
            get => _variableImpacted;
            set
            {
                _variableImpacted = value;
                SetValue(ref _variableImpacted, value);
            }
        }
        private bool _isPositive;
        public bool IsPositive
        {
            get => _isPositive;
            set
            {
                _isPositive = value;
                SetValue(ref _isPositive, value);
            }
        }

        public ICommand AddNewItem { get; }
        public ICommand SaveItemChanges { get; }

        private List<Cluster> _listOfAllClusters;
        public List<Cluster> ListOfAllClusters
        {
            get => _listOfAllClusters;
            set
            {
                _listOfAllClusters = value;
                SetValue(ref _listOfAllClusters, value);
            }
        }

        // this is a button on the toolbar.  Creates blank new record.
        private void AddNewItemMethod(object obj)
        {
            Cluster c = new Cluster();

            using(DBContext db = new DBContext())
            {
                db.Add(c);
                db.SaveChanges();
            }
            ItemReadOnlyMode = false;            
        }

        // this is the context for deleting items - visible in ViewMode
        private async void DeleteItemMethod(object obj)
        {
            var c = obj as Cluster;

            PageService ps = new PageService();
            bool deleteItem = await ps.DisplayAlert("Delete Item?", "Are you sure you want to delete item?", "yes", "no");

            if (deleteItem)
            {
                using (DBContext db = new DBContext())
                {
                    db.Remove(c);
                    db.SaveChanges();
                }
            }            
        }

        // this is the context button to edit items - visible in ViewMode
        public void EditItemMethod(ClusterVM cvm)
        {
            ItemReadOnlyMode = false;
        }

        //this is for the buttons that slide out on the right side of the item...visible in EditMode
        private void SaveItemChangesMethod(object obj)
        {
            var c = obj as Cluster;
            using(DBContext db = new DBContext())
            {
                db.Update(c);
            }
            ItemReadOnlyMode = true;
        }

        private void InitClusterDBWithSampleValues()
        {
            using (DBContext db = new DBContext())
            {
                var _clusterList = db.Clusters.ToList();

                if (_clusterList.Count == 0)
                {
                    db.Add(new Cluster()
                    {
                        Applicability = ApplicabilityE.ActivePlayer,
                        VariableImpacted = VariableImpactedE.Cash,
                        Headline = "{0} has been convicted of fraud, and has been fined {1}",
                        IsPositive = false
                    });

                    db.Add(new Cluster()
                    {
                        Applicability = ApplicabilityE.CommodityPricing,
                        VariableImpacted = VariableImpactedE.AllCommodityPrices,
                        Headline = "Major global recession, {0} is reducing in value by {1}",
                        IsPositive = false
                    });

                    db.Add(new Cluster()
                    {
                        Applicability = ApplicabilityE.AllPlayers,
                        VariableImpacted = VariableImpactedE.TheMan,
                        Headline = "New government in place, Costs for The Man have been increased by {0}",
                        IsPositive = true
                    });
                    db.SaveChanges();
                }
            }
        }
        private void LoadListFromDB()
        {
            using (DBContext db = new DBContext())
            {
                ListOfAllClusters = db.Clusters.ToList();
            }
        }
    }
}
