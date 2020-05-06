using System;
using System.Collections.Generic;
using System.IO;
using TheManXS.Model.Main;
using TheManXS.Services.IO;
using TheManXS.View.DetailView;
using TheManXS.ViewModel;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using M = TheManXS.ViewModel.MapBoardVM;

namespace TheManXS
{
    public partial class App : Application
    {
        public enum FileNames { DB, ParameterBounded, ParameterConstant, ColorPalette, MapGame1Type0, MapGame1Type1, MapGame2Type0, MapGame2Type1, MapGame3Type0, MapGame3Type1, Total }
        public enum ObjectsInPropertyDictionary { MapVM, ScreenWidth, ScreenHeight, Orientation,
            Rotation, Density, ActiveSQ, ActivePlayer, ActiveUnit, Game, ApplicationVM }

        public static PathList PathList;
        public static bool MapHasBeenCreated;
        private ApplicationVM _applicationVM;

        public App(PathList pathList)
        {
            PathList = pathList;

            InitScreenMetrics();
            InitializeComponent();

            BindingContext = _applicationVM = new ApplicationVM();

            MainPage = new NavigationPage(new MainMenuView());
            InitPropertyDictionary();
        }

        void InitPropertyDictionary()
        {
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActiveSQ)] = new TheManXS.Model.Main.SQ(true);
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActivePlayer)] = new TheManXS.Model.Main.Player();
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActiveUnit)] = new TheManXS.Model.Units.Unit();
            Properties[Convert.ToString(ObjectsInPropertyDictionary.Game)] = new Game(true);
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ApplicationVM)] = _applicationVM;
        }
        
        protected override void OnStart()
        {
            // Handle when your app starts
            _applicationVM.InitMainColors();
        }
        
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            if (MapHasBeenCreated)
            {
                Game thisGame = (Game)Properties[Convert.ToString(ObjectsInPropertyDictionary.Game)];
                new SaveGameAction(thisGame);
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        private void InitScreenMetrics()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Orientation (Landscape, Portrait, Square, Unknown)
            var orientation = mainDisplayInfo.Orientation;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.Orientation))] = mainDisplayInfo.Orientation;

            //Rotation(0, 90, 180, 270)
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.Rotation))] = mainDisplayInfo.Rotation;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.ScreenWidth))] = mainDisplayInfo.Width;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.ScreenHeight))] = mainDisplayInfo.Height;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.Density))] = mainDisplayInfo.Density;
        }
        public void UpdateColors()
        {
            _applicationVM.InitMainColors();
        }
        public static string GetFolderPath(FileNames fileName) => PathList[(int)fileName];
    }
}
