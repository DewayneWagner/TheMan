using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.Model.Financial
{
    public class NextAction
    {
        public enum NextActionType { Purchase, Explore, Develop, Suspend, Reactivate, NotEnabled }
        Game _game;
        public NextAction(SQ sq, Game game)
        {
            _game = game;
            if (sq.TerrainType != TerrainTypeE.City) { UpdateNextActionNonCity(sq); }
            else { UpdateNextActionCity(sq); }
        }

        public NextActionType ActionType { get; set; }
        public string Text { get; set; }
        public double Cost { get; set; }

        private void UpdateNextActionNonCity(SQ sq)
        {
            switch (sq.Status)
            {
                case ST.Nada:
                    Text = "Purchase Property";
                    Cost = _game.ParameterConstantList.GetConstant(AllConstantParameters.CashConstant, (int)CashConstantSecondary.SquarePrice);
                    ActionType = NextActionType.Purchase;
                    break;
                case ST.Unexplored:
                    Text = "Explore for Resources";
                    Cost = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.ExploreCostPerTerrainType, (int)sq.TerrainType);
                    ActionType = NextActionType.Explore;
                    break;
                case ST.Explored:
                    Text = "Develop Property";
                    Cost = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.DevelopmentCostPerTerrainType, (int)sq.TerrainType);
                    ActionType = NextActionType.Develop;
                    break;
                case ST.Developing:
                    Text = "Under Development";
                    Cost = 0;
                    ActionType = NextActionType.NotEnabled;
                    break;
                case ST.Producing:
                    Text = "Suspend Production";
                    Cost = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.ActionCosts, (int)ActionCostsSecondary.SuspendCostPerUnit);
                    ActionType = NextActionType.Suspend;
                    break;
                case ST.Suspended:
                    Text = "Reactive Property";
                    Cost = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.ActionCosts, (int)ActionCostsSecondary.ReactivationCostPerUnit);
                    ActionType = NextActionType.Reactivate;
                    break;
                default:
                    Text = "Nada";
                    Cost = 0;
                    ActionType = NextActionType.NotEnabled;
                    break;
            }
        }
        private void UpdateNextActionCity(SQ sq)
        {

        }
    }
}
