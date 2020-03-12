using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.ParametersForGame;
using Xamarin.Forms;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using ACP = TheManXS.ViewModel.Style.AvailablePaletteColors;
using System.IO;

namespace TheManXS.ViewModel.Style
{
    public class PaletteColorList : List<PaletteColor>
    {
        System.Random rnd = new System.Random();
        public enum ColorTypes { C0, C1, C2, C3, C4, Total }
        public PaletteColorList()
        {
            InitColors();
            ReadPColorsFromBinaryFile();
        }

        public PaletteColor C0 { get; set; }
        public PaletteColor C1 { get; set; }
        public PaletteColor C2 { get; set; }
        public PaletteColor C3 { get; set; }
        public PaletteColor C4 { get; set; }

        void InitColors()
        {
            Add(new PaletteColor(Color.FromRgb(126, 124, 124), "Banff 1", TT.Mountain, ACP.M1));
            Add(new PaletteColor(Color.FromRgb(63, 67, 67), "Banff 2", TT.Mountain, ACP.M2));
            Add(new PaletteColor(Color.FromRgb(82, 81, 73), "Banff 3", TT.Mountain, ACP.M3));
            Add(new PaletteColor(Color.FromRgb(90, 90, 76), "Banff 4", TT.Mountain, ACP.M4));
            Add(new PaletteColor(Color.FromRgb(98, 93, 84), "Banff 5", TT.Mountain, ACP.M5));
            Add(new PaletteColor(Color.FromRgb(182, 171, 141), "Salvador 1", TT.Grassland, ACP.G1));
            Add(new PaletteColor(Color.FromRgb(63, 92, 64), "Salvador 2", TT.Grassland, ACP.G2));
            Add(new PaletteColor(Color.FromRgb(109, 122, 96), "Salvador 3", TT.Grassland, ACP.G3));
            Add(new PaletteColor(Color.FromRgb(147, 160, 136), "Salvador 4", TT.Grassland, ACP.G4));
            Add(new PaletteColor(Color.FromRgb(116, 145, 117), "Salvador 5", TT.Grassland, ACP.G5));
            Add(new PaletteColor(Color.FromRgb(57, 89, 68), "Conklin 1", TT.Forest, ACP.F1));
            Add(new PaletteColor(Color.FromRgb(26, 57, 49), "Conklin 2", TT.Forest, ACP.F2));
            Add(new PaletteColor(Color.FromRgb(37, 72, 52), "Conklin 3", TT.Forest, ACP.F3));
            Add(new PaletteColor(Color.FromRgb(80, 106, 97), "Conklin 4", TT.Forest, ACP.F4));
            Add(new PaletteColor(Color.FromRgb(40, 72, 67), "Conklin 5", TT.Forest, ACP.F5));
            Add(new PaletteColor(Color.FromRgb(12, 163, 218), "Morraine Lake 1", TT.River, ACP.R1));
            Add(new PaletteColor(Color.FromRgb(63, 179, 236), "Morraine Lake 2", TT.River, ACP.R2));
            Add(new PaletteColor(Color.FromRgb(130, 193, 247), "Morraine Lake 3", TT.River, ACP.R3));
            Add(new PaletteColor(Color.FromRgb(135, 198, 244), "Morraine Lake 4", TT.River, ACP.R4));
            Add(new PaletteColor(Color.FromRgb(95, 92, 104), "City 1", TT.City, ACP.C1));
            Add(new PaletteColor(Color.FromRgb(168, 165, 161), "City 2", TT.City, ACP.C2));
            Add(new PaletteColor(Color.FromRgb(96, 92, 99), "City 3", TT.City, ACP.C3));
            Add(new PaletteColor(Color.FromRgb(97, 100, 123), "City 4", TT.City, ACP.C4));
            Add(new PaletteColor(Color.FromRgb(157, 170, 186), "City 5", TT.City, ACP.C5));
            Add(new PaletteColor(Color.FromRgb(13, 36, 31), "Sask Slough 1", TT.Slough, ACP.S1));
            Add(new PaletteColor(Color.FromRgb(15, 35, 24), "Sask Slough 2", TT.Slough, ACP.S2));
            Add(new PaletteColor(Color.FromRgb(23, 48, 29), "Sask Slough 3", TT.Slough, ACP.S3));
            Add(new PaletteColor(Color.FromRgb(194, 178, 128), "Sand 1", TT.Sand, ACP.SAND));
        }
        public SKColor GetRandomColor(TT tt)
        {
            List<SKColor> terrainColors = this.Where(p => p.TerrainType == tt)
                .Select(p => p.SKColor)
                .ToList();

            return terrainColors[rnd.Next(terrainColors.Count)];
        }
        private void ReadPColorsFromBinaryFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(App.ColorPalettes, FileMode.OpenOrCreate)))
            {
                while (br.PeekChar() != (-1))
                {
                    this[br.ReadInt32()].IsC0 = true;
                    this[br.ReadInt32()].IsC1 = true;
                    this[br.ReadInt32()].IsC2 = true;
                    this[br.ReadInt32()].IsC3 = true;
                    this[br.ReadInt32()].IsC4 = true;                    
                }
                br.Close();
            }
            DeleteColorPaletteBinaryFile();
        }
        private void DeleteColorPaletteBinaryFile()
        {
            string colorPaletteFile = App.ColorPalettes;
            File.Delete(colorPaletteFile);
        }
        public void WritePColorsToBinaryFile()
        {
            DeleteColorPaletteBinaryFile();
            using (BinaryWriter bw = new BinaryWriter(File.Open(App.ColorPalettes, FileMode.OpenOrCreate)))
            {
                List<int> pList = getIndexOfPs();
                foreach (int p in pList) { bw.Write(p); }
                bw.Close();
            }
            List<int> getIndexOfPs()
            {
                int c0 = (int)this.Where(c => c.IsC0).Select(c => c.AvailablePaletteColors).FirstOrDefault();
                int c1 = (int)this.Where(c => c.IsC1).Select(c => c.AvailablePaletteColors).FirstOrDefault();
                int c2 = (int)this.Where(c => c.IsC2).Select(c => c.AvailablePaletteColors).FirstOrDefault();
                int c3 = (int)this.Where(c => c.IsC3).Select(c => c.AvailablePaletteColors).FirstOrDefault();
                int c4 = (int)this.Where(c => c.IsC4).Select(c => c.AvailablePaletteColors).FirstOrDefault();

                return new List<int>() { c0, c1, c2, c3, c4 };
            }
        }
    }
}
