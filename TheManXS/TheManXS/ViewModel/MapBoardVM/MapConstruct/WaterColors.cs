using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class WaterColors : List<List<SKColor>>
    {
        public enum WaterColorTypesE { River, Slough, Bank }
        System.Random rnd = new System.Random();
        public WaterColors()
        {
            this.Add(GetRiverColors());
            this.Add(GetSloughColors());
            this.Add(GetSandColors());
        }
        public SKColor GetRandomColor(WaterColorTypesE w) => this[(int)w][rnd.Next(this[(int)w].Count)];
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
