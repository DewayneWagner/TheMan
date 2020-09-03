using SkiaSharp;
using SkiaSharp.Views.Forms;
using TheManXS.ViewModel.DetailPages;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos;
using Xamarin.Forms;
using Xamarin.Forms.Markup.LeftToRight;
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
            new Logo(canvasView, canvas);
        }
    }
}