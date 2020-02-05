using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using static TheManXS.Model.Settings.SettingsMaster;
using QC = TheManXS.Model.Settings.QuickConstants;
using Xamarin.Forms;
using System.Linq;
using TheManXS.Model.Financial;
using TheManXS.Model.CityStuff;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.Model.InfrastructureStuff;

namespace TheManXS.Model.Main
{
    public class SQ
    {
        public SQ() { }
        public SQ(bool isForPropertyDictionary) { NextAction = new NextAction(this); }

        public SQ(int row, int col)
        {
            Row = row;
            Col = col;
            SavedGameSlot = QC.CurrentSavedGameSlot;
            Key = Coordinate.GetSQKey(row, col);
            NextAction = new NextAction(this);
            //new SQInfrastructure(this);
            OwnerNumber = QC.PlayerIndexTheMan;
            ResourceType = ResourceTypeE.Nada;
            OwnerName = QC.NameOfOwnerOfUnOwnedSquares;         
        }

        public int Key { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }   
        public int SavedGameSlot { get; set; }        
        public TerrainTypeE TerrainType { get; set; }
        public ResourceTypeE ResourceType { get; set; }
        public StatusTypeE Status { get; set; }
        public int OwnerNumber { get; set; }
        public string OwnerName { get; set; }
        public bool IsStartSquare { get; set; }
        public bool IsPartOfUnit { get; set; }
        public int UnitNumber { get; set; }
        public int Production { get; set; }
        public double OPEXPerUnit { get; set; }
        public int FormationID { get; set; }
        public double Transport { get; set; }

        
        // not included in DB
        public NextAction NextAction { get; set; }
        public City City { get; set; }
        public Coordinate FullCoordinate { get; set; }
        public Tile Tile { get; set; }
    }    
}
