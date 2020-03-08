using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class TerrainColors : List<List<SKColor>>
    {
        private System.Random rnd = new System.Random();
        public TerrainColors()
        {           
            this.Add(GetGrasslandColors());
            this.Add(GetForestColors());
            this.Add(GetMountainColors());
            this.Add(GetCityColors());            
        }
        public SKColor GetRandomColor(TerrainTypeE tt) => this[(int)tt][rnd.Next(this[(int)tt].Count)];
        public SKColor GetBackGroundColor(TerrainTypeE tt) => this[(int)tt][0];
        
        private List<SKColor> GetGrasslandColors()
        {
            return new List<SKColor>()
            {
                new SKColor(182, 171, 141),
                new SKColor(63, 92, 64),
                new SKColor(109, 122, 96),
                new SKColor(147, 160, 136),                
                new SKColor(116, 145, 117),
            };
        }
        private List<SKColor> GetForestColors()
        {
            return new List<SKColor>()
            {
                new SKColor(57,89,68),
                new SKColor(26, 57, 49),
                new SKColor(37,72,52),                
                new SKColor(80,106,97),
                new SKColor(40,72,67),
            };
        }
        private List<SKColor> GetMountainColors()
        {
            return new List<SKColor>()
            {
                new SKColor(126,124,124),
                new SKColor(63,67,67),
                new SKColor(82,81,73),                
                new SKColor(90,90,76),                
                new SKColor(98,93,84),
            };
        }
        private List<SKColor> GetCityColors()
        {
            return new List<SKColor>() { SKColors.Black };
            //return new List<SKColor>()
            //{
            //    new SKColor(95,92,104),
            //    new SKColor(168,165,161),
            //    new SKColor(96,92,99),
            //    new SKColor(97,100,123),
            //    new SKColor(157,170,186),
            //};
        }
        
    }
}
