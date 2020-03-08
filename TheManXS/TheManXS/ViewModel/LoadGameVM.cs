using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Main;
using TheManXS.Model.Settings;
using Xamarin.Forms;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.ViewModel.DetailPages
{
    public class LoadGameVM
    {
        public LoadGameVM()
        {
            SavedGamesList = GameSpecificParameters.GetListOfSavedGameData();
            LoadGame = new Command(LoadGameMethod);
            SelectedGSP = new GameSpecificParameters();
        }
        public List<GameSpecificParameters> SavedGamesList { get; set; }
        public string CompanyName { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime LastPlayed { get; set; }
        public ICommand LoadGame { get; set; }
        public GameSpecificParameters SelectedGSP { get; set; }
        private void LoadGameMethod(object obj)
        {
            new Game(SelectedGSP, false);
        }
    }
}
