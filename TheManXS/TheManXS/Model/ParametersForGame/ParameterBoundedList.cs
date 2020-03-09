using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TheManXS.Model.ParametersForGame
{
    public class ParameterBoundedList : List<ParameterBounded>
    {
        System.Random rnd = new System.Random();
        public ParameterBoundedList(bool isForSettingsVM) { }
        
        public ParameterBoundedList()
        {
            InitListOfEmptyParameters();
            ReadDataFromBinaryFile();
        }
        private void InitListOfEmptyParameters()
        {
            for (int primary = 0; primary < (int)AllBoundedParameters.Total; primary++)
            {
                for (int secondary = 0; secondary < getTotalOfElementsInSecondaryIndex(primary); secondary++)
                {
                    this.Add(new ParameterBounded(primary, secondary));
                }
            }
            int getTotalOfElementsInSecondaryIndex(int primaryIndex)
            {
                AllBoundedParameters a = (AllBoundedParameters)primaryIndex;
                switch (a)
                {                    
                    case AllBoundedParameters.CityProdutionPerCityDensity:
                        return (int)CityDensity.Total;

                    case AllBoundedParameters.DevelopmentCostPerTerrainType:
                    case AllBoundedParameters.ExploreCostPerTerrainType:
                    case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                        return (int)TerrainTypeE.Total;

                    case AllBoundedParameters.TerrainConstruct:
                        return (int)TerrainBoundedConstructSecondary.Total;

                    case AllBoundedParameters.PoolConstructParameters:
                        return (int)PoolConstructParametersSecondary.Total;

                    case AllBoundedParameters.ActionCosts:
                        return (int)ActionCostsSecondary.Total;

                    case AllBoundedParameters.ProductionUnitsPerTerrainType:
                        return (int)TerrainTypeE.Total;

                    case AllBoundedParameters.Total:
                    default:
                        return 0;
                }
            }
        }      
        private void ReadDataFromBinaryFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(App.ParameterBoundedPath, FileMode.OpenOrCreate)))
            {
                while (br.PeekChar() != (-1))
                {
                    int primaryIndex = br.ReadInt32();
                    int secondaryIndex = br.ReadInt32();

                    bool paramExists = this.Exists(p => p.PrimaryIndexNumber == primaryIndex && p.SecondaryParameterIndex == secondaryIndex);

                    if (paramExists)
                    {
                        ParameterBounded pb = this.Where(p => p.PrimaryIndexNumber == primaryIndex)
                            .Where(p => p.SecondaryParameterIndex == secondaryIndex)
                            .FirstOrDefault();

                        pb.LowerBounds = br.ReadDouble();
                        pb.UpperBounds = br.ReadDouble();
                    }
                    else { this.Add(new ParameterBounded(primaryIndex, secondaryIndex)); }                    
                }
                br.Close();
            }
        }
        public double GetRandomValue(AllBoundedParameters ap, int secondaryIndex)
        {
            ParameterBounded pb = this.Where(p => p.PrimaryParameter == ap)
                .Where(p => p.SecondaryParameterIndex == secondaryIndex)
                .FirstOrDefault();

            return (rnd.NextDouble() * (pb.UpperBounds - pb.LowerBounds) + pb.LowerBounds);
        }
        public void WriteDataToBinaryFile()
        {
            string boundedParameterFile = App.ParameterBoundedPath;
            File.Delete(boundedParameterFile);

            using (BinaryWriter bw = new BinaryWriter(File.Open(boundedParameterFile,FileMode.OpenOrCreate)))
            {                
                foreach (ParameterBounded pb in this)
                {
                    bw.Write(pb.PrimaryIndexNumber);
                    bw.Write(pb.SecondaryParameterIndex);
                    bw.Write(pb.LowerBounds);
                    bw.Write(pb.UpperBounds);
                }
                bw.Close();
            }
        }
    }
}
