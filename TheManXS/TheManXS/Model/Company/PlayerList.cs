using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Settings;
using Xamarin.Forms;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Company
{
    public class PlayerList
    {
        public PlayerList(bool isNewGame) { InitNewPlayersForGame(); }
        private void InitNewPlayersForGame()
        {
            int key = 0;
            bool playerExists;
            using (DBContext db = new DBContext())
            {
                // initialize actual player here
                CompanyNameGenerator companyNameGenerator = new CompanyNameGenerator();                
                GameSpecificParameters gsp = db.GameSpecificParameters.Find(QC.CurrentSavedGameSlot);
                key = GetPlayerKey(QC.PlayerIndexActual);
                playerExists = db.Player.Any(p => p.Key == key);

                CompanyColorsList companyColorsList = new CompanyColorsList();
                CompanyColors cc = new CompanyColors(gsp.Color);

                Player actualPlayer = new Player()
                {
                    Key = key,
                    Number = QC.PlayerIndexActual,
                    Cash = Settings.Setting.GetConstant(AS.CashConstant,
                            (int)Settings.SettingsMaster.CashConstantParameters.StartCash),
                    Debt = Settings.Setting.GetConstant(AS.CashConstant,
                            (int)Settings.SettingsMaster.CashConstantParameters.StartDebt),
                    Color = cc.ColorEnum,
                    IsComputer = false,
                    Name = gsp.CompanyName,
                    Ticker = gsp.Ticker,
                };

                if (playerExists) { db.Player.Update(actualPlayer); }
                else { db.Player.Add(actualPlayer); }

                companyColorsList.RemoveColorFromList(cc.ColorXamarin);

                for (int i = 1; i < QC.PlayerQ; i++)
                {
                    key = GetPlayerKey(i);
                    playerExists = db.Player.Any(pl => pl.Key == key);
                    CompanyColors companyColors = companyColorsList.GetRandomColor();

                    Player p = new Player()
                    {
                        Key = key,
                        Number = i,
                        Cash = Settings.Setting.GetConstant(AS.CashConstant,
                            (int)Settings.SettingsMaster.CashConstantParameters.StartCash),
                        Debt = Settings.Setting.GetConstant(AS.CashConstant,
                            (int)Settings.SettingsMaster.CashConstantParameters.StartDebt),
                        IsComputer = true,
                        Color = companyColors.ColorEnum,
                        Name = companyNameGenerator[i].Name,
                        Ticker = companyNameGenerator[i].Ticker,
                    };
                    
                    if (playerExists) { db.Player.Update(p); }
                    else { db.Player.Add(p); }
                }
                db.SaveChanges();
            }
        }
        public static int GetPlayerKey(int playerNum) => QC.CurrentSavedGameSlot * 10 + playerNum;
    }
}
