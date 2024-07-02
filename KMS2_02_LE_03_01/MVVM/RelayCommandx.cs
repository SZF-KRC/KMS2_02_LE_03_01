using System;
using System.Windows.Input;

namespace KMS2_02_LE_03_01.MVVM
{
    /// <summary>
    /// Eine Implementierung von ICommand, die zum Binden von Befehlen an die Benutzeroberfläche verwendet wird.
    /// </summary>
    public class CustomRelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initialisiert eine neue Instanz der CustomRelayCommand-Klasse.
        /// </summary>
        /// <param name="execute">Die Aktion, die beim Ausführen des Befehls ausgeführt wird.</param>
        /// <param name="canExecute">Die Funktion, die bestimmt, ob der Befehl ausgeführt werden kann.</param>
        /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn das execute-Argument null ist.</exception>
        public CustomRelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Überprüft, ob der Befehl ausgeführt werden kann.
        /// </summary>
        /// <param name="parameter">Ein Parameter, der bei der Ausführung des Befehls verwendet werden kann.</param>
        /// <returns>True, wenn der Befehl ausgeführt werden kann; andernfalls false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Führt die mit dem Befehl verknüpfte Aktion aus.
        /// </summary>
        /// <param name="parameter">Ein Parameter, der bei der Ausführung des Befehls verwendet werden kann.</param>
        public void Execute(object parameter)
        {
            _execute();
        }

        /// <summary>
        /// Tritt auf, wenn sich die Ausführungsfähigkeit des Befehls ändert.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class CustomRelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public CustomRelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

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
