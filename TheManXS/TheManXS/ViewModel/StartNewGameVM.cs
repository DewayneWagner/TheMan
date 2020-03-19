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
        private double _startButtonOpacity = 0.9, _notSelectedButtonOpacity = 0.5,
            _easyButtonOpacity, _mediumButtonOpacity, _hardButtonOpacity;

        private List<GameSpecificParameters> _savedGameSlotsList;

        private int _companyColorIndex;
        private string _companyName, _ticker;

        private Difficulty _difficulty;
        private bool isDifficultySelected, isColorSelected;
        public bool IsGameSlotSelected;
        private GameSpecificParameters _gameSaveSlot;
        private PageService _pageService;
        CompanyColorGenerator _companyColorGenerator;

        public StartNewGameVM()
        {
            SavedGameSlotsList = GameSpecificParameters.GetListOfSavedGameData();

            InitCommands();
            InitOpacity();

            SelectedGameSaveSlot = new GameSpecificParameters();
            _pageService = new PageService();
            CompressedLayout.SetIsHeadless(this, true);
        }

        public List<string> CompanyColorList
        {
            get
            {
                _companyColorGenerator = new CompanyColorGenerator();
                return _companyColorGenerator.GetListOfAvailableSKColors();
            }
        }

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

        private void InitOpacity()
        {
            EasyButtonOpacity = _startButtonOpacity;
            MediumButtonOpacity = _startButtonOpacity;
            HardButtonOpacity = _startButtonOpacity;
        }

        public ICommand Easy { get; set; }
        public ICommand Medium { get; set; }
        public ICommand Hard { get; set; }
        public ICommand StartNewGame { get; set; }
        public ICommand UseTestData { get; set; }

        private void OnEasyButton()
        {
            Difficulty = Difficulty.Easy;
            EasyButtonOpacity = _startButtonOpacity;
            MediumButtonOpacity = _notSelectedButtonOpacity;
            HardButtonOpacity = _notSelectedButtonOpacity;
            isDifficultySelected = true;
        }

        private void OnMediumButton()
        {
            Difficulty = Difficulty.Medium;
            EasyButtonOpacity = _notSelectedButtonOpacity;
            MediumButtonOpacity = _startButtonOpacity;
            HardButtonOpacity = _notSelectedButtonOpacity;
            isDifficultySelected = true;
        }

        private void OnHardButton()
        {
            Difficulty = Difficulty.Hard;
            EasyButtonOpacity = _notSelectedButtonOpacity;
            MediumButtonOpacity = _notSelectedButtonOpacity;
            HardButtonOpacity = _startButtonOpacity;
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

        private async void GoToGameBoard() => await _pageService.PushAsync(new MapBoard());
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

        private void InitCommands()
        {
            Easy = new Command(OnEasyButton);
            Medium = new Command(OnMediumButton);
            Hard = new Command(OnHardButton);
            StartNewGame = new Command(StartNewGameMethod);
            UseTestData = new Command(AddTestData);
            WTF = new Command<string>((wtf) => DisplayHelpMessage(wtf));
        }

        enum WTFMessagesEnum { Easy, Medium, Hard, CompanyName, Ticker, CompanyColor, Total }
        public ICommand WTF { get; set; }

        // doesn't work when a new Icommand is set-up without parameter
        //private async void DisplayHelpMessage(string wtfType) doesn't work
        //{
        //    await _pageService.DisplayAlert("blah", "blah", "OK", "Cancel");
        //}

        private async void DisplayHelpMessage(string wtfType)
        {
            int index = 0;
            setIndex();
            WTFMessagesEnum wtf = (WTFMessagesEnum)index;
            string message = null;
            setMessage();

            await _pageService.DisplayAlert(message);

            //App.Current.MainPage.DisplayAlert("blah", "blah", "blah"); doesn't work either
            //ContentPage c = new ContentPage();
            //await c.DisplayAlert("blah", "blah", "blah"); doesn't work
            //await _pageService.DisplayAlert(message); doesn't work either

            void setIndex()
            {
                for (int i = 0; i < (int)WTFMessagesEnum.Total; i++) 
                { if(wtfType == Convert.ToString((WTFMessagesEnum)i)) { index = i; }}
            }
            void setMessage()
            {
                switch (wtf)
                {
                    case WTFMessagesEnum.Easy:
                        message += "This is for the fresh-out-of business college types - ready to take over" +
                            "\n" + "the family company and take it into the future!";
                        break;
                    case WTFMessagesEnum.Medium:
                        message += "You've put in a couple of years - and know it all!" + "\n" +
                            "This level introduces a few more difficulties - including the declining production" + "\n" +
                            "over time, and stronger competition.";
                        break;
                    case WTFMessagesEnum.Hard:
                        message += "This one is for only the real hard-cores, and reflects the true realities of " + "\n" +
                            "trying to operate a business in Canada.  Any development project requires a " + "\n" +
                            "wait of up to 20 turns to get approval (if it is approved), all your resources " + "\n" +
                            "are sold for 20-40% less then market, and operating costs rise drastically over time.";
                        break;
                    case WTFMessagesEnum.CompanyName:
                        message += "The name of your company - pick anything.";
                        break;
                    case WTFMessagesEnum.Ticker:
                        message += "The 3 letter symbol that your stock is tracked and reported in.";
                        break;
                    case WTFMessagesEnum.CompanyColor:
                        message += "The company color that will be used to mark your territory and facilities.";
                        break;
                    case WTFMessagesEnum.Total:
                    default:
                        break;
                }
            }
        }
    }
}
