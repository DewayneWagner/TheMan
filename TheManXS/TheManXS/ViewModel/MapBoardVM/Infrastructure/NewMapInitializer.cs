﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using IT = TheManXS.Model.Settings.SettingsMaster.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class NewMapInitializer
    {
        MapVM _mapVM;
        Builder _infrastructureBuilder;
        PathCalculations _calc;

        //List<SQ>[] _allInfrastructure = new List<SQ>[(int)IT.Total];
        List<SQ_Infrastructure>[] _allInfrastructure = new List<SQ_Infrastructure>[(int)IT.Total];
        List<SKPath> _listOfAllSKPaths = new List<SKPath>((int)IT.Total);

        public NewMapInitializer(MapVM mapVM, Builder infrastructureBuilder)
        {
            _mapVM = mapVM;
            _calc = new PathCalculations();
            _infrastructureBuilder = infrastructureBuilder;
            InitListOfInfrastructureSQs();
            InitInfrastructure();
            DrawAllPathsOnCanvas();
            InitHubs();
        }
        private void InitListOfInfrastructureSQs()
        {
            using (DBContext db = new DBContext())
            {
                _allInfrastructure[(int)IT.MainRiver] = db.Infrastructure.Where(i => i.IsMainRiver == true).ToList();

                _allInfrastructure[(int)IT.Tributary] = db.Infrastructure.Where(i => i.IsTributary == true).ToList();

                _allInfrastructure[(int)IT.Road] = db.Infrastructure.Where(i => i.IsRoadConnected == true ||
                    i.IsMainTransportationCorridor == true).ToList();

                _allInfrastructure[(int)IT.Pipeline] = db.Infrastructure.Where(i => i.IsPipelineConnected ||
                    i.IsMainTransportationCorridor == true).ToList();

                _allInfrastructure[(int)IT.RailRoad] = db.Infrastructure.Where(i => i.IsTrainConnected == true ||
                    i.IsMainTransportationCorridor == true).ToList();

                _allInfrastructure[(int)IT.Hub] = db.Infrastructure.Where(i => i.IsHub == true).ToList();
            }
        }
        private void InitListOfInfrastructureSQs(bool isOldVersion)
        {
            using (DBContext db = new DBContext())
            {
                //_allInfrastructure[(int)IT.MainRiver] = db.SQ.Where(s => s.IsMainRiver == true).ToList();

                //_allInfrastructure[(int)IT.Tributary] = db.SQ.Where(s => s.IsTributary == true).ToList();

                //_allInfrastructure[(int)IT.Road] = db.SQ.Where(s => s.IsRoadConnected == true ||
                //        s.IsMainTransportationCorridor == true ).ToList();

                //_allInfrastructure[(int)IT.Pipeline] = db.SQ.Where(s => s.IsPipelineConnected == true ||
                //        s.IsMainTransportationCorridor == true).ToList();

                //_allInfrastructure[(int)IT.RailRoad] = db.SQ.Where(s => s.IsTrainConnected == true || 
                //        s.IsMainTransportationCorridor == true).ToList();

                //_allInfrastructure[(int)IT.Hub] = db.SQ.Where(s => s.IsHub == true).ToList();
            }
        }
        private void InitInfrastructure()
        {
            for (int i = 0; i < (int)IT.Total; i++)
            {
                var sortedList = _allInfrastructure[i].OrderBy(s => s.Col).ToList();
                IT it = (IT)i;

                if(it != IT.Hub && it != IT.Tributary)
                { CreateMainTransporationCorridorAndMainRiver(it, sortedList); }
                if (it != IT.Hub && it != IT.MainRiver)
                { CreateSmallPaths((IT)i, _allInfrastructure[i].OrderBy(s => s.Col).ThenBy(s => s.Row).ToList()); }
            }
        }
        private void CreateMainTransporationCorridorAndMainRiver(IT it, List<SQ_Infrastructure> sortedList)
        {
            SKPath path = new SKPath();

            foreach (SQ_Infrastructure sq in sortedList)
            {
                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                path.LineTo(_calc.GetInfrastructureSKPoint(sq, it)); 
                if(!sq.IsHub) { _allInfrastructure[(int)it].Remove(sq); }
            }
            path.Close();
            _listOfAllSKPaths.Add(path);
        }

        private static int[] R = new int[(int)AdjSqsDirection.Total] { 0, 1, 1, 1 };
        private static int[] C = new int[(int)AdjSqsDirection.Total] { 1, 1, 0, -1 };
        private enum AdjSqsDirection { E, SE, S, SW, Total }

        private void CreateSmallPaths(IT it, List<SQ_Infrastructure> doubleSortedList)
        {
            SKPath path;
            if(it == IT.Tributary) 
            { 
                path = new SKPath();
                _listOfAllSKPaths.Add(path);            
            }
            else { path = _listOfAllSKPaths[(int)it]; }

            foreach (SQ_Infrastructure sq in doubleSortedList)
            {
                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                path.MoveTo(_calc.GetInfrastructureSKPoint(sq, it));
                for (int i = 0; i < (int)AdjSqsDirection.Total; i++)
                {
                    SQ_Infrastructure adjSQ = doubleSortedList.Where(s => s.Row == (sq.Row + R[i]))
                        .Where(s => s.Col == sq.Col + C[i])
                        .FirstOrDefault();
                    if(adjSQ != null) { path.LineTo(_calc.GetInfrastructureSKPoint(adjSQ, it)); }
                }
            }
            path.Close();
        }
        private void DrawAllPathsOnCanvas()
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
        private void InitHubs()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                foreach (SQ_Infrastructure sq in _allInfrastructure[(int)IT.Hub])
                {
                    gameboard.DrawRect(_calc.GetHubRect(sq), _infrastructureBuilder.Formats[(int)IT.Hub]);
                }
                gameboard.Save();
            }
        }
    }
}
