using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.ParametersForGame
{    public enum AllConstantParameters
    {
        CashConstant, CommodityConstants, PrimeRateAdderBasedOnCreditRating, PrimeRateAdderBasedOnTermLength,
        MapConstants, ResourceConstant, GameConstants, AssetValuationByStatusType, 
        InfrastructureConstructionRatiosTT, Total
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
        MaxSavedGameSlots, NumberOfResourceStartSQsPerPlayer, PlayerQ, Total
    }

    public class ParameterConstant
    {
        
        public ParameterConstant(int primaryIndex, int secondaryIndex, double constant = 0)
        {
            PrimaryParameter = (AllConstantParameters)primaryIndex;
            SecondaryParameterIndex = secondaryIndex;
            SetSecondaryParameterTypeOf();
            Constant = constant;
        }

        public double Constant { get; set; }
        public AllConstantParameters PrimaryParameter { get; set; }
        public string SecondaryParameterTypeOf { get; set; }
        public int SecondaryParameterIndex { get; set; }
        public string SecondaryParameterSubIndex { get; set; }

        private void SetSecondaryParameterTypeOf()
        {
            switch (PrimaryParameter)
            {
                case AllConstantParameters.CashConstant:
                    SecondaryParameterTypeOf = nameof(CashConstantSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((CashConstantSecondary)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.CommodityConstants:
                    SecondaryParameterTypeOf = nameof(CommodityConstantSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((CommodityConstantSecondary)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                    SecondaryParameterTypeOf = nameof(CreditRatings);
                    SecondaryParameterSubIndex = Convert.ToString((CreditRatings)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                    SecondaryParameterTypeOf = nameof(LoanTermLength);
                    SecondaryParameterSubIndex = Convert.ToString((LoanTermLength)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.MapConstants:
                    SecondaryParameterTypeOf = nameof(MapConstantsSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((MapConstantsSecondary)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.ResourceConstant:
                    SecondaryParameterTypeOf = nameof(ResourceConstantSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((ResourceConstantSecondary)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.GameConstants:
                    SecondaryParameterTypeOf = nameof(GameConstantsSecondary);
                    SecondaryParameterSubIndex = Convert.ToString((GameConstantsSecondary)SecondaryParameterIndex);
                    break;
                case AllConstantParameters.AssetValuationByStatusType:
                    SecondaryParameterTypeOf = nameof(StatusTypeE);
                    SecondaryParameterSubIndex = Convert.ToString((StatusTypeE)SecondaryParameterIndex);
                    break;
            }
        }
    }
}
