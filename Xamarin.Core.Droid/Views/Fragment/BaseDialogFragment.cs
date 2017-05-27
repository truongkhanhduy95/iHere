using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core.ViewModels;

namespace Xamarin.Core.Droid
{
    public abstract class BaseDialogFragment : DialogFragment
    {
        protected Dictionary<object, Binding> _bindings = new Dictionary<object, Binding>();

        private Action _onAfterShownDialog;

        public Action OnAfterShownDialog
        {
            get
            {
                return _onAfterShownDialog;
            }

            set
            {

                _onAfterShownDialog = value;
            }
        }


        public override void OnStart()
        {
            base.OnStart();
            SetBindings();
        }

        public override void OnStop()
        {
            base.OnStop();
            RemoveBindings();
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);

            if (OnAfterShownDialog != null)
            {
                OnAfterShownDialog();
            }
        }

        protected virtual void RemoveBindings()
        {
            foreach (var entry in _bindings)
            {
                entry.Value.Detach();
            }
            _bindings.Clear();
        }

        protected abstract void SetBindings();
    }

    public abstract class BaseDialogFragment<TViewModel> : BaseDialogFragment where TViewModel : BaseViewModel
    {
        protected TViewModel _viewModel;

        public virtual TViewModel VM
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = ServiceLocator.Current.GetInstance<TViewModel>();
                }

                return _viewModel;
            }

            protected set
            {
                _viewModel = value;
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);

            //VM.ApplyDefaultValue();

            return dialog;
        }

    }
}

