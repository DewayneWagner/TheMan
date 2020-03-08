using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using static TheManXS.Model.Parameter.ParameterConstant;

namespace TheManXS.Model.Parameter
{    public enum AllConstantParameters
    {
        CashConstant, CommodityConstants, PrimeRateAdderBasedOnCreditRating, PrimeRateAdderBasedOnTermLength,
        MapConstants, ResourceConstant, GameConstants, AssetValuationByStatusType, Total
    }
    public enum CashConstantSecondary
    {
        StartCash, StartDebt, TheManCut, SquarePrice, Total
    }
    public enum CommodityConstantSecondary
    {
        StartPrice, MaxPrice, MinPrice, MinChange, MaxChange, Total
    }
    public enum CreditRatings
    {
        AAA, AA, A, B, C, Junk, Total
    }
    public enum LoanTermLength
    {
        Five, Ten, Fifteen, Twenty, TwentyFive, Total
    }
    public enum MapConstantsSecondary
    {
        RowQ, ColQ, SqSize, StartRowRatioFromEdgeOfMap, Total
    }
    public enum ResourceConstantSecondary
    {
        DeclineTurnsFactor, ResSqRatio, MaxPoolSQ, Total
    }
    public enum GameConstantsSecondary
    {
        MaxSavedGameSlots, NumberOfResourceStartSQsPerPlayer, Total
    }

    public class ParameterConstant
    {
        
        public ParameterConstant() { }

        public ParameterConstant(int primaryIndex, int secondaryIndex)
        {
            PrimaryParameter = (AllConstantParameters)primaryIndex;
            SecondaryParameterIndex = secondaryIndex;
            SecondaryParameterTypeOf = GetSecondaryParameterTypeOf();
        }

        public double Constant { get; set; }
        public AllConstantParameters PrimaryParameter { get; set; }
        public string SecondaryParameterTypeOf { get; set; }
        public int SecondaryParameterIndex { get; set; }

        private string GetSecondaryParameterTypeOf()
        {
            switch (PrimaryParameter)
            {
                case Model.Parameter.AllConstantParameters.CashConstant:
                    return nameof(CashConstantSecondary);
                case Model.Parameter.AllConstantParameters.CommodityConstants:
                    return nameof(CommodityConstantSecondary);
                case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                    return nameof(CreditRatings);
                case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                    return nameof(LoanTermLength);
                case Model.Parameter.AllConstantParameters.MapConstants:
                    return nameof(MapConstantsSecondary);
                case Model.Parameter.AllConstantParameters.ResourceConstant:
                    return nameof(ResourceConstantSecondary);
                case Model.Parameter.AllConstantParameters.GameConstants:
                    return nameof(GameConstantsSecondary);
                case Model.Parameter.AllConstantParameters.AssetValuationByStatusType:
                    return nameof(StatusTypeE);
            }
            return null;
        }
    }
}
