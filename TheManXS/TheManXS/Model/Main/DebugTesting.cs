using System;
using System.Collections.Generic;
using System.Text;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    class DebugTesting
    {
        Game _game;
        System.Random rnd = new System.Random();
        public DebugTesting(Game game)
        {
            _game = game;
            InitSQsForTesting();
        }
        void InitTestProperties()
        {

        }
        private void InitSQsForTesting()
        {
            int row = 2;
            int col = 2;

            var s = _game.SQList[row, col];

            s.ResourceType = RT.Oil;
            s.Production = rnd.Next(5, 20);
            s.OPEXPerUnit = rnd.Next(15, 35);
            s.FormationID = 50;
            s.Transport = rnd.Next(5, 20);
            s.Status = ST.Producing;
            s.OwnerNumber = QC.PlayerIndexActual;

            foreach (Player player in _game.PlayerList)
            {
                int productingSQsForPlayer = 0;
                int loopCounter = 0;
                do
                {
                    SQ sq = _game.SQList[rnd.Next(0, QC.RowQ), rnd.Next(0, QC.ColQ)];
                    loopCounter++;

                    if (sq.OwnerNumber == QC.PlayerIndexTheMan
                        && sq.Production > 0
                        && sq.Status == ST.Nada
                        && sq.ResourceType != RT.RealEstate
                        && (int)sq.ResourceType < (int)RT.Nada)
                    {
                        sq.OwnerNumber = player.Number;
                        sq.OwnerName = player.Name;
                        sq.Status = ST.Producing;
                        productingSQsForPlayer++;
                    }

                } while (loopCounter < 50 && productingSQsForPlayer < 3);
            }
        }
    }
}
