using System;

namespace TheManXS.Model.ParametersForGame
{
    public enum AllConstantParameters
    {
        CashConstant, CommodityConstants, PrimeRateAdderBasedOnCreditRating, PrimeRateAdderBasedOnTermLength,
        MapConstants, ResourceConstant, GameConstants, AssetValuationByStatusType,
        InfrastructureConstructionRatiosTT, ConstructionConstants, NextParameterSet2, NextParameterSet3, Total
    }
    public enum CashConstantSecondary { StartCash, StartDebt, TheManCut, SquarePrice, StartPrimeRate, Total }
    public enum CommodityConstantSecondary { StartPrice, MaxPrice, MinPrice, MinChange, MaxChange, Total }
    public enum CreditRatings { AAA, AA, A, B, C, Junk, Total }
    public enum LoanTermLength { Five, Ten, Fifteen, Twenty, TwentyFive, Total }
    public enum MapConstantsSecondary
    {
        RowQ, ColQ, SqSize, StartRowRatioFromEdgeOfMap, NumberOfTreesPerSideOfSQ,
        TreeVerticalOverlapRatio, Total
    }
    public enum ResourceConstantSecondary { DeclineTurnsFactor, ResSqRatio, MaxPoolSQ, Total }
    public enum GameConstantsSecondary { MaxSavedGameSlots, NumberOfResourceStartSQsPerPlayer, PlayerQ, Total }
    public enum ConstructionConstantsSecondary { TurnsToBuildOnGrassLand, TurnsToBuildOnForestTile, TurnsToBuildOnMountainTile, Total }
    public enum NextConstantParameterSet2SecondaryIndex { Next1, Total }
    public enum NextConstantParameterSet3SecondaryIndex { Next1, Total }

    public class ParameterConstant
    {
        public ParameterConstant(int primaryIndex, string secondarySubIndex, int secondaryIndexNumber = (-1), double constant = 0)
        {
            PrimaryIndexNumber = primaryIndex;
            PrimaryParameter = (AllConstantParameters)primaryIndex;
            SecondarySubIndex = secondarySubIndex;
            SecondaryParameterIndex = (secondaryIndexNumber == (-1)) ? GetSecondarySubIndex() : secondaryIndexNumber;
            SecondaryParameterTypeOf = GetSecondaryParameterTypeOf();
            Constant = constant;
        }

        public double Constant { get; set; }
        public AllConstantParameters PrimaryParameter { get; set; }
        public string SecondaryParameterTypeOf { get; set; }
        public int SecondaryParameterIndex { get; set; }
        public string SecondarySubIndex { get; set; }
        public int PrimaryIndexNumber { get; set; }

        public int GetSecondarySubIndex()
        {
            switch (PrimaryParameter)
            {
                case AllConstantParameters.CashConstant:
                    for (int i = 0; i < (int)CashConstantSecondary.Total; i++)
                    { if (Convert.ToString((CashConstantSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.CommodityConstants:
                    for (int i = 0; i < (int)CommodityConstantSecondary.Total; i++)
                    { if (Convert.ToString((CommodityConstantSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                    for (int i = 0; i < (int)CreditRatings.Total; i++)
                    { if (Convert.ToString((CreditRatings)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                    for (int i = 0; i < (int)LoanTermLength.Total; i++)
                    { if (Convert.ToString((LoanTermLength)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.MapConstants:
                    for (int i = 0; i < (int)MapConstantsSecondary.Total; i++)
                    { if (Convert.ToString((MapConstantsSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.ResourceConstant:
                    for (int i = 0; i < (int)ResourceConstantSecondary.Total; i++)
                    { if (Convert.ToString((ResourceConstantSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.GameConstants:
                    for (int i = 0; i < (int)GameConstantsSecondary.Total; i++)
                    { if (Convert.ToString((GameConstantsSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.AssetValuationByStatusType:
                    for (int i = 0; i < (int)StatusTypeE.Total; i++)
                    { if (Convert.ToString((StatusTypeE)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.InfrastructureConstructionRatiosTT:
                    for (int i = 0; i < (int)TerrainTypeE.Total; i++)
                    { if (Convert.ToString((TerrainTypeE)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.ConstructionConstants:
                    for (int i = 0; i < (int)ConstructionConstantsSecondary.Total; i++)
                    { if (Convert.ToString((ConstructionConstantsSecondary)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.NextParameterSet2:
                    for (int i = 0; i < (int)NextConstantParameterSet2SecondaryIndex.Total; i++)
                    { if (Convert.ToString((NextConstantParameterSet2SecondaryIndex)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.NextParameterSet3:
                    for (int i = 0; i < (int)NextConstantParameterSet3SecondaryIndex.Total; i++)
                    { if (Convert.ToString((NextConstantParameterSet3SecondaryIndex)i) == SecondarySubIndex) { return i; } }
                    return 0;
                case AllConstantParameters.Total:
                default:
                    break;
            }
            return 0;
        }

        private string GetSecondaryParameterTypeOf()
        {
            switch (PrimaryParameter)
            {
                case AllConstantParameters.CashConstant:
                    return nameof(CashConstantSecondary);
                case AllConstantParameters.CommodityConstants:
                    return nameof(CommodityConstantSecondary);
                case AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                    return nameof(CreditRatings);
                case AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                    return nameof(LoanTermLength);
                case AllConstantParameters.MapConstants:
                    return nameof(MapConstantsSecondary);
                case AllConstantParameters.ResourceConstant:
                    return nameof(ResourceConstantSecondary);
                case AllConstantParameters.GameConstants:
                    return nameof(GameConstantsSecondary);
                case AllConstantParameters.AssetValuationByStatusType:
                    return nameof(StatusTypeE);
                case AllConstantParameters.InfrastructureConstructionRatiosTT:
                    return nameof(TerrainTypeE);
                case AllConstantParameters.ConstructionConstants:
                    return nameof(ConstructionConstantsSecondary);
                case AllConstantParameters.NextParameterSet2:
                    return nameof(NextConstantParameterSet2SecondaryIndex);
                case AllConstantParameters.NextParameterSet3:
                    return nameof(NextConstantParameterSet3SecondaryIndex);
                case AllConstantParameters.Total:
                default:
                    break;
            }
            return null;
        }
    }
}
