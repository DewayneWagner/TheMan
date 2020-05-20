using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.ViewModel.Style.PaletteColorList;

namespace TheManXS.ViewModel.Style
{
    public class ColorPaletteVM : BaseViewModel
    {
        private PaletteColorList _paletteColorList;

        private Color _colorSelectedButton = Color.Red;
        private Color _colorNotSelectedButton = Color.LightSeaGreen;

        public ColorPaletteVM()
        {
            PaletteColorList = new PaletteColorList();

            InitCommands();
            InitColorListOC();
            CompressedLayout.SetIsHeadless(this, true);
        }

        public ColorPaletteVM(PaletteColor pc)
        {
            BVColor = pc.Color;
            ColorDescription = pc.Description;
            ID = pc.ID;
            if (pc.IsC0 == true) { C0ButtonColor = _colorSelectedButton; }
        }

        public void UpdateSelectedColors()
        {
            int indexOfC0 = PaletteColorList.Where(c => c.IsC0).Select(c => c.ID).FirstOrDefault();
            int indexOfC1 = PaletteColorList.Where(c => c.IsC1).Select(c => c.ID).FirstOrDefault();
            int indexOfC2 = PaletteColorList.Where(c => c.IsC2).Select(c => c.ID).FirstOrDefault();
            int indexOfC3 = PaletteColorList.Where(c => c.IsC3).Select(c => c.ID).FirstOrDefault();
            int indexOfC4 = PaletteColorList.Where(c => c.IsC4).Select(c => c.ID).FirstOrDefault();

            ColorListOC[indexOfC0].C0ButtonColor = _colorSelectedButton;
            ColorListOC[indexOfC1].C1ButtonColor = _colorSelectedButton;
            ColorListOC[indexOfC2].C2ButtonColor = _colorSelectedButton;
            ColorListOC[indexOfC3].C3ButtonColor = _colorSelectedButton;
            ColorListOC[indexOfC4].C4ButtonColor = _colorSelectedButton;
        }

        public PaletteColorList PaletteColorList { get; set; }

        private ObservableCollection<ColorPaletteVM> _colorListOC;
        public ObservableCollection<ColorPaletteVM> ColorListOC
        {
            get => _colorListOC;
            set
            {
                _colorListOC = value;
                SetValue(ref _colorListOC, value);
            }
        }

        public Color BVColor { get; set; }
        public string ColorDescription { get; set; }
        public int ID { get; set; }

        private Color _c0ButtonColor;
        public Color C0ButtonColor
        {
            get => _c0ButtonColor;
            set
            {
                _c0ButtonColor = value;
                SetValue(ref _c0ButtonColor, value);
            }
        }

        private Color _c1ButtonColor;
        public Color C1ButtonColor
        {
            get => _c1ButtonColor;
            set
            {
                _c1ButtonColor = value;
                SetValue(ref _c1ButtonColor, value);
            }
        }

        private Color _c2ButtonColor;
        public Color C2ButtonColor
        {
            get => _c2ButtonColor;
            set
            {
                _c2ButtonColor = value;
                SetValue(ref _c2ButtonColor, value);
            }
        }

        private Color _c3ButtonColor;
        public Color C3ButtonColor
        {
            get => _c3ButtonColor;
            set
            {
                _c3ButtonColor = value;
                SetValue(ref _c3ButtonColor, value);
            }
        }

        private Color _c4ButtonColor;
        public Color C4ButtonColor
        {
            get => _c4ButtonColor;
            set
            {
                _c4ButtonColor = value;
                SetValue(ref _c4ButtonColor, value);
            }
        }

        public ICommand C0 { get; set; }
        public ICommand C1 { get; set; }
        public ICommand C2 { get; set; }
        public ICommand C3 { get; set; }
        public ICommand C4 { get; set; }

        void InitCommands()
        {
            C0 = new Command(OnC0);
            C1 = new Command(OnC1);
            C2 = new Command(OnC2);
            C3 = new Command(OnC3);
            C4 = new Command(OnC4);
        }

        private void OnC0(object obj) => ProcessButtonClick(ColorTypes.C0, (int)obj);
        private void OnC1(object obj) => ProcessButtonClick(ColorTypes.C1, (int)obj);
        private void OnC2(object obj) => ProcessButtonClick(ColorTypes.C2, (int)obj);
        private void OnC3(object obj) => ProcessButtonClick(ColorTypes.C3, (int)obj);
        private void OnC4(object obj) => ProcessButtonClick(ColorTypes.C4, (int)obj);

        private void InitColorListOC()
        {
            _paletteColorList = new PaletteColorList();
            ColorListOC = new ObservableCollection<ColorPaletteVM>();

            foreach (PaletteColor pc in _paletteColorList)
            {
                ColorListOC.Add(new ColorPaletteVM(pc));
            }
        }
        private void ProcessButtonClick(ColorTypes ct, int id)
        {
            switch (ct)
            {
                case ColorTypes.C0:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C0ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C0ButtonColor = _colorSelectedButton;
                    foreach (PaletteColor pc in PaletteColorList) { pc.IsC0 = false; }
                    PaletteColorList[id].IsC0 = true;
                    break;
                case ColorTypes.C1:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C1ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C1ButtonColor = _colorSelectedButton;
                    foreach (PaletteColor pc in PaletteColorList) { pc.IsC1 = false; }
                    PaletteColorList[id].IsC1 = true;
                    break;
                case ColorTypes.C2:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C2ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C2ButtonColor = _colorSelectedButton;
                    foreach (PaletteColor pc in PaletteColorList) { pc.IsC2 = false; }
                    PaletteColorList[id].IsC2 = true;
                    break;
                case ColorTypes.C3:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C3ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C3ButtonColor = _colorSelectedButton;
                    foreach (PaletteColor pc in PaletteColorList) { pc.IsC3 = false; }
                    PaletteColorList[id].IsC3 = true;
                    break;
                case ColorTypes.C4:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C4ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C4ButtonColor = _colorSelectedButton;
                    foreach (PaletteColor pc in PaletteColorList) { pc.IsC4 = false; }
                    PaletteColorList[id].IsC4 = true;
                    break;
                default:
                    break;
            }
        }
    }
}
