using System;
using System.Windows.Input;

namespace KMS2_02_LE_03_01.MVVM
{
    /// <summary>
    /// An implementation of ICommand used for binding commands to the user interface.
    /// </summary>
    public class CustomRelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the CustomRelayCommand class.
        /// </summary>
        /// <param name="execute">The action to be executed when the command is invoked.</param>
        /// <param name="canExecute">The function that determines whether the command can execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if the execute argument is null.</exception>
        public CustomRelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">A parameter that can be used by the command.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Executes the action associated with the command.
        /// </summary>
        /// <param name="parameter">A parameter that can be used by the command.</param>
        public void Execute(object parameter)
        {
            _execute();
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    /// <summary>
    /// A generic implementation of ICommand used for binding commands to the user interface with a parameter.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class CustomRelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the CustomRelayCommand class.
        /// </summary>
        /// <param name="execute">The action to be executed when the command is invoked.</param>
        /// <param name="canExecute">The function that determines whether the command can execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if the execute argument is null.</exception>
        public CustomRelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">A parameter that can be used by the command.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        /// <summary>
        /// Executes the action associated with the command.
        /// </summary>
        /// <param name="parameter">A parameter that can be used by the command.</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
