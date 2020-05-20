using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.TouchTracking
{
    public class MapBoardCanvasView : SKCanvasView
    {
        private const double _minScale = 1;
        private const double _maxScale = 5;
        private const double _overShoot = 0.15;

        private double _startScale = 3;
        private double _lastScale;
        private double _startX;
        private double _startY;
        private SKPoint _touchPoint;
        private PageService _pageService;

        bool _tapHandled;

        public MapBoardCanvasView()
        {
            _pageService = new PageService();

            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            var doubleTap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            doubleTap.Tapped += OnDoubleTapped;
            GestureRecognizers.Add(doubleTap);

            Scale = _minScale;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = _minScale;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            _tapHandled = false;
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), ExecuteIfSingleTap);

            //if (Scale > _minScale)
            //{
            //    this.ScaleTo(_minScale, 250, Easing.CubicInOut);
            //    this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            //}
            //else
            //{
            //    AnchorX = AnchorY = 0.5; //TODO tapped position
            //    this.ScaleTo(_maxScale, 250, Easing.CubicInOut);
            //}
        }
        private bool ExecuteIfSingleTap()
        {

            _pageService.DisplayAlert("Single Tap");
            return false;
        }
        private void OnDoubleTapped(object sender, EventArgs e)
        {
            _tapHandled = true;
            ExecuteIfDoubleTap();

        }
        private async void ExecuteIfDoubleTap()
        {
            await _pageService.DisplayAlert("Double Tap");
        }
        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //await _pageService.DisplayAlert("Pan");

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = (1 - AnchorX) * Width;
                    _startY = (1 - AnchorY) * Height;
                    break;
                case GestureStatus.Running:
                    AnchorX = Clamp(1 - (_startX + e.TotalX) / Width, 0, 1);
                    AnchorY = Clamp(1 - (_startY + e.TotalY) / Height, 0, 1);
                    break;
            }
        }

        private async void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            await _pageService.DisplayAlert("Pinch");

            //switch (e.Status)
            //{
            //    case GestureStatus.Started:
            //        _lastScale = e.Scale;
            //        _startScale = Scale;
            //        AnchorX = e.ScaleOrigin.X;
            //        AnchorY = e.ScaleOrigin.Y;
            //        break;
            //    case GestureStatus.Running:
            //        if (e.Scale < 0 || Math.Abs(_lastScale - e.Scale) > (_lastScale * 1.3) - _lastScale)
            //        { return; }
            //        _lastScale = e.Scale;
            //        var current = Scale + (e.Scale - 1) * _startScale;
            //        Scale = Clamp(current, _minScale * (1 - _overShoot), _maxScale * (1 + _overShoot));
            //        break;
            //    case GestureStatus.Completed:
            //        if (Scale > _maxScale)
            //            this.ScaleTo(_maxScale, 250, Easing.SpringOut);
            //        else if (Scale < _minScale)
            //            this.ScaleTo(_minScale, 250, Easing.SpringOut);
            //        break;
            //}
        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }
    }
}

