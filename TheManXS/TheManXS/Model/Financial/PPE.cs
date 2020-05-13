using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Financial
{
    class PPE
    {
        static List<double> _ratiosForPPE = new List<double>();
        Game _game;
        Player _player;
        public PPE(Game game, Player player)
        {
            _game = game;
            _player = player;
            if(_ratiosForPPE.Count == 0) { LoadListWithRatios(); }
            SetPPEValuation();
        }
        public double Valuation { get; set; }
        void LoadListWithRatios()
        {
            _ratiosForPPE = _game.ParameterConstantList.Where(p => p.PrimaryParameter == AllConstantParameters.AssetValuationByStatusType)
                .Select(p => p.Constant)
                .ToList();
        }
        void SetPPEValuation()
        {
            double valuation = 0;

            foreach (SQ sq in _game.SQList)
            {
                if(sq.OwnerNumber == _player.Number)
                {
                    valuation += sq.Production * _game.CommodityList[(int)sq.ResourceType].FourTurnMovingAvgPricing
                        * _ratiosForPPE[(int)sq.Status];
                }
            }

            Valuation = valuation;
        }
    }
}
