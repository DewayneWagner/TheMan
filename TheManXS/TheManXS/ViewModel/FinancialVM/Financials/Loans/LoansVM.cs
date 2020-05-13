using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Financial.Debt;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.Loans
{
    class LoansVM : BaseViewModel
    {
        Game _game;
        public LoansVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            InitList();
            GetLoan = new Command(OnGetLoan);
            LoanAction = new Command(OnLoanAction);
        }

        private void InitList()
        {
            foreach (Loan loan in _game.LoanList) { LoanList.Add(new LoansVM(loan)); }
        }
        public LoansVM(Loan loan)
        {
            ID = loan.ID;
            Term = (int)loan.Term;
            InterestRate = loan.InterestRate;
            StartingBalance = loan.StartingBalance;
            PrincipalPaymentPerTurn = loan.PrincipalPaymentPerTurn;
            InterestPaymentPerTurn = loan.InterestPaymentPerTurn;
            TurnsRemaining = loan.TurnsRemaining;
            CompanyName = _game.PlayerList[loan.PlayerNumber].Name;
        }

        private List<LoansVM> _loanList;
        public List<LoansVM> LoanList
        {
            get => _loanList;
            set
            {
                _loanList = value;
                SetValue(ref _loanList, value);
            }
        }

        public ICommand GetLoan { get; set; }
        public ICommand LoanAction { get; set; }

        // properties of each loan to display
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int Term { get; set; }
        public double InterestRate { get; set; }
        public double StartingBalance { get; set; }
        public double PrincipalPaymentPerTurn { get; set; }
        public double InterestPaymentPerTurn { get; set; }
        public int TurnsRemaining { get; set; }

        private void OnGetLoan(object obj) {; }
        private void OnLoanAction(object obj) {; }

    }
}
