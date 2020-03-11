using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.ParametersForGame;
using Xamarin.Forms;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using ACP = TheManXS.ViewModel.Style.AvailablePaletteColors;

namespace TheManXS.ViewModel.Style
{
    public class PaletteColorList : List<PaletteColor>
    {
        System.Random rnd = new System.Random();
        public PaletteColorList()
        {
            InitColors();
        }
        void InitColors()
        {
            Add(new PaletteColor(Color.FromRgb(126, 124, 124), "Mountain 1", TT.Mountain, ACP.M1));
            Add(new PaletteColor(Color.FromRgb(63, 67, 67), "Mountain 2", TT.Mountain, ACP.M2));
            Add(new PaletteColor(Color.FromRgb(82, 81, 73), "Mountain 3", TT.Mountain, ACP.M3));
            Add(new PaletteColor(Color.FromRgb(90, 90, 76), "Mountain 4", TT.Mountain, ACP.M4));
            Add(new PaletteColor(Color.FromRgb(98, 93, 84), "Mountain 5", TT.Mountain, ACP.M5));
            Add(new PaletteColor(Color.FromRgb(182, 171, 141), "Grassland 1", TT.Grassland, ACP.G1));
            Add(new PaletteColor(Color.FromRgb(63, 92, 64), "Grassland 2", TT.Grassland, ACP.G2));
            Add(new PaletteColor(Color.FromRgb(109, 122, 96), "Grassland 3", TT.Grassland, ACP.G3));
            Add(new PaletteColor(Color.FromRgb(147, 160, 136), "Grassland 4", TT.Grassland, ACP.G4));
            Add(new PaletteColor(Color.FromRgb(116, 145, 117), "Grassland 5", TT.Grassland, ACP.G5));
            Add(new PaletteColor(Color.FromRgb(57, 89, 68), "Forest 1", TT.Forest, ACP.F1));
            Add(new PaletteColor(Color.FromRgb(26, 57, 49), "Forest 2", TT.Forest, ACP.F2));
            Add(new PaletteColor(Color.FromRgb(37, 72, 52), "Forest 3", TT.Forest, ACP.F3));
            Add(new PaletteColor(Color.FromRgb(80, 106, 97), "Forest 4", TT.Forest, ACP.F4));
            Add(new PaletteColor(Color.FromRgb(40, 72, 67), "Forest 5", TT.Forest, ACP.F5));
            Add(new PaletteColor(Color.FromRgb(12, 163, 218), "River 1", TT.River, ACP.R1));
            Add(new PaletteColor(Color.FromRgb(63, 179, 236), "River 2", TT.River, ACP.R2));
            Add(new PaletteColor(Color.FromRgb(130, 193, 247), "River 3", TT.River, ACP.R3));
            Add(new PaletteColor(Color.FromRgb(135, 198, 244), "River 4", TT.River, ACP.R4));
            Add(new PaletteColor(Color.FromRgb(95, 92, 104), "City 1", TT.City, ACP.C1));
            Add(new PaletteColor(Color.FromRgb(168, 165, 161), "City 2", TT.City, ACP.C2));
            Add(new PaletteColor(Color.FromRgb(96, 92, 99), "City 3", TT.City, ACP.C3));
            Add(new PaletteColor(Color.FromRgb(97, 100, 123), "City 4", TT.City, ACP.C4));
            Add(new PaletteColor(Color.FromRgb(157, 170, 186), "City 5", TT.City, ACP.C5));
            Add(new PaletteColor(Color.FromRgb(13, 36, 31), "Slough 1", TT.Slough, ACP.S1));
            Add(new PaletteColor(Color.FromRgb(15, 35, 24), "Slough 2", TT.Slough, ACP.S2));
            Add(new PaletteColor(Color.FromRgb(23, 48, 29), "Slough 3", TT.Slough, ACP.S3));
            Add(new PaletteColor(Color.FromRgb(194, 178, 128), "Sand 1", TT.Sand, ACP.SAND));
        }
        public SKColor GetRandomColor(TT tt)
        {
            List<SKColor> terrainColors = this.Where(p => p.TerrainType == tt)
                .Select(p => p.SKColor)
                .ToList();
            return terrainColors[rnd.Next(terrainColors.Count)];
        }
    }
}
