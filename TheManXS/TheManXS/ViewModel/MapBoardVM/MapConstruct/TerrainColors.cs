using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public enum TerrainTypeE { Grassland,Forest,Mountain,River,Slough,Sand,Total }
    public class TerrainColors : List<List<SKColor>>
    {
        private System.Random rnd = new System.Random();
        public TerrainColors()
        {
            this.Add(GetGrasslandColors());
            this.Add(GetForestColors());
            this.Add(GetMountainColors());
            this.Add(GetRiverColors());
            this.Add(GetSloughColors());
            this.Add(GetSandColors());
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
        private List<SKColor> GetRiverColors()
        {
            return new List<SKColor>()
            {
                new SKColor(12,163,218),
                new SKColor(63,179,236),
                new SKColor(63,179,236),
                new SKColor(130,193,247),
                new SKColor(135,198,244),                
            };
        }
        private List<SKColor> GetSloughColors()
        {
            return new List<SKColor>()
            {
                new SKColor(13, 36, 31),
                new SKColor(15, 35, 24),
                new SKColor(23, 48, 29),
            };
        }
        private List<SKColor> GetSandColors()
        {
            return new List<SKColor>()
            {
                new SKColor(194, 178, 128),
            };
        }
    }
}
