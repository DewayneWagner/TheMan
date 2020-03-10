using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TheManXS.Model.ParametersForGame
{
    public class ParameterConstantList : List<ParameterConstant>
    {
        public ParameterConstantList(bool isForSettingsVM) { }
        private int _elementCount = 0;

        public ParameterConstantList()
        {
            InitListOfEmptyParameters();
            ReadDataFromBinaryFile();
        }

        public double GetConstant(AllConstantParameters acp, int secondaryIndex)
        {
            return this.Where(p => p.PrimaryParameter == acp)
                .Where(p => p.SecondaryParameterIndex == secondaryIndex)
                .Select(p => p.Constant)
                .FirstOrDefault();
        }

        private void InitListOfEmptyParameters()
        {
            for (int primary = 0; primary < (int)AllConstantParameters.Total; primary++)
            {
                int countOfEnum = getSecondaryIndexTotal(primary);
                for (int secondary = 0; secondary < countOfEnum; secondary++)
                {
                    string secondarySubIndex = GetSecondarySubIndex(primary, secondary);
                    this.Add(new ParameterConstant(primary, secondarySubIndex, secondary));
                    _elementCount++;
                }
            }
            int getSecondaryIndexTotal(int primaryIndex)
            {
                switch ((AllConstantParameters)primaryIndex)
                {
                    case AllConstantParameters.CashConstant:
                        return (int)CashConstantSecondary.Total;
                    case AllConstantParameters.CommodityConstants:
                        return (int)CommodityConstantSecondary.Total;
                    case AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                        return (int)CreditRatings.Total;
                    case AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                        return (int)LoanTermLength.Total;
                    case AllConstantParameters.MapConstants:
                        return (int)MapConstantsSecondary.Total;
                    case AllConstantParameters.ResourceConstant:
                        return (int)ResourceConstantSecondary.Total;
                    case AllConstantParameters.GameConstants:
                        return (int)GameConstantsSecondary.Total;
                    case AllConstantParameters.AssetValuationByStatusType:
                        return (int)StatusTypeE.Total;
                    case AllConstantParameters.InfrastructureConstructionRatiosTT:
                        return (int)TerrainTypeE.Total;
                    case AllConstantParameters.NextParameterSet1:
                        return (int)NextConstantParameterSet1SecondaryIndex.Total;
                    case AllConstantParameters.NextParameterSet2:
                        return (int)NextConstantParameterSet2SecondaryIndex.Total;
                    case AllConstantParameters.NextParameterSet3:
                        return (int)NextConstantParameterSet3SecondaryIndex.Total;
                    case AllConstantParameters.Total:
                    default:
                        break;
                }
                return 0;
            }
        }
        private string GetSecondarySubIndex(int primaryIndex, int secondaryIndex)
        {
            switch ((AllConstantParameters)primaryIndex)
            {
                case AllConstantParameters.CashConstant:
                    return Convert.ToString((CashConstantSecondary)secondaryIndex);
                case AllConstantParameters.CommodityConstants:
                    return Convert.ToString((CommodityConstantSecondary)secondaryIndex);
                case AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                    return Convert.ToString((CreditRatings)secondaryIndex);
                case AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                    return Convert.ToString((LoanTermLength)secondaryIndex);
                case AllConstantParameters.MapConstants:
                    return Convert.ToString((MapConstantsSecondary)secondaryIndex);
                case AllConstantParameters.ResourceConstant:
                    return Convert.ToString((ResourceConstantSecondary)secondaryIndex);
                case AllConstantParameters.GameConstants:
                    return Convert.ToString((GameConstantsSecondary)secondaryIndex);
                case AllConstantParameters.AssetValuationByStatusType:
                    return Convert.ToString((StatusTypeE)secondaryIndex);
                case AllConstantParameters.InfrastructureConstructionRatiosTT:
                    return Convert.ToString((TerrainTypeE)secondaryIndex);
                case AllConstantParameters.NextParameterSet1:
                    return Convert.ToString((NextConstantParameterSet1SecondaryIndex)secondaryIndex);
                case AllConstantParameters.NextParameterSet2:
                    return Convert.ToString((NextConstantParameterSet2SecondaryIndex)secondaryIndex);
                case AllConstantParameters.NextParameterSet3:
                    return Convert.ToString((NextConstantParameterSet3SecondaryIndex)secondaryIndex);
                case AllConstantParameters.Total:
                default:
                    break;
            }
            return null;
        }
        private void ReadDataFromBinaryFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(App.ParameterConstantPath, FileMode.OpenOrCreate)))
            {
                for (int i = 0; i < _elementCount; i++)
                {
                    while (br.PeekChar() != (-1))
                    {
                        int primaryIndex = br.ReadInt32();
                        string secondaryIndex = br.ReadString();

                        bool paramExistsInThis = this.Exists(p => p.PrimaryIndexNumber == primaryIndex && p.SecondarySubIndex == secondaryIndex);
                        if (paramExistsInThis)
                        {
                            ParameterConstant pc = this.Where(p => p.PrimaryIndexNumber == primaryIndex)
                                .Where(p => p.SecondarySubIndex == secondaryIndex)
                                .FirstOrDefault();

                            if (br.PeekChar() != (-1)) { pc.Constant = br.ReadDouble(); }
                            else { pc.Constant = 0; }
                        }
                    }
                }                
            }
        }
        public void WriteDataToBinaryFile()
        {
            string constantParameterFile = App.ParameterConstantPath;
            File.Delete(constantParameterFile);

            using (BinaryWriter bw = new BinaryWriter(File.Open(constantParameterFile,FileMode.OpenOrCreate)))
            {
                foreach (ParameterConstant pc in this)
                {
                    bw.Write(pc.PrimaryIndexNumber);
                    bw.Write(pc.SecondarySubIndex);
                    bw.Write(pc.Constant);
                }
            }
        }
    }
}
