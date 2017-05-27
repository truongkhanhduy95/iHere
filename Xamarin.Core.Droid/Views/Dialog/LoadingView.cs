
using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;

namespace Xamarin.Core.Droid
{
    public class LoadingView : SurfaceView
    {
        #region Constants

        private const int ANIMATE_DURATION = 1000;
        private const int REFRESH_TIME = 30;
        private const float ALPHA_MAX = 1;
        private const float ALPHA_MIN = 0.5f;

        #endregion

        #region Fields

        private ValueAnimator _animator;
        private long _currentDuration;
        private List<Tuple<float, float>> _positions = new List<Tuple<float, float>>();

        #endregion

        #region Properties

        public virtual Color Color { get; set; } = Color.DarkGray;

        public virtual float RadiusMax
        {
            get { return Width / 8f; }
        }

        public virtual float RadiusMin
        {
            get { return Width / 16f; }
        }

        private bool _running = true;

        public bool Running
        {
            get { return _running; }
            set
            {
                if (_animator != null)
                {
                    if (!value)
                    {
                        _animator.RepeatCount = 0;
                    }
                    else
                    {
                        _animator.RepeatCount = int.MaxValue;
                    }
                    _animator.Start();
                }
                _running = value;
            }
        }

        #endregion

        public LoadingView(Context context)
            : base(context)
        {
            Initialize();
        }

        public LoadingView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public LoadingView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var size = Context.Resources.GetDimensionPixelSize(Resource.Dimension.loading_view_size);

            var widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightSize = MeasureSpec.GetSize(heightMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            int width = 0, height = 0;

            switch (widthMode)
            {
                case MeasureSpecMode.AtMost:
                    width = Math.Min(widthSize, size);
                    break;
                case MeasureSpecMode.Exactly:
                    width = widthSize;
                    break;
                case MeasureSpecMode.Unspecified:
                    width = size;
                    break;
            }

            switch (heightMode)
            {
                case MeasureSpecMode.AtMost:
                    height = Math.Min(heightSize, size);
                    break;
                case MeasureSpecMode.Exactly:
                    height = heightSize;
                    break;
                case MeasureSpecMode.Unspecified:
                    height = size;
                    break;
            }

            SetMeasuredDimension(width, height);
        }

        private void Initialize()
        {
            SetBackgroundColor(Color.Transparent);
            SetZOrderOnTop(true);
            Holder.SetFormat(Format.Transparent);

            if (Running)
            {
                _animator = ValueAnimator.OfFloat(0, ANIMATE_DURATION / REFRESH_TIME);
                _animator.SetDuration(ANIMATE_DURATION);
                _animator.Update += OnAnimatorUpdate;
                _animator.RepeatMode = ValueAnimatorRepeatMode.Restart;
                _animator.RepeatCount = int.MaxValue;
                _animator.SetInterpolator(new LinearInterpolator());
                _animator.Start();
            }
        }

        private void OnAnimatorUpdate(object sender, ValueAnimator.AnimatorUpdateEventArgs e)
        {
            var value = (long)e.Animation.AnimatedValue;
            _currentDuration = value * REFRESH_TIME;
            Invalidate();
        }

        private void CalculatePositions()
        {
            var width = (float)Width;
            var height = (float)Height;
            var haftWidth = (width - (2f * RadiusMax)) / 2f;
            var haftHeight = (height - (2f * RadiusMax)) / 2f;

            _positions.Add(CreatePosition(width / 2f, RadiusMax));
            _positions.Add(CreatePosition((float)((width - haftWidth * Math.Sqrt(2f)) / 2), (float)((height - haftHeight * Math.Sqrt(2f)) / 2)));
            _positions.Add(CreatePosition(RadiusMax, height / 2f));
            _positions.Add(CreatePosition((float)((width - haftWidth * Math.Sqrt(2f)) / 2), (float)((height + haftHeight * Math.Sqrt(2f)) / 2)));
            _positions.Add(CreatePosition(width / 2f, height - RadiusMax));
            _positions.Add(CreatePosition((float)((width + haftWidth * Math.Sqrt(2f)) / 2), (float)((height + haftHeight * Math.Sqrt(2f)) / 2)));
            _positions.Add(CreatePosition(width - RadiusMax, height / 2f));
            _positions.Add(CreatePosition((float)((width + haftWidth * Math.Sqrt(2f)) / 2), (float)((height - haftHeight * Math.Sqrt(2f)) / 2)));
        }

        private Tuple<float, float> CreatePosition(float x, float y)
        {
            return new Tuple<float, float>(x, y);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (_positions.Count <= 0)
            {
                CalculatePositions();
            }

            var width = (float)Width;
            var height = (float)Height;
            var haftWidth = (width - (2f * RadiusMax)) / 2f;
            var haftHeight = (height - (2f * RadiusMax)) / 2f;

            var paint = new Paint();
            paint.Color = Color;

            var circle2Info = GetStatusInfo(7f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle2Info.Item2;
            canvas.DrawCircle(_positions[7].Item1, _positions[7].Item2, circle2Info.Item1, paint);

            var circle3Info = GetStatusInfo(6f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle3Info.Item2;
            canvas.DrawCircle(_positions[6].Item1, _positions[6].Item2, circle3Info.Item1, paint);

            var circle4Info = GetStatusInfo(5f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle4Info.Item2;
            canvas.DrawCircle(_positions[5].Item1, _positions[5].Item2, circle4Info.Item1, paint);

            var circle5Info = GetStatusInfo(4f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle5Info.Item2;
            canvas.DrawCircle(_positions[4].Item1, _positions[4].Item2, circle5Info.Item1, paint);

            var circle6Info = GetStatusInfo(3f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle6Info.Item2;
            canvas.DrawCircle(_positions[3].Item1, _positions[3].Item2, circle6Info.Item1, paint);

            var circle7Info = GetStatusInfo(2f * ANIMATE_DURATION / 8f);
            paint.Alpha = circle7Info.Item2;
            canvas.DrawCircle(_positions[2].Item1, _positions[2].Item2, circle7Info.Item1, paint);

            var circle8Info = GetStatusInfo(ANIMATE_DURATION / 8f);
            paint.Alpha = circle8Info.Item2;
            canvas.DrawCircle(_positions[1].Item1, _positions[1].Item2, circle8Info.Item1, paint);

            var circle1Info = GetStatusInfo(0);
            paint.Alpha = circle1Info.Item2;
            canvas.DrawCircle(_positions[0].Item1, _positions[0].Item2, circle1Info.Item1, paint);
        }

        private Tuple<float, int> GetStatusInfo(float phase)
        {
            var ratio = (_currentDuration + phase) / ANIMATE_DURATION;
            var radius = CalculateNeededValue(ratio, RadiusMin, RadiusMax);
            var alpha = CalculateNeededValue(ratio, ALPHA_MIN, ALPHA_MAX);

            return new Tuple<float, int>(radius, (int)(alpha * 255));
        }

        private float CalculateNeededValue(float ratio, float minValue, float maxValue)
        {
            var value = minValue / 2f * Math.Cos(ratio * 2 * Math.PI) + 3f * minValue / 2f;
            return (float)value;
        }
    }
}

