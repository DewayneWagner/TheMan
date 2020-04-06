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
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using TheManXS.Model.Financial;

namespace TheManXS.Model.Units
{    
    public class Unit : List<SQ>
    {
        public enum UnitSelectionStatus { Tentative, Complete, }

        private Game _game;
        private List<SQ> _ListOfSQsFromOriginal;
        public Unit() { }

        public Unit(List<SQ> listOfSquaresInUnit, Game game)
        {
            _game = game;
            _ListOfSQsFromOriginal = listOfSquaresInUnit;
            SetPropertiesOfUnitFromFirstSQInList(listOfSquaresInUnit[0]);            
            foreach (SQ sq in listOfSquaresInUnit) { AddSQToUnit(sq); }
            SetNextActionCostAndText();
            SetOPEX();
        }

        void SetPropertiesOfUnitFromFirstSQInList(SQ sq)
        {
            Number = QC.UnitCounter + 1;
            UnitCreationStatus = UnitSelectionStatus.Tentative;
            ResourceType = sq.ResourceType;
            PlayerNumber = sq.OwnerNumber;
            PlayerName = sq.OwnerName;
            FormationNumber = sq.FormationID;
        }
        
        public void AddSQToUnit(SQ sq)
        {
            Player p = (Player)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActivePlayer)];
            Status = sq.Status;

            if (!this.Contains(sq))
            {
                this.Add(sq);
                OPEXDiscount += QC.OPEXDiscountPerSQInUnit;
                Production += sq.Production;
                DevelopmentDiscount += QC.CAPEXDiscountPerSQInUnit;                
                new StaggeredBorder(_game.ActivePlayer.SKColor).InitStaggeredBorders(this);
            }
            else
            {
                this.Remove(sq);
                OPEXDiscount -= QC.OPEXDiscountPerSQInUnit;
                Production -= sq.Production;
                DevelopmentDiscount -= QC.CAPEXDiscountPerSQInUnit;
                new StaggeredBorder(_game.ActivePlayer.SKColor).InitStaggeredBorders(this);
            }
        }

        public int Number { get; set; }

        public UnitSelectionStatus UnitCreationStatus { get; set; }

        private ST _status;
        public ST Status
        {
            get => _status;
            set
            {
                _status = value;
                SetNextActionCostAndText();
            }
        }
        public double OPEXTotalBeforeDiscount { get; set; }        
        public double OPEXDiscount { get; set; }
        public double OPEXTotalAfterDiscount { get; set; }
        public double OPEXAveragePerUnit { get; set; }
        public double DevelopmentDiscount { get; set; }
        public RT ResourceType { get; set; }
        public int FormationNumber { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int Production { get; set; }
        public string NextActionText { get; set; }
        public double NextActionCost { get; set; }
        public NextAction.NextActionType NextActionType { get; set; }

        public void CreateUnit()
        {
            this.UnitCreationStatus = UnitSelectionStatus.Complete;

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
            foreach (SQ sq in this) {; }
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
        private void SetNextActionCostAndText()
        {
            if(this.Count > 0)
            {
                NextAction n = new NextAction(this[0], _game);
                NextActionText = n.Text;
                NextActionType = n.ActionType;

                foreach (SQ sq in this)
                {
                    sq.Status = Status;
                    NextActionCost += n.Cost;
                }
            }            
        }
        private void SetOPEX()
        {
            OPEXTotalBeforeDiscount = getOPEX();
            OPEXTotalAfterDiscount = OPEXTotalBeforeDiscount * (1 - OPEXDiscount);
            OPEXAveragePerUnit = OPEXTotalAfterDiscount / Production;

            double getOPEX()
            {
                double opex = 0;
                foreach (SQ sq in this)
                {
                    opex += (sq.OPEXPerUnit * sq.Production);
                }
                return opex;
            }
        }

        public static void LoadUnitWithSavedGameData(Game game)
        {
            using (DBContext db = new DBContext())
            {
                int numberOfUnits = db.SQ.Where(s => s.IsPartOfUnit)
                    .Max(s => s.UnitNumber);

                List<SQ> listOfSqsInUnits = db.SQ.Where(s => s.IsPartOfUnit)
                    .OrderBy(s => s.UnitNumber)
                    .ToList();

                game.ListOfCreatedProductionUnits = new List<Unit>(numberOfUnits);

                foreach (SQ sq in listOfSqsInUnits)
                {
                    game.ListOfCreatedProductionUnits[sq.UnitNumber].Add(sq);
                }
            }
        }
    }    
}
