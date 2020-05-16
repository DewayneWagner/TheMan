using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using ACP = TheManXS.Model.ParametersForGame.AllConstantParameters;
using CC = TheManXS.Model.ParametersForGame.ConstructionConstantsSecondary;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class DevelopAction : SQAction
    {
        // will need to incorporate turns until development at a later point
        // will need to incorporate budget fluctuations into here later
        public DevelopAction(Game game, PanelType pt) : base(game, pt)
        {
            if (PanelType == PanelType.SQ) { ExecuteDevelopmentActionSQ(); }
            else { ExecuteDevelopmentActionUnit(); }
        }
        private void ExecuteDevelopmentActionSQ()
        {
            var s = Game.ActiveSQ;
            var p = Game.ActivePlayer;

            if (s.IsRoadConnected)
            {
                if (PlayerHasEnoughDough(s.NextActionCost))
                {
                    s.Status = ST.Producing;
                    p.Cash -= s.NextActionCost;
                    AddSurfaceStructure(s);
                }
            }
            else { DisplayMessageThatRoadIsRequired(); }
        }
        private void ExecuteDevelopmentActionUnit()
        {
            var u = Game.ActiveUnit;
            var p = Game.ActivePlayer;

            if (PlayerHasEnoughDough(u.NextActionCost))
            {
                if (u.Any(s => s.IsRoadConnected))
                {
                    u.StructureSQ = u.Where(s => s.IsRoadConnected).FirstOrDefault();
                    u.Status = ST.Producing;
                    p.Cash -= u.NextActionCost;
                    AddSurfaceStructure(u.StructureSQ);
                }
                else { DisplayMessageThatRoadIsRequired(); }
            }
        }
        private async void DisplayMessageThatRoadIsRequired() => await PageServices.DisplayAlert("Heh DumbAss - you need a road before you can develop.");
        
        void AddSurfaceStructure(SQ sq)
        {
            if(sq.ResourceType == Model.ParametersForGame.ResourceTypeE.Oil)
            {
                new PumpJack(Game, Game.ActiveSQ);
            }
            else
            {
                new MineShaft(Game, Game.ActiveSQ);
            }
        }
        private int getTurnsToCompleteConstruction()
        {
            var c = Game.ParameterConstantList;
            switch (Game.ActiveSQ.TerrainType)
            {
                case Model.ParametersForGame.TerrainTypeE.Grassland:
                    return (int)c.GetConstant(ACP.ConstructionConstants, (int)CC.TurnsToBuildOnGrassLand);
                    
                case Model.ParametersForGame.TerrainTypeE.Forest:
                    return (int)c.GetConstant(ACP.ConstructionConstants, (int)CC.TurnsToBuildOnForestTile);

                case Model.ParametersForGame.TerrainTypeE.Mountain:
                    return (int)c.GetConstant(ACP.ConstructionConstants, (int)CC.TurnsToBuildOnMountainTile);

                case Model.ParametersForGame.TerrainTypeE.City:
                case Model.ParametersForGame.TerrainTypeE.River:
                case Model.ParametersForGame.TerrainTypeE.Slough:
                case Model.ParametersForGame.TerrainTypeE.Sand:
                case Model.ParametersForGame.TerrainTypeE.Total:
                default:
                    break;
            }
            return 1;
        }
    }
}
