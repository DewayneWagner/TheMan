using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using TheManXS.ViewModel.DetailPages;

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
                    this.Add(new ParameterBounded(primary, GetSecondarySubIndexName(primary,secondary),secondary));
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

                    case AllBoundedParameters.NextParameterSet1:
                        return (int)NextParameterSet1SecondaryIndex.Total;

                    case AllBoundedParameters.NextParameterSet2:
                        return (int)NextParameterSet2SecondaryIndex.Total;

                    case AllBoundedParameters.NextParameterSet3:
                        return (int)NextParameterSet3SecondaryIndex.Total;

                    case AllBoundedParameters.Total:
                    default:
                        return 0;
                }
            }
        }
        string GetSecondarySubIndexName(int primaryIndexNumber, int secondaryIndexNumber)
        {
            AllBoundedParameters a = (AllBoundedParameters)primaryIndexNumber;
            switch (a)
            {
                case AllBoundedParameters.CityProdutionPerCityDensity:
                    return Convert.ToString((CityDensity)secondaryIndexNumber);

                case AllBoundedParameters.DevelopmentCostPerTerrainType:
                case AllBoundedParameters.ExploreCostPerTerrainType:
                case AllBoundedParameters.ProductionUnitsPerTerrainType:
                case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                    return Convert.ToString((TerrainTypeE)secondaryIndexNumber);

                case AllBoundedParameters.TerrainConstruct:
                    return Convert.ToString((TerrainBoundedConstructSecondary)secondaryIndexNumber);

                case AllBoundedParameters.PoolConstructParameters:
                    return Convert.ToString((PoolConstructParametersSecondary)secondaryIndexNumber);

                case AllBoundedParameters.ActionCosts:
                    return Convert.ToString((ActionCostsSecondary)secondaryIndexNumber);

                case AllBoundedParameters.NextParameterSet1:
                    return Convert.ToString((NextParameterSet1SecondaryIndex)secondaryIndexNumber);

                case AllBoundedParameters.NextParameterSet2:
                    return Convert.ToString((NextParameterSet2SecondaryIndex)secondaryIndexNumber);

                case AllBoundedParameters.NextParameterSet3:
                    return Convert.ToString((NextParameterSet3SecondaryIndex)secondaryIndexNumber);

                case AllBoundedParameters.Total:
                default:
                    break;
            }
            return null;
        }

        private void ReadDataFromBinaryFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(App.ParameterBoundedPath, FileMode.OpenOrCreate)))
            {
                while (br.PeekChar() != (-1))
                {
                    int primaryIndex = br.ReadInt32();
                    string secondaryIndexSubType = br.ReadString();

                    bool paramExists = this.Exists(p => p.PrimaryIndexNumber == primaryIndex && p.SecondaryParameterSubIndex == secondaryIndexSubType);

                    if (paramExists)
                    {
                        ParameterBounded pb = this.Where(p => p.PrimaryIndexNumber == primaryIndex)
                            .Where(p => p.SecondaryParameterSubIndex == secondaryIndexSubType)
                            .FirstOrDefault();

                        pb.LowerBounds = br.ReadDouble();
                        pb.UpperBounds = br.ReadDouble();
                    }
                    else { this.Add(new ParameterBounded(primaryIndex, secondaryIndexSubType)); }                    
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

            //if (this.Exists(p => p.PrimaryParameter == ap && p.SecondaryParameterIndex == secondaryIndex))
            //{
            //    ParameterBounded pb = this.Where(p => p.PrimaryParameter == ap)
            //    .Where(p => p.SecondaryParameterIndex == secondaryIndex)
            //    .FirstOrDefault();

            //    return (rnd.NextDouble() * (pb.UpperBounds - pb.LowerBounds) + pb.LowerBounds);
            //}
            //else { return 0; }            
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
                    bw.Write(pb.SecondaryParameterSubIndex);
                    bw.Write(pb.LowerBounds);
                    bw.Write(pb.UpperBounds);
                }
                bw.Close();
            }
        }
    }
}
