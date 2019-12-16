using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;
using static TheManXS.Model.Settings.SettingsMaster;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class RiverBuilder
    {
        private bool[,] riverSQs = new bool[QC.ColQ, QC.RowQ];
        private System.Random rnd = new System.Random();
        private MapVM _mapVM;
        private WaterColors WaterColors { get; } = new WaterColors();
        TributaryPathList _riverSQList;

        SKPaint riverBank = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeJoin = SKStrokeJoin.Round,
        };

        SKPaint water = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round,
        };

        SKPaint tributary = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round,
        };

        public RiverBuilder(MapVM mapVM)
        {
            _mapVM = mapVM;
            _riverSQList = new TributaryPathList();
            InitVariables();
            InitMainRiver();
            InitTributaries();
        }        
        private void InitVariables()
        {
            riverBank.Color = WaterColors.GetRandomColor(WaterColors.WaterColorTypesE.Bank);
            riverBank.StrokeWidth = (float)(QC.SqSize * 0.3);

            water.Color = WaterColors.GetRandomColor(WaterColors.WaterColorTypesE.River);
            water.StrokeWidth = (float)(QC.SqSize * 0.2);

            tributary.Color = riverBank.Color;
            tributary.StrokeWidth = (float)(QC.SqSize * 0.1);
        }
        private void InitMainRiver()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                SKPath riverPath = _riverSQList.MainRiver;               
                
                gameboard.DrawPath(riverPath, riverBank);
                gameboard.DrawPath(riverPath, water);

                riverPath.Close();
                gameboard.Save();
            }
        }
        private void InitTributaries()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                for (int i = 0; i < _riverSQList.TributariesList.Count; i++)
                {
                    SKPath t = _riverSQList.TributariesList[i];
                    
                    gameboard.DrawPath(t, tributary);
                    t.Close();
                }
                gameboard.Save();
            }
        }
    }
}
