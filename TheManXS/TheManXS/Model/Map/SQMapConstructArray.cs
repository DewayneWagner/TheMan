﻿using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map
{
    public class SQMapConstructArray
    {
        private SQ[,] _mapArray;
        Game _game;
        public SQMapConstructArray(Game game)
        {
            _game = game;
            _mapArray = new SQ[QC.RowQ, QC.ColQ];
            InitNewMap();
        }
        public SQ CityStartSQ { get; set; }
        public SQ this[int r, int c]
        {
            get => _mapArray[r, c];
            set => _mapArray[r, c] = value;
        }
        public void InitNewMap()
        {
            for (int r = 0; r < QC.RowQ; r++)
            {
                for (int c = 0; c < QC.ColQ; c++)
                {
                    _mapArray[r, c] = new SQ(r, c, _game);
                }
            }
        }
        public SQList GetListOfSQs()
        {
            SQList _sqList = new SQList();
            for (int r = 0; r < _mapArray.GetLength(0); r++)
            {
                for (int c = 0; c < _mapArray.GetLength(1); c++)
                {
                    _sqList.Add(_mapArray[r, c]);
                }
            }
            return _sqList;
        }
    }
}
