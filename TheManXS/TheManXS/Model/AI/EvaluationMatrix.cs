using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.AI
{
    public enum Dimension { Players, Criteria, ResourceType, Total }
    public enum Criteria { RiskTolerance, ShortTermCashFlow, MediumTermCashFlow, LongTermCashFlow, Total }

    public class EvaluationMatrix
    {
        Game _game;
        private byte[,,,] _evaluationMatrix;
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
