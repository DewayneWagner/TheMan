using System.Collections.Generic;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Company
{
    public class CompanyNameGenerator
    {
        private List<string> _firstNameList;
        private List<string> _seconNamesList;
        private List<string> _lastNamesList;
        private List<CompanyNameGenerator> _listOfCompanyNames;

        System.Random rnd = new System.Random();

        public CompanyNameGenerator()
        {
            _firstNameList = FirstNameList();
            _seconNamesList = SecondNameList();
            _lastNamesList = LastNameList();
            _listOfCompanyNames = new List<CompanyNameGenerator>();

            LoadList();
        }

        private void LoadList()
        {
            string name, ticker;

            for (int i = 0; i < QC.PlayerQ; i++)
            {
                name = GetCompanyName();
                ticker = GetTicker(name);

                _listOfCompanyNames.Add(new CompanyNameGenerator(name, ticker));
            }
        }

        public CompanyNameGenerator this[int i]
        {
            get => _listOfCompanyNames[i];
            set => _listOfCompanyNames[i] = value;
        }

        public CompanyNameGenerator(string name, string ticker)
        {
            Name = name;
            Ticker = ticker;
        }

        public string Name { get; }
        public string Ticker { get; }

        private string GetCompanyName()
        {
            string first, second, third;

            setFirstName();
            setSecond();
            setThird();

            return (first + " " + second + " " + third);

            void setFirstName()
            {
                first = _firstNameList[rnd.Next(_firstNameList.Count)];
                _firstNameList.Remove(first);
            }
            void setSecond()
            {
                second = _seconNamesList[rnd.Next(_seconNamesList.Count)];
                _seconNamesList.Remove(second);
            }
            void setThird()
            {
                third = _lastNamesList[rnd.Next(_lastNamesList.Count)];
            }
        }

        private string GetTicker(string name)
        {
            string[] names = name.Split();
            string ticker = null;

            for (int i = 0; i < names.Length; i++)
            {
                char[] charA = names[i].ToCharArray();
                ticker += charA[0];
            }
            return ticker;
        }

        private List<string> FirstNameList()
        {
            List<string> firstNameList = new List<string>()
            {
                "Red", "Golden","Black","Mid","North","South","East","West","Frontier","Rushing","Swift",
                "Neutral","Yellow", "Blue","Smith",
            };
            return firstNameList;
        }
        private List<string> SecondNameList()
        {
            List<string> secondNames = new List<string>()
            {
                "Hills","Mountains","River","Creek","Lake","Lion","Resources","Minerals","Energy",
                "Mining","Stream","Bear","Tree", "Moose",
            };
            return secondNames;
        }
        private List<string> LastNameList()
        {
            List<string> lastNames = new List<string>()
            {
                "Corp","Corporation","Ltd","Limited","Group","Incorporated","Enterprises",
                "Inc",
            };
            return lastNames;
        }
    }
}
