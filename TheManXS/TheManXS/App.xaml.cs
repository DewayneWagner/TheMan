using System;
using System.Collections.Generic;
using System.IO;
using TheManXS.Model.Main;
using TheManXS.View.DetailView;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using M = TheManXS.ViewModel.MapBoardVM;

namespace TheManXS
{
    public partial class App : Application
    {
        public enum ObjectsInPropertyDictionary { GameBoardVM,ScreenWidth,ScreenHeight,Orientation,Rotation,
            Density,ActiveSQ,ActivePlayer,ActiveUnit }
        public static string DataBaseLocation = string.Empty;
        public static string BinaryBackupPath = string.Empty;

        public App(string dbLocation, string binaryBackupPath)
        {
            DataBaseLocation = dbLocation;
            BinaryBackupPath = binaryBackupPath;

            InitScreenMetrics();
            InitializeComponent();

            MainPage = new NavigationPage(new MainMenuView());
            InitPropertyDictionary();
            
        }
        void InitPropertyDictionary()
        {
            Properties[Convert.ToString(ObjectsInPropertyDictionary.GameBoardVM)] = new M.GameBoardVM(true);
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActiveSQ)] = new TheManXS.Model.Main.SQ(true);
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActivePlayer)] = new TheManXS.Model.Main.Player();
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ActiveUnit)] = new TheManXS.Model.Units.Unit();
        }
        
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
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

            // Rotation (0, 90, 180, 270)
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.Rotation))] = mainDisplayInfo.Rotation;

            Properties[(Convert.ToString(ObjectsInPropertyDictionary.ScreenWidth))] = mainDisplayInfo.Width;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.ScreenHeight))] = mainDisplayInfo.Height;
            Properties[(Convert.ToString(ObjectsInPropertyDictionary.Density))] = mainDisplayInfo.Density;
        }
    }
}
