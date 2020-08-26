using System.Linq;
using TheManXS.Model.Main;

namespace TheManXS.Model.AI
{
    // starting thoughts about framework for future addition of computer players using AI
    // not connected yet.
    public enum Dimension { Players, Criteria, ResourceType, Total }
    public enum Criteria { RiskTolerance, ShortTermCashFlow, MediumTermCashFlow, LongTermCashFlow, Total }

    public class EvaluationMatrix
    {
        Game _game;
        //private byte[,,,] _evaluationMatrix;
        public EvaluationMatrix(Game game)
        {
            _game = game;

        }

        void InitArray()
        {

        }

        private int getNumberOfAIPlayers()
        {
            var pList = _game.PlayerList.Where(p => p.IsComputer).ToList();
            return pList.Count;
        }
    }
}
