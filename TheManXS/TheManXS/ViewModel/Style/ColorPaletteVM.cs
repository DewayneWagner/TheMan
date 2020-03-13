using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using TheManXS.ViewModel.Style;
using Xamarin.Forms;
using static TheManXS.ViewModel.Style.PaletteColorList;

namespace TheManXS.ViewModel.Style
{
    public class ColorPaletteVM : BaseViewModel
    {
        private PaletteColorList _paletteColorList;

        private Color _colorSelectedButton = Color.SeaGreen;
        private Color _colorNotSelectedButton = Color.LightGray;

        public ColorPaletteVM()
        {
            PaletteColorList = new PaletteColorList();

            InitColorListOC();
            CompressedLayout.SetIsHeadless(this, true);

            InitCommands();
        }

        public ColorPaletteVM(PaletteColor pc)
        {
            BVColor = pc.Color;
            ColorDescription = pc.Description;
            ID = pc.ID;

            C1ButtonColor = _colorNotSelectedButton;
            C1ButtonColor = Color.Black;
            C1ButtonColor = _colorNotSelectedButton;
            C1ButtonColor = _colorNotSelectedButton;
            C1ButtonColor = _colorNotSelectedButton;
        }
        public PaletteColorList PaletteColorList { get; set; }

        private ObservableCollection<ColorPaletteVM> _colorListOC;
        public ObservableCollection<ColorPaletteVM> ColorListOC
        {
            get => _colorListOC;
            set
            {
                _colorListOC = value;
                SetValue(ref _colorListOC,value);
            }
        }

        public Color BVColor  { get; set; }
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

        private void OnC0(object obj) => ProcessButtonClick(ColorTypes.C0, (Button)obj);
        private void OnC1(object obj) => ProcessButtonClick(ColorTypes.C1, (Button)obj);
        private void OnC2(object obj) => ProcessButtonClick(ColorTypes.C2, (Button)obj);
        private void OnC3(object obj) => ProcessButtonClick(ColorTypes.C3, (Button)obj);
        private void OnC4(object obj) => ProcessButtonClick(ColorTypes.C4, (Button)obj);

        private void InitColorListOC()
        {
            _paletteColorList = new PaletteColorList();
            ColorListOC = new ObservableCollection<ColorPaletteVM>();

            foreach (PaletteColor pc in _paletteColorList)
            {
                ColorListOC.Add(new ColorPaletteVM(pc));
            }
        }
        private void ProcessButtonClick(ColorTypes ct, Button button)
        {
            int id = (int)button.CommandParameter;

            switch (ct)
            {
                case ColorTypes.C0:
                    foreach(ColorPaletteVM cp in ColorListOC) { cp.C0ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C0ButtonColor = _colorSelectedButton;
                    // need to set "IsC0" property to true, and all others to false
                    break;
                case ColorTypes.C1:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C1ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C1ButtonColor = _colorSelectedButton;
                    break;
                case ColorTypes.C2:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C2ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C2ButtonColor = _colorSelectedButton;
                    break;
                case ColorTypes.C3:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C3ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C3ButtonColor = _colorSelectedButton;
                    break;
                case ColorTypes.C4:
                    foreach (ColorPaletteVM cp in ColorListOC) { cp.C4ButtonColor = _colorNotSelectedButton; }
                    ColorListOC[id].C4ButtonColor = _colorSelectedButton;
                    break;
                default:
                    break;
            }
        }
    }
}
