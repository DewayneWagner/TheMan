using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TheManXS.Model.Parameter
{
    public class ParameterBoundedBinaryReader
    {
        public ParameterBoundedBinaryReader()
        {

        }

       


        public static void UpdateBinaryWithSettingsFromDB()
        {




            SettingsMaster sm = new SettingsMaster(true);
            List<Setting> settingsList = sm.GetListOfSettingsInDB();

            string parameterFile = App.BinaryBackupPath;

            File.Delete(parameterFile);

            using (BinaryWriter bw = new BinaryWriter(File.Open(parameterFile, FileMode.OpenOrCreate)))
            {
                foreach (Setting s in settingsList)
                {
                    bw.Write(s.IsBounded);
                    bw.Write(s.Key);
                    bw.Write(s.LBOrConstant);
                    bw.Write((int)(s.PrimaryIndex));
                    bw.Write(s.SecondaryIndexTypeName);
                    bw.Write(s.SecondarySubIndex);
                    bw.Write(s.UB);
                }
                bw.Close();
            }
        }
        public static List<Setting> GetSettingsFromBinary()
        {
            List<Setting> settingsFromBinary = new List<Setting>();

            using (BinaryReader br = new BinaryReader(File.Open(App.BinaryBackupPath, FileMode.OpenOrCreate)))
            {
                while (br.PeekChar() != (-1))
                {
                    Setting s = new Setting();

                    s.IsBounded = br.ReadBoolean();
                    s.Key = br.ReadInt32();
                    s.LBOrConstant = br.ReadDouble();
                    s.PrimaryIndex = (AS)br.ReadInt32();
                    s.SecondaryIndexTypeName = br.ReadString();
                    s.SecondarySubIndex = br.ReadString();
                    s.UB = br.ReadDouble();

                    settingsFromBinary.Add(s);
                }
            }
            return settingsFromBinary;
        }
    }
}
