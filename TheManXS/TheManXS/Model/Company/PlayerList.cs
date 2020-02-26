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
    public class PlayerList : List<Player>
    {
        GameSpecificParameters _gsp;

        public PlayerList(GameSpecificParameters gsp)
        {
            _gsp = gsp;
            InitPlayerList();
            WriteToDB();
        }

        private void InitPlayerList()
        {
            // init variables
            CompanyNameGenerator companyNameGenerator = new CompanyNameGenerator();
            CompanyColorsList companyColorsList = new CompanyColorsList();
            CompanyColors cc = new CompanyColors(_gsp.Color);
            double cash = Settings.Setting.GetConstant(AS.CashConstant, (int)Settings.SettingsMaster.CashConstantParameters.StartCash);
            double debt = Settings.Setting.GetConstant(AS.CashConstant, (int)Settings.SettingsMaster.CashConstantParameters.StartDebt);

            // init playerList with variables that change for each player
            for (int i = 0; i < QC.PlayerQ; i++)
            {
                if(i == 0) { Add(getActualPlayer()); }
                else { Add(getPlayer(i)); }
            }

            Player getActualPlayer()
            {
                return new Player()
                {
                    Key = GetPlayerKey(QC.PlayerIndexActual),
                    SavedGameSlot = QC.CurrentSavedGameSlot,
                    Color = _gsp.Color,
                    IsComputer = false,
                    Name = _gsp.CompanyName,
                    Number = QC.PlayerIndexActual,
                    Ticker = _gsp.Ticker,
                    Cash = cash,
                    Debt = debt,
                };
            }
            Player getPlayer(int i)
            {
                return new Player()
                {
                    Key = GetPlayerKey(i),
                    SavedGameSlot = QC.CurrentSavedGameSlot,
                    Color = companyColorsList.GetRandomColor().ColorEnum,
                    IsComputer = true,
                    Name = companyNameGenerator[i].Name,
                    Ticker = companyNameGenerator[i].Ticker,
                    Number = i,
                    Cash = cash,
                    Debt = debt,
                };
            }
        }

        public static int GetPlayerKey(int playerNum) => QC.CurrentSavedGameSlot * 10 + playerNum;

        private void WriteToDB()
        {
            using (DBContext db = new DBContext())
            {
                var listOfExistingPlayersInDB = db.Player.Where(p => p.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                db.RemoveRange(listOfExistingPlayersInDB);
                db.Player.AddRange(this);
                db.SaveChanges();
            }
        }
    }
}
