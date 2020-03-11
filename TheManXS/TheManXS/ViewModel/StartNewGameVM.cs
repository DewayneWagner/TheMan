using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Company;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Settings;
using TheManXS.View;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.ViewModel
{
    public class StartNewGameVM : BaseViewModel
    {
        #region(Fields)
        private double selectedButtonOpacity = 1, notSelectedButtonOpacity = 0.5,
            _easyButtonOpacity, _mediumButtonOpacity, _hardButtonOpacity;

        private List<GameSpecificParameters> _savedGameSlotsList;

        private int _companyColorIndex;
        private string _companyName, _ticker;

        private Difficulty _difficulty;
        private bool isDifficultySelected, isColorSelected;
        public bool IsGameSlotSelected;
        private GameSpecificParameters _gameSaveSlot;
        private PageService _pageService = new PageService();
        CompanyColorGenerator _companyColorGenerator;

        #endregion

        #region(Constructors)
        public StartNewGameVM()
        {
            SavedGameSlotsList = GameSpecificParameters.GetListOfSavedGameData();

            Easy = new Command(OnEasyButton);
            Medium = new Command(OnMediumButton);
            Hard = new Command(OnHardButton);
            StartNewGame = new Command(StartNewGameMethod);
            UseTestData = new Command(AddTestData);

            EasyButtonOpacity = 1;
            MediumButtonOpacity = 1;
            HardButtonOpacity = 1;

            SelectedGameSaveSlot = new GameSpecificParameters();
            CompressedLayout.SetIsHeadless(this, true);
        }
        #endregion

        public List<string> CompanyColorList
        {
            get
            {
                _companyColorGenerator = new CompanyColorGenerator();
                return _companyColorGenerator.GetListOfAvailableSKColors();
            }
        }

        #region(Properties)

        public int CompanyColorIndex
        {
            get => _companyColorIndex;
            set
            {
                _companyColorIndex = value;
                SetValue(ref _companyColorIndex, value);
            }
        }

        public Difficulty Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                SetValue(ref _difficulty, value);
            }
        }
        public string DifficultyString => Convert.ToString(_difficulty);

        public double EasyButtonOpacity
        {
            get => _easyButtonOpacity;
            set
            {
                _easyButtonOpacity = value;
                SetValue(ref _easyButtonOpacity, value);
            }
        }

        public double MediumButtonOpacity
        {
            get => _mediumButtonOpacity;
            set
            {
                _mediumButtonOpacity = value;
                SetValue(ref _mediumButtonOpacity, value);
            }
        }

        public double HardButtonOpacity
        {
            get => _hardButtonOpacity;
            set
            {
                _hardButtonOpacity = value;
                SetValue(ref _hardButtonOpacity, value);
            }
        }

        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                SetValue(ref _companyName, value);
            }
        }

        public string Ticker
        {
            get => _ticker;
            set
            {
                _ticker = value;
                SetValue(ref _ticker, value);
            }
        }

        public GameSpecificParameters SelectedGameSaveSlot
        {
            get => _gameSaveSlot;
            set
            {
                _gameSaveSlot = value;
                SetValue(ref _gameSaveSlot, value);
            }
        }

        public List<GameSpecificParameters> SavedGameSlotsList // referenced by listview
        {
            get => _savedGameSlotsList;
            set
            {
                _savedGameSlotsList = value;
                SetValue(ref _savedGameSlotsList, value);
            }
        }

        #endregion

        #region(Interfaces)
        public ICommand Easy { get; set; }
        public ICommand Medium { get; set; }
        public ICommand Hard { get; set; }
        public ICommand StartNewGame { get; set; }
        public ICommand UseTestData { get; set; }

        #endregion

        #region(Methods)
        private void OnEasyButton()
        {
            Difficulty = Difficulty.Easy;
            EasyButtonOpacity = selectedButtonOpacity;
            MediumButtonOpacity = notSelectedButtonOpacity;
            HardButtonOpacity = notSelectedButtonOpacity;
            isDifficultySelected = true;
        }
        private void OnMediumButton()
        {
            Difficulty = Difficulty.Medium;
            EasyButtonOpacity = notSelectedButtonOpacity;
            MediumButtonOpacity = selectedButtonOpacity;
            HardButtonOpacity = notSelectedButtonOpacity;
            isDifficultySelected = true;
        }
        private void OnHardButton()
        {
            Difficulty = Difficulty.Hard;
            EasyButtonOpacity = notSelectedButtonOpacity;
            MediumButtonOpacity = notSelectedButtonOpacity;
            HardButtonOpacity = selectedButtonOpacity;
            isDifficultySelected = true;
        }        

        private void StartNewGameMethod(object obj)
        {
            ContentPage c = new ContentPage();

            if (!isDifficultySelected) { c.DisplayAlert("Difficulty", "Difficulty must be selected, dumbass.", "OK"); }                
            else if(!IsGameSlotSelected) { c.DisplayAlert("Game Save Slot", "Saved Game Slot must be selected, dumbass", "OK"); }
            else if(!isColorSelected) { c.DisplayAlert("Company Color", "You have to pick a Company Color, dumbass", "OK"); }
            else if(CompanyName == null) { c.DisplayAlert("Company Name", "You have to enter a Company Name, dumbass", "OK"); }
            else if(Ticker.Length != 3) { c.DisplayAlert("Company Ticker", "Company Ticker has to be 3 letters, dumbass", "OK"); }

            else
            {
                SKColor companyColor = _companyColorGenerator.GetSKColor(CompanyColorIndex);
                _companyColorGenerator.RemoveSelectedColorFromOptions(companyColor);                

                GameSpecificParameters gsp = new GameSpecificParameters()
                {
                    Slot = SelectedGameSaveSlot.Slot,
                    CompanyColor = companyColor,
                    CompanyName = CompanyName,
                    Diff = Difficulty,
                    PlayerNumber = 0,
                    Ticker = Ticker,
                };
                gsp.CompanyColorGenerator = _companyColorGenerator;

                using (DBContext db = new DBContext())
                {
                    var oldGame = db.GameSpecificParameters.Where(o => o.Slot == gsp.Slot);
                    var g = oldGame.FirstOrDefault<GameSpecificParameters>();

                    if (g != null) { db.Remove(g); }                                        
                    db.Add(gsp);
                    db.SaveChanges();
                }
                new Game(gsp, true);
                GoToGameBoard();
            }            
        }
        private async void GoToGameBoard() => await _pageService.PushAsync(new MapBoard()); //(new GameBoardView());
        private void AddTestData()
        {
            CompanyColorIndex = 0;
            SelectedGameSaveSlot = SavedGameSlotsList[1];
            IsGameSlotSelected = true;
            CompanyName = "Rockyspring Ltd.";
            Difficulty = Difficulty.Medium;
            Ticker = "RSL";
            EasyButtonOpacity = 0.5;
            HardButtonOpacity = 0.5;
            isDifficultySelected = true;
            isColorSelected = true;
        }
        #endregion
    }
}
