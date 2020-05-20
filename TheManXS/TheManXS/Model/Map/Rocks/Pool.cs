using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.ParametersForGame;
using PP = TheManXS.Model.ParametersForGame.PoolConstructParametersSecondary;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;

namespace TheManXS.Model.Map.Rocks
{
    public class Pool
    {
        protected int poolLength;
        ResourcePools rp;
        private SQMapConstructArray _map;
        System.Random rnd = new System.Random();
        Game _game;

        public Pool(ResourcePools resPools, SQMapConstructArray map, Game game)
        {
            _game = game;
            _map = map;
            rp = resPools;
            IsPlayerStartSQ = rp.FormationCounter < QC.PlayerQ ? true : false;
            RT = rp.FormationCounter < (int)RT.RealEstate ? (RT)rp.FormationCounter : (RT)((rp.FormationCounter % (int)RT.RealEstate));
            poolLength = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.PoolConstructParameters, (int)PP.PoolLength);

            StartCoordinate = new Coordinate(true, _map);
            Formation = new Formation { ID = rp.FormationCounter };

            // determines which way rows or columns change
            X = GetDirection();
            Y = GetDirection();

            // these 2 factors are either 0 or 1....and are used to switc between row and column shifting
            XX = rnd.Next(0, 2);
            YY = XX == 1 ? 0 : 1;

            PoolResSqCounter = 0;
            int axisShift;

            for (int i = 0; i < poolLength; i++)
            {
                PoolCrossSection pcs = new PoolCrossSection(this, _map, _game);

                this.StartCoordinate.Row += (X * XX);
                this.StartCoordinate.Col += (Y * YY);
                int loopCounter = 0;

                do
                {
                    axisShift = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.PoolConstructParameters, (int)PP.AxisShift);
                    this.StartCoordinate.Row += (axisShift * YY);
                    this.StartCoordinate.Col += (axisShift * XX);

                    loopCounter++;
                    if (loopCounter == 5)
                        break;

                } while (!SqExists(this.StartCoordinate.Row, this.StartCoordinate.Col));

                if (PoolResSqCounter > QC.MaxResourceSQsInPool) { break; }
            }
            rp.TotalResSq += PoolResSqCounter;
            PoolResSqCounter = 0;
            rp.FormationCounter++;
        }
        private bool SqExists(int r, int c) => (r >= 0 && r < QC.RowQ && c >= 0 && c < QC.ColQ) ? true : false;

        public bool IsPlayerStartSQ { get; set; }
        public RT RT { get; set; }
        public Formation Formation { get; set; }
        public int PoolResSqCounter { get; set; }
        public Coordinate StartCoordinate { get; set; }
        public int TotalResSq { get; set; }

        public int X { get; set; }
        public int XX { get; set; }
        public int Y { get; set; }
        public int YY { get; set; }

        private int GetDirection()
        {
            // has to return -1 or 1 
            int mult = 0;
            do
            {
                mult = rnd.Next(-1, 2);
            } while (mult == 0);
            return mult;
        }
    }
}
