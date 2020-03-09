using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.Model.ParametersForGame
{
    public enum AllBoundedParameters
    {
        CityProdutionPerCityDensity, DevelopmentCostPerTerrainType, ExploreCostPerTerrainType, ProductionUnitsPerTerrainType,
        TransportationCostPerTerrainTypePerUnit, TerrainConstruct, PoolConstructParameters, 
        ActionCosts, NextParameterSet1, NextParameterSet2, NextParameterSet3, Total
    }
    public enum PoolConstructParametersSecondary
    {
        PoolLength, PoolWidth, PlStSq, AxisShift, Multiple, Decline, Total
    }
    public enum TerrainBoundedConstructSecondary
    {
        ForestWidthRatio, GrasslandWidthRatio, AxisShift, TerrainOffset, ForestAroundTributaryWidth, TributaryOffset, Total
    }
    public enum ActionCostsSecondary
    {
        AbandonCostPerUnit, OpexCostPerUnit, ProductionCostPerUnit, ReactivationCostPerUnit, SuspendCostPerUnit, Total
    }
    public enum NextParameterSet1SecondaryIndex { Next1, Total }
    public enum NextParameterSet2SecondaryIndex { Next1, Total }
    public enum NextParameterSet3SecondaryIndex { Next1, Total }
    public class ParameterBounded
    {
        public ParameterBounded(int primaryIndex, string secondarySubIndexName, int secondaryIndexNumber = (-1),double ub = 0, double lb = 0)
        {
            PrimaryIndexNumber = primaryIndex;
            PrimaryParameter = (AllBoundedParameters)primaryIndex;
            SecondaryParameterSubIndex = secondarySubIndexName;
            SecondaryParameterIndex = (secondaryIndexNumber == (-1)) ? GetSecondaryIndex() : secondaryIndexNumber;
            SetSecondaryIndex();
            UpperBounds = ub;
            LowerBounds = lb;
        }

        public double LowerBounds { get; set; }
        public double UpperBounds { get; set; }
        public int PrimaryIndexNumber { get; set; }
        public AllBoundedParameters PrimaryParameter { get; set; }
        public string SecondaryParameterType { get; set; }
        public int SecondaryParameterIndex { get; set; }
        public string SecondaryParameterSubIndex { get; set; }

        private void SetSecondaryIndex()
        {
            switch (PrimaryParameter)
            {
                case AllBoundedParameters.CityProdutionPerCityDensity:                    
                    SecondaryParameterType = nameof(CityDensity);
                    break;

                case AllBoundedParameters.DevelopmentCostPerTerrainType:
                case AllBoundedParameters.ExploreCostPerTerrainType:
                case AllBoundedParameters.ProductionUnitsPerTerrainType:
                case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                    SecondaryParameterType = nameof(TerrainTypeE);
                    break;

                case AllBoundedParameters.TerrainConstruct:
                    SecondaryParameterType = nameof(TerrainBoundedConstructSecondary);
                    break;

                case AllBoundedParameters.PoolConstructParameters:
                    SecondaryParameterType = nameof(PoolConstructParametersSecondary);
                    break;

                case AllBoundedParameters.ActionCosts:
                    SecondaryParameterType = nameof(ActionCostsSecondary);
                    break;

                case AllBoundedParameters.NextParameterSet1:
                    SecondaryParameterType = nameof(NextParameterSet1SecondaryIndex);
                    break;

                case AllBoundedParameters.NextParameterSet2:
                    SecondaryParameterType = nameof(NextParameterSet2SecondaryIndex);
                    break;

                case AllBoundedParameters.NextParameterSet3:
                    SecondaryParameterType = nameof(NextParameterSet3SecondaryIndex);
                    break;

                case AllBoundedParameters.Total:
                default:
                    break;
            }
        }
        public int GetSecondaryIndex()
        {
            switch (PrimaryParameter)
            {
                case AllBoundedParameters.CityProdutionPerCityDensity:
                    for (int i = 0; i < (int)CityDensity.Total; i++) 
                    { if (Convert.ToString((CityDensity)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.DevelopmentCostPerTerrainType:                    
                case AllBoundedParameters.ExploreCostPerTerrainType:
                case AllBoundedParameters.ProductionUnitsPerTerrainType:
                case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                    for (int i = 0; i < (int)TerrainTypeE.Total; i++)
                    { if (Convert.ToString((TerrainTypeE)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.TerrainConstruct:
                    for (int i = 0; i < (int)TerrainBoundedConstructSecondary.Total; i++)
                    { if (Convert.ToString((TerrainBoundedConstructSecondary)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.PoolConstructParameters:
                    for (int i = 0; i < (int)PoolConstructParametersSecondary.Total; i++)
                    { if (Convert.ToString((PoolConstructParametersSecondary)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.ActionCosts:
                    for (int i = 0; i < (int)ActionCostsSecondary.Total; i++)
                    { if (Convert.ToString((ActionCostsSecondary)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.NextParameterSet1:
                    for (int i = 0; i < (int)NextParameterSet1SecondaryIndex.Total; i++)
                    { if (Convert.ToString((NextParameterSet1SecondaryIndex)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.NextParameterSet2:
                    for (int i = 0; i < (int)NextParameterSet2SecondaryIndex.Total; i++)
                    { if (Convert.ToString((NextParameterSet2SecondaryIndex)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.NextParameterSet3:
                    for (int i = 0; i < (int)NextParameterSet3SecondaryIndex.Total; i++)
                    { if (Convert.ToString((NextParameterSet3SecondaryIndex)i) == SecondaryParameterSubIndex) { return i; } }
                    return 0;

                case AllBoundedParameters.Total:
                default:
                    break;
            }
            return 0;
        }
    }
}
