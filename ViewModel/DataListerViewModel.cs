using GenAssess.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenAssess
{
    /// <summary>
    ///  DataEditorViewModel: The View Model for the Data Editor window.
    /// </summary>
    public class DataListerViewModel : BaseViewModel
    {
        /// <summary>
        ///  private DataLister dataLister: Window that Data Lister controls.
        /// </summary>
        private readonly DataLister dataLister;

        /// <summary>
        ///  private dynamic[] arrayToList: Array which will be listed.
        /// </summary>
        private readonly dynamic[] arrayToList;

        /// <summary>
        ///  public int ResizeBorder: The size of the resize border handles.
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        ///  public Thickness ResizeBorderThickness: The size of the resize border handles around the window.
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        ///  public int TitleHeight: The height of the title bar.
        /// </summary>
        public int TitleHeight { get; set; } = 29;

        /// <summary>
        ///  public GridLength TitleHeightGridLength: The height of the title bar.
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        /// <summary>
        ///  public ICommand MinimizeCommand: Command to minimize the window.
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        ///  public ICommand CloseCommand: Command to close the window.
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        ///  public ICommand MenuCommand: Command to open the system menu of the window.
        /// </summary>
        public ICommand MenuCommand { get; set; }

        /// <summary>
        ///  public DataListerViewModel: Constructor.
        /// </summary>
        /// <param name="window">Window that Data Lister controls.</param>
        public DataListerViewModel(DataLister dataLister, StackPanel stackPanel, TextBlock dataListerTitleTextBlock, Question[] arrayToList, bool remove = true)
        {
            this.dataLister = dataLister;
            this.arrayToList = arrayToList;

            MinimizeCommand = new RelayCommand(() => dataLister.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(() => dataLister.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(dataLister, dataLister.PointToScreen(Mouse.GetPosition(dataLister))));
            dataListerTitleTextBlock.Text = remove ? "Remove Questions" : "Edit Questions";
            for (int i = 0; i < arrayToList.Length; i++)
            {
                Question question = arrayToList[i];
                if (stackPanel != null)
                {
                    Button button = new Button();
                    button.Height = 20;
                    button.Style = dataLister.FindResource("DataEditorButton") as Style;
                    button.Width = stackPanel.Width;
                    button.Content = question.Text;
                    button.FontFamily = new FontFamily("Nirmala UI");
                    stackPanel.Children.Add(button);
                    if (remove)
                    {
                        button.Click += (s, e) => {
                            WindowViewModel.RemoveQuestion(question);
                            stackPanel.Children.Remove(button);
                        };
                    }
                    else
                    {
                        button.Click += (s, e) =>
                        {
                            WindowViewModel.dataEditor = new DataEditor(question);
                            WindowViewModel.dataEditor.Show();
                            WindowViewModel.dataEditor.Activate();
                        };
                    }
                }
            }
        }
    }
}
