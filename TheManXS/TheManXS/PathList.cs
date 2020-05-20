using System;
using System.Collections.Generic;
using System.IO;

namespace TheManXS
{
    public class PathList : List<string>
    {
        private string[] _fileNamesArray;
        public PathList(string folderPath)
        {
            _fileNamesArray = new string[(int)App.FileNames.Total];
            LoadFileNameList();
            CreatePathList(folderPath);
        }
        private void LoadFileNameList()
        {
            _fileNamesArray[(int)App.FileNames.ColorPalette] = "ColorPalette.bin";
            _fileNamesArray[(int)App.FileNames.DB] = "TheManXS_db.sqlite";
            _fileNamesArray[(int)App.FileNames.ParameterBounded] = "ParameterBounded.bin";
            _fileNamesArray[(int)App.FileNames.ParameterConstant] = "ParameterConstant.bin";

            for (int i = (int)App.FileNames.MapGame1Type0; i <= (int)App.FileNames.MapGame3Type1; i++)
            {
                _fileNamesArray[i] = (Convert.ToString((App.FileNames)i) + ".png");
            }
        }
        private void CreatePathList(string folderPath)
        {
            for (int i = 0; i < (int)App.FileNames.Total; i++)
            {
                string filePath = Path.Combine(folderPath, _fileNamesArray[i]);
                this.Add(filePath);
            }
        }
    }
}
