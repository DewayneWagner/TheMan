using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;
using TheManXS.ViewModel.Services;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SKGameBoard : ContentPage
    {
        SKPaint _square = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            StrokeWidth = QC.SqSize,
        };
        private PageService _pageServices;
        private int _sqSize = QC.SqSize;
        public SKGameBoard()
        {
            InitializeComponent();
            _pageServices = new PageService();
        }

        private void gameboard_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Black);
            SQ sq;

            using(DBContext db = new DBContext())
            {
                for (int row = 0; row < QC.RowQ; row++)
                {
                    for (int col = 0; col < QC.ColQ; col++)
                    {
                        sq = db.SQ.Find(Coordinate.GetSQKey(row, col));
                        _square.Color = GetTerrainColor(sq.TerrainType);
                        canvas.DrawRect(col * _sqSize, row * _sqSize, _sqSize, _sqSize, _square);
                    }
                }
            }
        }
        private SKColor GetTerrainColor(TT tt)
        {
            switch (tt)
            {
                case TT.Grassland:
                    return SKColors.LightGreen;
                case TT.Forest:
                    return SKColors.ForestGreen;
                case TT.Mountain:
                    return SKColors.SlateGray;
                case TT.City:
                    return SKColors.CornflowerBlue;
                case TT.Total:
                default:
                    return SKColors.White;
            }
        }
        void skiastuff()
        {
            //SKSurface surface = e.Surface;
            //SKCanvas canvas = surface.Canvas;
            //canvas.Clear(SKColors.Blue);

            //int width = e.Info.Width;
            //int height = e.Info.Height;

            //canvas.Translate(width / 2, height / 2);
            //canvas.Scale(width / 400f);

            //canvas.DrawCircle(0, 0, 100, blackFillPaint);

            //DateTime dateTime = DateTime.Now;

            //for (int angle = 0; angle < 360; angle += 6)
            //{
            //    canvas.DrawCircle(0, -90, angle % 30 == 0 ? 4 : 2, whiteFillPaint);
            //    canvas.RotateDegrees(6);
            //}

            //canvas.Save();
            //canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            //whiteStrokePaint.StrokeWidth = 15;
            //canvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            //canvas.Restore();

            //canvas.Save();
            //canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            //whiteStrokePaint.StrokeWidth = 10;
            //canvas.DrawLine(0, 0, 0, -70, whiteStrokePaint);
            //canvas.Restore();

            //canvas.Save();
            //float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            //canvas.RotateDegrees(6 * seconds);
            //whiteStrokePaint.StrokeWidth = 2;
            //canvas.DrawLine(0, 0, 0, -80, whiteStrokePaint);
            //canvas.Restore();
        }

        private async void gameboard_Touch(object sender, SKTouchEventArgs e)
        {
            SKPoint touchPoint = e.Location;

            string message = "Screen Tapped at: " + "\n" +
                "X: " + Convert.ToString(touchPoint.X) + "\n" +
                "Y: " + Convert.ToString(touchPoint.Y);

            await _pageServices.DisplayAlert(message);
        }
    }
}