using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class NewMapInitializer
    {
        MapVM _mapVM;
        InfrastructureBuilder _infrastructureBuilder;
        PathCalculations _calc;
        TributaryPathList _tributaryPathList;

        List<SQ>[] _allInfrastructure = new List<SQ>[(int)InfrastructureType.Total];

        public NewMapInitializer(MapVM mapVM, InfrastructureBuilder infrastructureBuilder)
        {
            _mapVM = mapVM;
            _calc = new PathCalculations();
            _infrastructureBuilder = infrastructureBuilder;
            InitListOfInfrastructureSQs();
            _tributaryPathList = new TributaryPathList(_allInfrastructure[(int)InfrastructureType.Tributary]);
            InitInfrastructure();
        }
        private void InitListOfInfrastructureSQs()
        {
            using (DBContext db = new DBContext())
            {
                _allInfrastructure[(int)InfrastructureType.MainRiver] = db.SQ.Where(s => s.IsMainRiver == true)
                                                                    .OrderBy(s => s.Col)
                                                                    .ToList();
                _allInfrastructure[(int)InfrastructureType.Tributary] = db.SQ.Where(s => s.IsTributary == true).ToList();
                _allInfrastructure[(int)InfrastructureType.Road] = db.SQ.Where(s => s.IsMainTransportationCorridor == true ||
                                                                    s.IsRoadConnected == true)
                                                                    .ToList();
                _allInfrastructure[(int)InfrastructureType.Pipeline] = db.SQ.Where(s => s.IsPipelineConnected == true).ToList();
                _allInfrastructure[(int)InfrastructureType.RailRoad] = db.SQ.Where(s => s.IsTrainConnected == true || 
                                                                    s.IsMainTransportationCorridor == true)
                                                                    .ToList();
                _allInfrastructure[(int)InfrastructureType.Hub] = db.SQ.Where(s => s.IsHub == true).ToList();
            }
        }
        private void InitInfrastructure()
        {
            for (int i = 0; i < (int)InfrastructureType.Total; i++)
            {
                if((InfrastructureType)i == InfrastructureType.Hub) { InitHubs(); }
                else if((InfrastructureType)i == InfrastructureType.Tributary) { InitTributaries(); }
                else { DrawPathsOnCanvas((InfrastructureType)i); }
            }
        }
        private void DrawPathsOnCanvas(InfrastructureType it)
        {
            SKPath path = new SKPath();

            foreach (SQ sq in _allInfrastructure[(int)it])
            {
                if (sq.Col == 0) { _calc.setStartPoint(sq,ref path); }
                path.LineTo(_calc.GetInfrastructureSKPoint(sq,it));
            }
            path.Close();

            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                gameboard.DrawPath(path, _infrastructureBuilder.Formats[(int)it]);
                gameboard.Save();
            }
        }
        private void InitHubs()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                foreach (SQ sq in _allInfrastructure[(int)InfrastructureType.Hub])
                {
                    gameboard.DrawRect(_calc.GetHubRect(sq), _infrastructureBuilder.Formats[(int)InfrastructureType.Hub]);
                }
                gameboard.Save();
            }
        }
        private void InitTributaries()
        {

        }
    }
}
