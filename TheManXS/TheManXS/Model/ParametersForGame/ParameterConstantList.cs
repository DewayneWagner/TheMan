using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TheManXS.Model.ParametersForGame
{
    public class ParameterConstantList : List<ParameterConstant>
    {
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
                for (int secondary = 0; secondary < getSecondaryIndexTotal(primary); secondary++)
                {
                    this.Add(new ParameterConstant(primary, secondary));
                }
            }
            int getSecondaryIndexTotal(int primaryIndex)
            {
                AllConstantParameters ap = (AllConstantParameters)primaryIndex;

                switch (ap)
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
                }
                return 0;
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
                    else { this.Add(new ParameterConstant(primaryIndex, secondaryIndex)); }
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
