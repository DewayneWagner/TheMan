using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Settings;
using Xamarin.Forms;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Company
{
    public class PlayerList : List<Player>
    {
        GameSpecificParameters _gsp;
        double _startCash, _startDebt;
        Game _game;

        public PlayerList(GameSpecificParameters gsp, Game game)
        {
            _gsp = gsp;
            _game = game;
            SetStartCashAndStartDebt();
            InitPlayerList();
            WriteToDB();
        }

        public PlayerList(bool isForLoadedGame)
        {
            using (DBContext db = new DBContext())
            {
                var plist = db.Player.Where(p => p.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                foreach (Player player in plist) { this.Add(player); }
            }
        }

        private void InitPlayerList()
        {
            // init variables
            CompanyNameGenerator companyNameGenerator = new CompanyNameGenerator();
            //double cash = Settings.Setting.GetConstant(AS.CashConstant, (int)Settings.SettingsMaster.CashConstantParameters.StartCash);
            //double debt = Settings.Setting.GetConstant(AS.CashConstant, (int)Settings.SettingsMaster.CashConstantParameters.StartDebt);

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
                    SKColor = _gsp.CompanyColor,
                    ColorString = Convert.ToString(_gsp.CompanyColor),
                    IsComputer = false,
                    Name = _gsp.CompanyName,
                    Number = QC.PlayerIndexActual,
                    Ticker = _gsp.Ticker,
                    Cash = _startCash,
                    Debt = _startDebt,
                };
            }
            Player getPlayer(int i)
            {
                SKColor cc = _gsp.CompanyColorGenerator.GetRandomSKColor();
                return new Player()
                {
                    Key = GetPlayerKey(i),
                    SavedGameSlot = QC.CurrentSavedGameSlot,
                    SKColor = cc,
                    ColorString = Convert.ToString(cc),
                    IsComputer = true,
                    Name = companyNameGenerator[i].Name,
                    Ticker = companyNameGenerator[i].Ticker,
                    Number = i,
                    Cash = _startCash,
                    Debt = _startDebt,
                };
            }
        }
        private void SetStartCashAndStartDebt()
        {
            _startCash = _game.ParameterConstantList.Where(p => p.PrimaryParameter == AllConstantParameters.CashConstant)
                .Where(p => p.SecondaryParameterIndex == (int)CashConstantSecondary.StartCash)
                .Select(p => p.Constant)
                .FirstOrDefault();

            _startDebt = _game.ParameterConstantList.Where(p => p.PrimaryParameter == AllConstantParameters.CashConstant)
                .Where(p => p.SecondaryParameterIndex == (int)CashConstantSecondary.StartDebt)
                .Select(p => p.Constant)
                .FirstOrDefault();
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
