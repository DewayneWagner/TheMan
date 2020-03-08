using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TheManXS.Model.Parameter
{
    public class ParameterBoundedList : List<ParameterBounded>
    {
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
                    this.Add(getEmptyParameter(primary, secondary));
                }
            }
            ParameterBounded getEmptyParameter(int primaryIndex, int secondaryIndex)
            {
                ParameterBounded p = new ParameterBounded();
                p.PrimaryParameter = (AllBoundedParameters)primaryIndex;

                switch (p.PrimaryParameter)
                {
                    case AllBoundedParameters.CityProdutionPerCityDensity:
                        p.SecondaryParameterIndex = secondaryIndex;
                        p.SecondaryParameterType = nameof(CityDensity);
                        break;

                    case AllBoundedParameters.DevelopmentCostPerTerrainType:
                    case AllBoundedParameters.ExploreCostPerTerrainType:
                    case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                        p.SecondaryParameterIndex = secondaryIndex;
                        p.SecondaryParameterType = nameof(TerrainTypeE);
                        break;

                    case AllBoundedParameters.TerrainConstruct:
                        p.SecondaryParameterIndex = secondaryIndex;
                        p.SecondaryParameterType = nameof(TerrainConstructSecondary);
                        break;

                    case AllBoundedParameters.PoolConstructParameters:
                        p.SecondaryParameterIndex = secondaryIndex;
                        p.SecondaryParameterType = nameof(PoolConstructParametersSecondary);
                        break;

                    case AllBoundedParameters.Total:
                    default:
                        break;
                }
                return p;
            }
            int getTotalOfElementsInSecondaryIndex(int primaryIndex)
            {
                AllBoundedParameters a = (AllBoundedParameters)primaryIndex;

                switch (a)
                {
                    case Model.Parameter.AllBoundedParameters.CityProdutionPerCityDensity:
                        return (int)CityDensity.Total;

                    case Model.Parameter.AllBoundedParameters.DevelopmentCostPerTerrainType:
                    case Model.Parameter.AllBoundedParameters.ExploreCostPerTerrainType:
                    case Model.Parameter.AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                        return (int)TerrainTypeE.Total;

                    case Model.Parameter.AllBoundedParameters.TerrainConstruct:
                        return (int)TerrainConstructSecondary.Total;

                    case Model.Parameter.AllBoundedParameters.PoolConstructParameters:
                        return (int)PoolConstructParametersSecondary.Total;

                    case Model.Parameter.AllBoundedParameters.Total:
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
                    AllBoundedParameters primary = (AllBoundedParameters)primaryIndex;
                    int secondaryIndex = br.ReadInt32();

                    bool paramExists = this.Exists(p => p.PrimaryParameter == primary && p.SecondaryParameterIndex == secondaryIndex);

                    if (paramExists)
                    {
                        ParameterBounded pb = this.Where(p => p.PrimaryParameter == primary)
                            .Where(p => p.SecondaryParameterIndex == secondaryIndex)
                            .FirstOrDefault();

                        pb.LowerBounds = br.ReadDouble();
                        pb.UpperBounds = br.ReadDouble();
                    }
                    else { this.Add(new ParameterBounded(primaryIndex, secondaryIndex)); }                    
                }
            }
        }
        public void WriteDataToBinaryFile()
        {
            string boundedParameterFile = App.ParameterBoundedPath;
            File.Delete(boundedParameterFile);

            using (BinaryWriter bw = new BinaryWriter(File.Open(boundedParameterFile,FileMode.OpenOrCreate)))
            {
                foreach (ParameterBounded pb in this)
                {
                    bw.Write((int)pb.PrimaryParameter);
                    bw.Write(pb.SecondaryParameterIndex);
                    bw.Write(pb.LowerBounds);
                    bw.Write(pb.UpperBounds);
                }
                bw.Close();
            }
        }
    }
}
