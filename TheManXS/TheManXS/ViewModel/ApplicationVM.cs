using System.Linq;
using TheManXS.ViewModel.Services;
using TheManXS.ViewModel.Style;
using Xamarin.Forms;

namespace TheManXS.ViewModel
{
    class ApplicationVM : BaseViewModel
    {
        PaletteColorList _paletteColorList;
        public ApplicationVM()
        {
            _paletteColorList = new PaletteColorList();
        }

        private Color _isC0;
        public Color C0
        {
            get => _isC0;
            set
            {
                _isC0 = value;
                SetValue(ref _isC0, value);
            }
        }

        private Color _isC1;
        public Color C1
        {
            get => _isC1;
            set
            {
                _isC1 = value;
                SetValue(ref _isC1, value);
            }
        }

        private Color _isC2;
        public Color C2
        {
            get => _isC2;
            set
            {
                _isC2 = value;
                SetValue(ref _isC2, value);
            }
        }

        private Color _isC3;
        public Color C3
        {
            get => _isC3;
            set
            {
                _isC3 = value;
                SetValue(ref _isC3, value);
            }
        }

        private Color _isC4;
        public Color C4
        {
            get => _isC4;
            set
            {
                _isC4 = value;
                SetValue(ref _isC4, value);
            }
        }

        public void InitMainColors()
        {
            C0 = _paletteColorList.Where(c => c.IsC0).Select(c => c.Color).FirstOrDefault();
            C1 = _paletteColorList.Where(c => c.IsC1).Select(c => c.Color).FirstOrDefault();
            C2 = _paletteColorList.Where(c => c.IsC2).Select(c => c.Color).FirstOrDefault();
            C3 = _paletteColorList.Where(c => c.IsC3).Select(c => c.Color).FirstOrDefault();
            C4 = _paletteColorList.Where(c => c.IsC4).Select(c => c.Color).FirstOrDefault();
        }

        public void UpdateColors(PaletteColorList c)
        {
            C0 = c.C0;
            C1 = c.C1;
            C2 = c.C2;
            C3 = c.C3;
            C4 = c.C4;
        }
    }
}
