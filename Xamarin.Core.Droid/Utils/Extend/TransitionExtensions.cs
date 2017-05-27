using System;
using Android.Animation;
using Android.App;

namespace Xamarin.Core.Droid
{
    public static class TransitionExtensions
    {
        public static Animator LoadAnimator(AnimationType animationType, FragmentTransit transit, bool isEnter)
        {
            switch (animationType)
            {
                case AnimationType.Fade:
                    return GetFadeAnimation(transit, isEnter);
                default:
                    return null;
            }
        }

        private static Animator GetFadeAnimation(FragmentTransit transit, bool isEnter)
        {
            if (isEnter && transit == FragmentTransit.FragmentOpen)
            {
                return AnimatorInflater.LoadAnimator(Application.Context, Resource.Animator.fade_in);
            }
            if (!isEnter && transit == FragmentTransit.FragmentClose)
            {
                return AnimatorInflater.LoadAnimator(Application.Context, Resource.Animator.fade_out);
            }
            return null;
        }
    }
}

