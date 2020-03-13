using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.ParametersForGame;
using Xamarin.Forms;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using SkiaSharp.Views.Forms;

namespace TheManXS.ViewModel.Style
{
    public enum AvailablePaletteColors { M1, M2, M3, M4, M5, G1, G2, G3, G4, G5, F1, F2, F3, F4, F5,
        R1, R2, R3, R4, C1, C2, C3, C4, C5, S1, S2, S3, SAND, Beth, Total }

    public class PaletteColor
    {        
        public PaletteColor(Color color, string description, TerrainTypeE tt, AvailablePaletteColors apc)
        {
            Color = color;
            Description = description;
            TerrainType = tt;
            SKColor = Extensions.ToSKColor(Color);
            AvailablePaletteColors = apc;
            ID = (int)apc;
        }

        public int ID { get; set; }
        public Color Color { get; set; }
        public SKColor SKColor { get; set; }
        public string Description { get; set; }
        public TerrainTypeE TerrainType { get; set; }
        public AvailablePaletteColors AvailablePaletteColors { get; set; }

        public bool IsC0 { get; set; }
        public bool IsC1 { get; set; }
        public bool IsC2 { get; set; }
        public bool IsC3 { get; set; }
        public bool IsC4 { get; set; }
    }
}
