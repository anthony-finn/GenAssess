using System;
using System.Windows.Input;

namespace GenAssess
{
    /// <summary>
    ///  RelayCommand: A command that runs an Action
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        ///  private Action mAction: Action instance variable to run.
        /// </summary>
        private Action action;

        /// <summary>
        ///  public event EventHandler CanExecuteChanged: Event fired when the <see cref="CanExecute(object)"/> value has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        ///  public RelayCommand: Constructor.
        /// </summary>
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public RelayCommand()
        {
        }

        /// <summary>
        ///  public bool CanExecute: Relay command can always execute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///  public void ExecuteExecutes: The commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action();
        }
    }
}