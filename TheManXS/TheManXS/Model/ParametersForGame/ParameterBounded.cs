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
        ActionCosts, Total
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
    public class ParameterBounded
    {
        public ParameterBounded(int primaryIndex, int secondaryIndex, double ub, double lb)
        {
            PrimaryIndexNumber = primaryIndex;
            PrimaryParameter = (AllBoundedParameters)primaryIndex;
            SecondaryParameterIndex = secondaryIndex;
            SetSecondaryIndex();
            UpperBounds = ub;
            LowerBounds = lb;
        }

        public ParameterBounded(int primaryIndex, int secondaryIndex)
        {
            PrimaryIndexNumber = primaryIndex;
            PrimaryParameter = (AllBoundedParameters)primaryIndex;
            SecondaryParameterIndex = secondaryIndex;
            SetSecondaryIndex();
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
                    SecondaryParameterSubIndex = Convert.ToString((CityDensity)SecondaryParameterIndex);
                    break;

                case AllBoundedParameters.DevelopmentCostPerTerrainType:
                case AllBoundedParameters.ExploreCostPerTerrainType:
                case AllBoundedParameters.ProductionUnitsPerTerrainType:
                case AllBoundedParameters.TransportationCostPerTerrainTypePerUnit:
                    SecondaryParameterType = nameof(TerrainTypeE);
                    SecondaryParameterSubIndex = Convert.ToString((TerrainTypeE)SecondaryParameterIndex);
                    break;

                case AllBoundedParameters.TerrainConstruct:
                    SecondaryParameterType = nameof(TerrainBoundedConstructSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((TerrainBoundedConstructSecondary)SecondaryParameterIndex);
                    break;

                case AllBoundedParameters.PoolConstructParameters:
                    SecondaryParameterType = nameof(PoolConstructParametersSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((PoolConstructParametersSecondary)SecondaryParameterIndex);
                    break;

                case AllBoundedParameters.ActionCosts:
                    SecondaryParameterType = nameof(ActionCostsSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((ActionCostsSecondary)SecondaryParameterIndex);
                    break;

                case AllBoundedParameters.Total:
                default:
                    break;
            }
        }
    }
}
