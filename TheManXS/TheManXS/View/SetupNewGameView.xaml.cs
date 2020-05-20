using SkiaSharp;
using SkiaSharp.Views.Forms;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupNewGameView : ContentPage
    {
        public SetupNewGameView()
        {
            InitializeComponent();
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvasView = sender as SKCanvasView;
            SKCanvas canvas = e.Surface.Canvas;
            float distanceAboveAndLeftOfCenter = (float)(LogoSKCanvasView.Width * 0.4f);
            float width = (float)LogoSKCanvasView.Width;
            float distanceFromLeft = (float)(LogoSKCanvasView.Width / 2);

            float left = (float)LogoSKCanvasView.X - distanceAboveAndLeftOfCenter + distanceFromLeft;
            float top = (float)LogoSKCanvasView.Y - distanceAboveAndLeftOfCenter;
            float right = left + width;
            float bottom = top + width;

            SKRect rect = new SKRect(left, top, right, bottom);
            new Logo(canvas, rect);
        }
    }
}