using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TheManXS.Model.Main;
using SkiaSharp;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Reflection;

namespace TheManXS.Services.IO
{
    public class SavedMap
    {
        Game _game;
        string _imagePath = @"C:\Users\deway\source\repos\TheManXS\TheManXS\TheManXS\Graphics\SavedMaps\";
        public enum SavedMapType { Raw, InProgressGame }
        public SavedMap(Game game) { _game = game; }

        private SavedMapType savedMapType => _game.TurnNumber == 0 ? SavedMapType.Raw : SavedMapType.InProgressGame;
        private string filePath
        {
            get
            {
                string fileName = "MapGame" + Convert.ToString(QC.CurrentSavedGameSlot) + "Type" + Convert.ToString((int)savedMapType);
                App.FileNames fileNameType = App.FileNames.MapGame1Type0;

                for (int i = 0; i < (int)App.FileNames.Total; i++)
                {
                    if(Convert.ToString((App.FileNames)i) == fileName) { fileNameType = (App.FileNames)i; }
                }

                return App.GetFolderPath(fileNameType);
            }
        }

        public void SaveMap()
        {
            using (var mapImage = SKImage.FromBitmap(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                using (var mapData = mapImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(filePath))
                    {
                        mapData.SaveTo(stream);
                    }
                }
            }
        }
        public SKBitmap LoadMap()
        {
            SKBitmap map;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(filePath))
            {
                map = SKBitmap.Decode(stream);
            }
            return map;
        }
    }
}
