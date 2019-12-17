﻿using SkiaSharp;
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
        //TributaryPathList _tributaryPathList;        

        List<SQ>[] _allInfrastructure = new List<SQ>[(int)InfrastructureType.Total];
        List<SKPath> _listOfAllSKPaths = new List<SKPath>((int)InfrastructureType.Total);

        public NewMapInitializer(MapVM mapVM, Builder infrastructureBuilder)
        {
            _mapVM = mapVM;
            _calc = new PathCalculations();
            _infrastructureBuilder = infrastructureBuilder;
            InitListOfInfrastructureSQs();
            InitInfrastructure();
        }
        private void InitListOfInfrastructureSQs()
        {
            using (DBContext db = new DBContext())
            {
                _allInfrastructure[(int)InfrastructureType.MainRiver] = db.SQ.Where(s => s.IsMainRiver == true).ToList();

                _allInfrastructure[(int)InfrastructureType.Tributary] = db.SQ.Where(s => s.IsTributary == true).ToList();

                _allInfrastructure[(int)InfrastructureType.Road] = db.SQ.Where(s => s.IsRoadConnected == true ||
                        s.IsMainTransportationCorridor == true ).ToList();

                _allInfrastructure[(int)InfrastructureType.Pipeline] = db.SQ.Where(s => s.IsPipelineConnected == true ||
                        s.IsMainTransportationCorridor == true).ToList();

                _allInfrastructure[(int)InfrastructureType.RailRoad] = db.SQ.Where(s => s.IsTrainConnected == true || 
                        s.IsMainTransportationCorridor == true).ToList();

                _allInfrastructure[(int)InfrastructureType.Hub] = db.SQ.Where(s => s.IsHub == true).ToList();
            }
        }
        private void InitInfrastructure()
        {
            for (int i = 0; i < (int)InfrastructureType.Total; i++)
            {
                var sortedList = _allInfrastructure[(int)i].OrderBy(s => s.Col).ToList();

                if ((InfrastructureType)i == InfrastructureType.Hub) { InitHubs(); }
                else if((InfrastructureType)i == InfrastructureType.MainRiver) { CreateMainRiver(sortedList); }
                else
                {                    
                    CreateMainTransporationCorridor((InfrastructureType)i, sortedList);
                    //CreateSmallPaths((InfrastructureType)i, sortedList);
                }
            }
            DrawPathsOnCanvas();
        }
        private void CreateMainTransporationCorridor(InfrastructureType it, List<SQ> sortedList)
        {
            SKPath path = new SKPath();

            foreach (SQ sq in sortedList)
            {
                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                else { path.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); }
            }
            path.Close();
            _listOfAllSKPaths.Add(path);
        }
        private void CreateMainRiver(List<SQ> sortedList)
        {
            SKPath river = new SKPath();
            InfrastructureType it = InfrastructureType.MainRiver;
            SQ sq;

            for (int i = 0; i < sortedList.Count; i++)
            {
                sq = sortedList[i];

                int row = sq.Row;
                int col = sq.Col;

                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref river, it); }
                else { river.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); }
            }

            //foreach (SQ sq in sortedList)
            //{
            //    if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref river, it); }
            //    else { river.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); }
            //}
            river.Close();
            DrawPathsOnCanvas(river, it);
        }
        private void CreateMainTransporationCorridor(InfrastructureType it, List<SQ> sortedList, bool ignoreThisMethodForNow)
        {
            SKPath path = new SKPath();
            foreach (SQ sq in sortedList)
            {
                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                else { path.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); }
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
        private void DrawPathsOnCanvas()
        {
            using (SKCanvas gameBoard = new SKCanvas(_mapVM.Map))
            {
                for (int i = 0; i < _listOfAllSKPaths.Count; i++)
                {
                    SKPaint paint = _infrastructureBuilder.Formats[i];
                    gameBoard.DrawPath(_listOfAllSKPaths[i], paint);
                    gameBoard.Save();
                }
            }
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
