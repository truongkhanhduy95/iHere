using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Helpers;

namespace Xamarin.Core.Utils
{
    public class RelayCommandEx<TResult> : RelayCommand
    {
        private readonly WeakFunc<TResult> _execute;

        public RelayCommandEx(Func<TResult> execute)
            : this(execute, null)
        {
        }

        public RelayCommandEx(Func<TResult> execute, Func<bool> canExecute) : base(() => execute.Invoke(), canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakFunc<TResult>(execute);
        }

        public new TResult Execute(object parameter)
        {
            var val = parameter;

            if (CanExecute(val)
                && _execute != null
                && (_execute.IsStatic || _execute.IsAlive))
            {
                return _execute.Execute();
            }

            // LATER: Handle if need
            return default(TResult);
        }
    }

    public class RelayCommand<TParam, TResult> : RelayCommand<TParam>
    {
        private readonly WeakFunc<TParam, TResult> _execute;

        public RelayCommand(Func<TParam, TResult> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Func<TParam, TResult> execute, Func<TParam, bool> canExecute) : base((obj) => execute.Invoke(obj), canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakFunc<TParam, TResult>(execute);
        }

        public new TResult Execute(object parameter)
        {
            var val = parameter;

            if (CanExecute(val)
                && _execute != null
                && (_execute.IsStatic || _execute.IsAlive))
            {
                if (val == null)
                {
                    return _execute.Execute(default(TParam));
                }
                else
                {
                    return _execute.Execute((TParam)val);
                }
            }

            // LATER: Handle if need
            return default(TResult);
        }
    }
}

