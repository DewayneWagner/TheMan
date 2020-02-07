using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using TheManXS.Model.Company;
using System.Linq;
using TheManXS.Model.Map.Surface;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;

namespace TheManXS.Model.Units
{    
    public class Unit : List<SQ>
    {
        public enum UnitStatus { Tentative, Complete, }
        public Unit() { }
        public Unit(SQ sq) 
        {
            Number = QC.UnitCounter + 1;
            Status = UnitStatus.Tentative;
            ResourceType = sq.ResourceType;
            PlayerNumber = sq.OwnerNumber;
            PlayerName = sq.OwnerName;
            FormationNumber = sq.FormationID;

            AddSQToUnit(sq);
        }
        public void AddSQToUnit(SQ sq)
        {
            Player p = (Player)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActivePlayer)];
            Color c = new CompanyColors(p.Color).ColorXamarin;

            if (!this.Contains(sq))
            {
                this.Add(sq);
                OPEXDiscount += QC.OPEXDiscountPerSQInUnit;
                DevelopmentDiscount += QC.CAPEXDiscountPerSQInUnit;                
                new StaggeredBorder(c).InitStaggeredBorders(this);
            }
            else
            {
                this.Remove(sq);
                sq.Tile.OverlayGrid.RemoveOutsideBorders();
                OPEXDiscount -= QC.OPEXDiscountPerSQInUnit;
                DevelopmentDiscount -= QC.CAPEXDiscountPerSQInUnit;
                new StaggeredBorder(c).InitStaggeredBorders(this);
            }
        }       
        public int Number { get; set; }
        public UnitStatus Status { get; set; }
        public double OPEXDiscount { get; set; }
        public double DevelopmentDiscount { get; set; }
        public RT ResourceType { get; set; }
        public int FormationNumber { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        
        public void CreateUnit()
        {
            this.Status = UnitStatus.Complete;

            using (DBContext db = new DBContext())
            {
                foreach (SQ sq in this)
                {
                    sq.IsPartOfUnit = true;
                    sq.UnitNumber = Number;
                    sq.OPEXPerUnit *= (1 - OPEXDiscount);
                    sq.NextActionCost *= (1 - DevelopmentDiscount);
                }
                db.SaveChanges();
            }
        }
        public void KillUnit()
        {
            QC.UnitCounter--;
            foreach (SQ sq in this) { sq.Tile.OverlayGrid.RemoveOutsideBorders(); }
        }
        public bool IsSQAdjacentToSQsAlreadyInUnit(SQ sq)
        {
            if(this.Any(s => s.Key == Coordinate.GetSQKey(sq.Row,(sq.Col-1))) || 
                (this.Any(q => q.Key == Coordinate.GetSQKey((sq.Row-1),sq.Col)) ||
                (this.Any(u => u.Key == Coordinate.GetSQKey(sq.Row, (sq.Col + 1))) ||
                (this.Any(a => a.Key == Coordinate.GetSQKey((sq.Row+1),sq.Col))))))
                { return true; }
            return false;
        }
        // will not activate until later
        public bool IsOwnedByPlayerAndHasSameResources(SQ sq)
        {
            if (sq.FormationID == FormationNumber &&
                sq.OwnerNumber == PlayerNumber &&
                sq.ResourceType == ResourceType &&
                sq.Status == ST.Explored)
            { return true; }
            { return false; }
        }
    }    
}
