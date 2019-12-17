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
        Builder _infrastructureBuilder;
        PathCalculations _calc;
        TributaryPathList _tributaryPathList;

        List<SQ>[] _allInfrastructure = new List<SQ>[(int)InfrastructureType.Total];

        public NewMapInitializer(MapVM mapVM, Builder infrastructureBuilder)
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
                _allInfrastructure[(int)InfrastructureType.MainRiver] = 
                    db.SQ.Where(s => s.IsMainRiver == true)
                        .ToList();
                _allInfrastructure[(int)InfrastructureType.Tributary] = db.SQ.Where(s => s.IsTributary == true).ToList();
                _allInfrastructure[(int)InfrastructureType.Road] = 
                    db.SQ.Where(s => s.IsMainTransportationCorridor == true ||
                        s.IsRoadConnected == true)
                        .ToList();
                _allInfrastructure[(int)InfrastructureType.Pipeline] = db.SQ.Where(s => s.IsPipelineConnected == true).ToList();
                _allInfrastructure[(int)InfrastructureType.RailRoad] = 
                    db.SQ.Where(s => s.IsTrainConnected == true || 
                        s.IsMainTransportationCorridor == true)
                        .ToList();
                _allInfrastructure[(int)InfrastructureType.Hub] = db.SQ.Where(s => s.IsHub == true).ToList();
            }
        }
        private void InitInfrastructure()
        {
            for (int i = 0; i < (int)InfrastructureType.Total; i++)
            {
                var sortedList = _allInfrastructure[(int)i].OrderBy(s => s.Row).ThenBy(s => s.Col).ToList();
                if ((InfrastructureType)i == InfrastructureType.Hub) { InitHubs(); }
                else
                {                    
                    CreateMainTransporationCorridor((InfrastructureType)i, sortedList);
                    CreateSmallPaths((InfrastructureType)i, sortedList);
                }
            }
        }
        private void CreateMainTransporationCorridor(InfrastructureType it, List<SQ> sortedList)
        {
            SKPath path = new SKPath();
            foreach (SQ sq in sortedList)
            {
                if (sq.IsMainTransportationCorridor || sq.IsMainRiver)
                {
                    if(_calc.isMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                    else { path.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); }
                }
            }
            path.Close();
            DrawPathsOnCanvas(path, it);
        }
        private void CreateSmallPaths(InfrastructureType it, List<SQ> sortedList)
        {
            SKPath path = new SKPath();
            foreach (SQ sq in sortedList)
            {
                AdjacentSQsList sl = new AdjacentSQsList(sq, sortedList);                

                for (int i = 0; i < sl.Count; i++)
                {
                    if (sl[i].HasTheSameInfrastructureType)
                    {                        
                        path.MoveTo(_calc.GetInfrastructureSKPoint(sq, it));
                        path.LineTo(_calc.GetInfrastructureSKPoint(sl[i].square, it));
                    }
                }
            }
            path.Close();
            DrawPathsOnCanvas(path, it);
        }
        private void DrawPathsOnCanvas(SKPath path, InfrastructureType it)
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                SKPaint paint = _infrastructureBuilder.Formats[(int)it];
                gameboard.DrawPath(path, paint);
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
    }
}
