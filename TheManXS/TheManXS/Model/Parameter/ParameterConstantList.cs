using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static TheManXS.Model.Parameter.ParameterConstant;

namespace TheManXS.Model.Parameter
{
    public class ParameterConstantList : List<ParameterConstant>
    {
        public ParameterConstantList()
        {
            InitListOfEmptyParameters();
            ReadDataFromBinaryFile();
        }
        private void InitListOfEmptyParameters()
        {
            for (int primary = 0; primary < (int)AllConstantParameters.Total; primary++)
            {
                for (int secondary = 0; secondary < getSecondaryIndexTotal(primary); secondary++)
                {
                    this.Add(GetEmptyParameterConstant(primary, secondary));
                }
            }
            int getSecondaryIndexTotal(int primaryIndex)
            {
                AllConstantParameters ap = (AllConstantParameters)primaryIndex;

                switch (ap)
                {
                    case Model.Parameter.AllConstantParameters.CashConstant:
                        return (int)CashConstantSecondary.Total;

                    case Model.Parameter.AllConstantParameters.CommodityConstants:
                        return (int)CommodityConstantSecondary.Total;

                    case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                        return (int)CreditRatings.Total;

                    case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                        return (int)LoanTermLength.Total;

                    case Model.Parameter.AllConstantParameters.MapConstants:
                        return (int)MapConstantsSecondary.Total;

                    case Model.Parameter.AllConstantParameters.ResourceConstant:
                        return (int)ResourceConstantSecondary.Total;

                    case Model.Parameter.AllConstantParameters.GameConstants:
                        return (int)GameConstantsSecondary.Total;

                    case Model.Parameter.AllConstantParameters.AssetValuationByStatusType:
                        return (int)StatusTypeE.Total;
                }
                return 0;
            }
            ParameterConstant GetEmptyParameterConstant(int primaryIndex, int secondaryIndex)
            {
                ParameterConstant pc = new ParameterConstant();
                pc.PrimaryParameter = (AllConstantParameters)primaryIndex;
                pc.SecondaryParameterIndex = secondaryIndex;

                switch (pc.PrimaryParameter)
                {
                    case Model.Parameter.AllConstantParameters.CashConstant:
                        pc.SecondaryParameterTypeOf = nameof(CashConstantSecondary);
                        break;
                    case Model.Parameter.AllConstantParameters.CommodityConstants:
                        pc.SecondaryParameterTypeOf = nameof(CommodityConstantSecondary);
                        break;
                    case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnCreditRating:
                        pc.SecondaryParameterTypeOf = nameof(CreditRatings);
                        break;
                    case Model.Parameter.AllConstantParameters.PrimeRateAdderBasedOnTermLength:
                        pc.SecondaryParameterTypeOf = nameof(LoanTermLength);
                        break;
                    case Model.Parameter.AllConstantParameters.MapConstants:
                        pc.SecondaryParameterTypeOf = nameof(MapConstantsSecondary);
                        break;
                    case Model.Parameter.AllConstantParameters.ResourceConstant:
                        pc.SecondaryParameterTypeOf = nameof(ResourceConstantSecondary);
                        break;
                    case Model.Parameter.AllConstantParameters.GameConstants:
                        pc.SecondaryParameterTypeOf = nameof(GameConstantsSecondary);
                        break;
                    case Model.Parameter.AllConstantParameters.AssetValuationByStatusType:
                        pc.SecondaryParameterTypeOf = nameof(StatusTypeE);
                        break;
                }
                return pc;
            }
        }
        private void ReadDataFromBinaryFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(App.ParameterConstantPath, FileMode.OpenOrCreate)))
            {
                while (br.PeekChar() != (-1))
                {
                    int primaryIndex = br.ReadInt32();
                    AllConstantParameters primary = (AllConstantParameters)primaryIndex;
                    int secondaryIndex = br.ReadInt32();

                    bool paramExistsInThis = this.Exists(p => p.PrimaryParameter == (AllConstantParameters)primaryIndex && p.SecondaryParameterIndex == secondaryIndex);
                    if (paramExistsInThis)
                    {
                        ParameterConstant pc = this.Where(p => p.PrimaryParameter == (AllConstantParameters)primaryIndex)
                            .Where(p => p.SecondaryParameterIndex == secondaryIndex)
                            .FirstOrDefault();
                        pc.Constant = br.ReadDouble();
                    }
                    else { this.Add(new ParameterConstant())}
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
                    bw.Write((int)pc.PrimaryParameter);
                    bw.Write(pc.SecondaryParameterIndex);
                    bw.Write(pc.Constant);
                }
            }
        }
    }
}
