using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using Xamarin.Core;
using Xamarin.Core.ViewModels;

namespace Xamarin.Core.iOS.Views
{
    public abstract class BaseViewController<TViewModel> : BaseViewController
        where TViewModel : BaseViewModel
    {
        private bool _isBound;
        private readonly object _locker = new object();
        protected readonly List<Binding> _bindings = new List<Binding>();
        protected TViewModel _viewModel;

        public override INavigator Navigator
        {
            get { return base.Navigator; }
            set
            {
                base.Navigator = value;
                if (ViewModel != null)
                {
                    ViewModel.Navigator = value;
                }
            }
        }

        public BaseViewController()
        {
        }

        public BaseViewController(string nibName)
            : base(nibName)
        {
        }

        public BaseViewController(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// First version: read-only ViewModel
        /// Editable ViewModel will be implemented later
        /// </summary>
        /// <value>The view model.</value>
        public TViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    SetViewModel(ServiceLocator.Current.GetInstance<TViewModel>());
                }

                return _viewModel;
            }
            set
            {
                SetViewModel(value);
            }
        }

        private void SetViewModel(TViewModel viewModel)
        {
            if (viewModel != _viewModel)
            {
                //RemoveBindings();
                _viewModel = viewModel;
                if (_viewModel != null)
                {
                    _viewModel.Navigator = this.Navigator;
                }
                //SetBindingsIfNeed();
            }
        }

        public override void ViewWillUnload()
        {
            base.ViewWillUnload();

            ViewModel?.Cleanup();
            ViewModel = null;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            RemoveBindings();
            ViewModel.UnregisterMessages();
        }

        protected virtual void RemoveBindings()
        {
            _bindings.ForEach(x => x?.Detach());
            _bindings.Clear();
            _isBound = false;
            System.Diagnostics.Debug.WriteLine($">>> Remove bindings...{Key}");
        }

        public override void ViewWillAppear(bool animated)
        {
            SetBindingsIfNeed();
            ViewModel.RegisterMessages();

            base.ViewWillAppear(animated);
        }

        private void SetBindingsIfNeed()
        {
            if (!_isBound)
            {
                lock (_locker)
                {
                    if (!_isBound)
                    {
                        System.Diagnostics.Debug.WriteLine($">>> Set bindings...{Key}");
                        SetBindings();
                        _isBound = true;
                    }
                }
            }
        }

        protected abstract void SetBindings();

        public override void ViewControllerWillShow()
        {
            SetBindingsIfNeed();
        }
    }

    public abstract class BaseViewController : UIViewController, IScreen
    {
        protected const float KEYBOARD_OFFSET = 260.0f;

        protected KeyboardHandler _keyboardHandler;

        protected bool _enableKeyboard = true;
        protected bool _isFirstTime;

        public int Key { get; set; }

        public bool IsRoot { get; set; }

        public bool NoCache { get; set; }

        public object NavigationParameter { get; set; }

        public AnimationType AnimationType { get; set; }

        public virtual INavigator Navigator { get; set; }

        public bool IsNavigateBack { get; set; }

        public BaseViewController()
        {
        }

        public BaseViewController(string nibName)
            : base(nibName, null)
        {
        }

        public BaseViewController(IntPtr handle)
            : base(handle)
        {
        }

        private void Initialize()
        {
            //Disable scrollview auto set contentView below status bar
            AutomaticallyAdjustsScrollViewInsets = false;

            if (_keyboardHandler == null)
            {
                _keyboardHandler = new KeyboardHandler(View);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Initialize();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!IsNavigateBack && !_isFirstTime)
            {
                LoadData(NavigationParameter);
                _isFirstTime = true;
            }
            else
            {
                OnNavigateBack();
            }

            if (_enableKeyboard)
            {
                _keyboardHandler.RegisterKeyboardObserver();
            }
        }

        protected virtual void OnNavigateBack()
        {
        }

        protected virtual void LoadData(object arg)
        {
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (_enableKeyboard)
            {
                _keyboardHandler.RemoveKeyboardObserver();
            }
        }

        protected void EnableKeyboard(bool enableKeyboard = false, UIView upView = null)
        {
            _enableKeyboard = enableKeyboard;

            if (enableKeyboard && _keyboardHandler != null)
            {
                _keyboardHandler.View = upView ?? View;
            }
        }

        public virtual void ViewControllerWillShow()
        {
        }

        public virtual void ViewControllerDidShow()
        {
        }

        public void Log(string message)
        {
        }

        public IControl GetSharedControlInterface(string tag)
        {
            throw new NotImplementedException();
        }
    }
}

