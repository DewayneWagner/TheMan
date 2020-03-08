using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.Model.Parameter
{
    public enum AllBoundedParameters
    {
        CityProdutionPerCityDensity, DevelopmentCostPerTerrainType, ExploreCostPerTerrainType,
        TransportationCostPerTerrainTypePerUnit, TerrainConstruct, PoolConstructParameters, Total
    }
    public enum PoolConstructParametersSecondary
    {
        PoolLength, PoolWidth, PlStSq, AxisShift, Multiple, Decline, Total
    }
    public enum TerrainConstructSecondary
    {
        ForestWidthRatio, GrasslandWidthRatio, AxisShift, TerrainOffset, Total
    }
    public enum ActionCostsSecondary
    {
        AbandonCostPerUnit, OpexCostPerUnit, ProductionCostPerUnit, ReactivationCostPerUnit, SuspendCostPerUnit, Total
    }
    public class ParameterBounded
    {
        public ParameterBounded() { }

        public ParameterBounded(int primaryIndex, int secondaryIndex)
        {
            PrimaryParameter = (AllBoundedParameters)primaryIndex;
            SecondaryParameterIndex = secondaryIndex;
        }

        public double LowerBounds { get; set; }
        public double UpperBounds { get; set; }
        public AllBoundedParameters PrimaryParameter { get; set; }
        public string SecondaryParameterType { get; set; }
        public int SecondaryParameterIndex { get; set; }

    }
}
