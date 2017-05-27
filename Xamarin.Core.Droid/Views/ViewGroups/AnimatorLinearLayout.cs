using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace Xamarin.Core.Droid.Views
{
    public interface IAnimatorViewGroup
    {
        float getXFraction();
        void setXFraction(float fraction);
    }

    public class AnimatorLinearLayout : LinearLayout, IAnimatorViewGroup, ViewTreeObserver.IOnPreDrawListener
    {
        public AnimatorLinearLayout(Context ctx) : base(ctx)
        {
        }

        private float _xFraction;

        [Export]
        public float getXFraction()
        {

            return _xFraction;
        }

        [Export]
        public void setXFraction(float fraction)
        {
            _xFraction = fraction;
            if (Width == 0)
            {
                ViewTreeObserver.AddOnPreDrawListener(this);
            }

            float translationX = Width * fraction;
            TranslationX = translationX;
            //System.Diagnostics.Debug.WriteLine($"TranslationX >>> {translationX}");
        }

        public bool OnPreDraw()
        {
            ViewTreeObserver.RemoveOnPreDrawListener(this);
            setXFraction(_xFraction);
            return true;
        }
    }
}

