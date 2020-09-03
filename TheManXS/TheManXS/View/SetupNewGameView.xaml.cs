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
            new Logo(canvasView, canvas);
        }
    }
}