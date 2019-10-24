using System;
using System.Collections.Generic;
using System.Text;
using static TheManXS.Model.Settings.SettingsMaster;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;

namespace TheManXS.Model.Settings
{
    public class SecondaryIndex
    {
        private static string[] _secondaryIndexType;
        private static bool[] _secondaryIndexIsBounded;

        // to be initialized at beginning...to load static arrays
        public SecondaryIndex()
        {
            _secondaryIndexType = InitSecondaryIndexTypeArray();
            _secondaryIndexIsBounded = InitSecondaryIsBoundedArray();
        }
        private bool GetIsBounded(AS a) => _secondaryIndexIsBounded[(int)a];
        public static string GetSecondaryIndexType(AS a) => _secondaryIndexType[(int)a];

        public SecondaryIndex(AS a, int index)
        {
            IsBounded = GetIsBounded(a);
            Type = GetSecondaryIndexType(a);
            Name = Convert.ToString(a);
            SubIndexName = SubIndex(Type, index);
        }

        public bool IsBounded { get; }
        public string Type { get; }
        public string Name { get; }
        public string SubIndexName { get; }

        private bool[] InitSecondaryIsBoundedArray()
        {
            bool[] b = new bool[(int)AS.Total];

            b[(int)AS.AbandonTT] = true;
            b[(int)AS.AssValST] = true;
            b[(int)AS.CashConstant] = false;
            b[(int)AS.CityProdCD] = false;
            b[(int)AS.DevTT] = true;
            b[(int)AS.ExpTT] = true;
            b[(int)AS.IntRateCR] = false;
            b[(int)AS.MapConstants] = false;
            b[(int)AS.OPEXTT] = true;
            b[(int)AS.PlayerConstants] = false;
            b[(int)AS.PoolParams] = true;
            b[(int)AS.ProductionTT] = true;
            b[(int)AS.ReactivateSingleP] = true;
            b[(int)AS.ResConstant] = false;
            b[(int)AS.SusTT] = true;
            b[(int)AS.TerrainBoundedTCB] = true;
            b[(int)AS.TransTT] = true;
            b[(int)AS.MiscellaneousConstants] = false;

            return b;
        }

        private string[] InitSecondaryIndexTypeArray()
        {
            string[] s = new string[(int)AS.Total];

            s[(int)AS.AbandonTT] = nameof(TerrainTypeE);
            s[(int)AS.AssValST] = nameof(StatusTypeE);
            s[(int)AS.CashConstant] = nameof(CashConstantParameters);
            s[(int)AS.CityProdCD] = nameof(CityDensity);
            s[(int)AS.DevTT] = nameof(TerrainTypeE);
            s[(int)AS.ExpTT] = nameof(TerrainTypeE);
            s[(int)AS.IntRateCR] = nameof(CreditRatingsE);
            s[(int)AS.MapConstants] = nameof(TerrainConstructConstants);
            s[(int)AS.OPEXTT] = nameof(TerrainTypeE);
            s[(int)AS.PlayerConstants] = nameof(PlayerConstants);
            s[(int)AS.PoolParams] = nameof(PoolParams);
            s[(int)AS.ProductionTT] = nameof(TerrainTypeE);
            s[(int)AS.ReactivateSingleP] = nameof(TerrainTypeE);
            s[(int)AS.ResConstant] = nameof(ResConstantParams);
            s[(int)AS.SusTT] = nameof(TerrainTypeE);
            s[(int)AS.TerrainBoundedTCB] = nameof(TerrainConstructBounded);
            s[(int)AS.TransTT] = nameof(TerrainTypeE);
            s[(int)AS.MiscellaneousConstants] = nameof(MiscellaneousStuff);

            return s;
        }

        private string SubIndex(string indexType, int index)
        {
            switch (indexType)
            {
                case nameof(TerrainTypeE):
                    return Convert.ToString((TerrainTypeE)index);
                case nameof(StatusTypeE):
                    return Convert.ToString((StatusTypeE)index);
                case nameof(CashConstantParameters):
                    return Convert.ToString((CashConstantParameters)index);
                case nameof(CityDensity):
                    return Convert.ToString((CityDensity)index);
                case nameof(CreditRatingsE):
                    return Convert.ToString((CreditRatingsE)index);
                case nameof(TerrainConstructBounded):
                    return Convert.ToString((TerrainConstructBounded)index);
                case nameof(TerrainConstructConstants):
                    return Convert.ToString((TerrainConstructConstants)index);
                case nameof(PlayerConstants):
                    return Convert.ToString((PlayerConstants)index);
                case nameof(PoolParams):
                    return Convert.ToString((PoolParams)index);
                case nameof(ResConstantParams):
                    return Convert.ToString((ResConstantParams)index);
                case nameof(MiscellaneousStuff):
                    return Convert.ToString((MiscellaneousStuff)index);
                default:
                    return "???";
            }
        }
    }
}
