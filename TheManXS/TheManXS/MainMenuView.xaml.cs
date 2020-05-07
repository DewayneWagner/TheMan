using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel.DetailPages;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.View.DetailView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
            InitializeComponent();
            MainMenuVM mmvm = new MainMenuVM();
            Content.BindingContext = mmvm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //initialize Quick Constants here, so that screen size is available
            new QC();
            QC.ScreenHeight = (int)this.Height;
            QC.ScreenWidth = (int)this.Width;
            QC.Rotation = this.Rotation;
        }

        private void logoCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvasView = sender as SKCanvasView;
            SKCanvas canvas = e.Surface.Canvas;
            float logoWidthRatio = 0.6f;
            float sqSize = QC.ScreenWidth * logoWidthRatio;
            float logoFromEdge = (float)(logoCanvasView.X + logoCanvasView.Width / 2);

            float left = (float)logoCanvasView.X + logoFromEdge;
            float top = (float)logoCanvasView.Y;
            float right = left + sqSize;
            float bottom = top + sqSize;

            SKRect rect = new SKRect(left, top, right, bottom);
            new Logo(canvas, rect);
        }
    }
}