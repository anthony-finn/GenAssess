using GenAssess.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenAssess
{
    /// <summary>
    ///  DataEditorViewModel: The View Model for the Data Editor window.
    /// </summary>
    public class DataEditorViewModel : BaseViewModel
    {
        /// <summary>
        ///  private DataEditor dataEditor: Window that Data Editor controls.
        /// </summary>
        private readonly DataEditor dataEditor;

        public static Question question;

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
        ///  public ICommand AddImageCommand: Command to add an image.
        /// </summary>
        public ICommand AddImageCommand { get; set; }

        /// <summary>
        ///  public ICommand AddChoiceCommand: Command to add an answer choice.
        /// </summary>
        public ICommand AddChoiceCommand { get; set; }

        /// <summary>
        ///  public ICommand AddQuestionCommand: Command to add a question.
        /// </summary>
        public ICommand AddQuestionCommand { get; set; }

        /// <summary>
        ///  public DataEditorViewModel: Constructor.
        /// </summary>
        /// <param name="dataEditor">Window that Data Editor controls.</param>
        public DataEditorViewModel(DataEditor dataEditor, Question load, bool forced = false)
        {
            this.dataEditor = dataEditor;
            question = load != null ? load : new Question();
            if (forced)
            {
                dataEditor.Loaded += (s, e) =>
                {
                    DataEditor_Loaded(load);
                };
            }

            MinimizeCommand = new RelayCommand(() => dataEditor.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(() => dataEditor.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(dataEditor, dataEditor.PointToScreen(Mouse.GetPosition(dataEditor))));
            AddImageCommand = new RelayCommand(() => AddImage());
            AddChoiceCommand = new RelayCommand(() => AddChoice());
            AddQuestionCommand = new RelayCommand(() => AddQuestion());
        }

        public void DataEditor_Loaded(Question load)
        {
            if (dataEditor.Template.FindName("QuestionTextBox", dataEditor) is TextBox questionTextBox)
            {
                questionTextBox.Text = load.Text;
                foreach (string choice in load.Choices)
                {
                    _AddChoice(choice, false);
                }
                foreach (string answer in load.Answers)
                {
                    _AddChoice(answer, true);
                }
            }
        }

        private void _AddImage(string filePath, string fileName)
        {
            if (dataEditor.Template.FindName("AddImageLabel", dataEditor) is Label addImageLabel)
            {
                addImageLabel.Content = fileName;
                question.ImagePath = filePath;
            }
        }

        public void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files(*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp",
                RestoreDirectory = true,
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                _AddImage(filePath, fileName);
            }
        }

        private void _AddChoice(string text, bool isAnswer)
        {
            if (dataEditor.Template.FindName("AddChoiceTextbox", dataEditor) is TextBox addChoiceTextbox && dataEditor.Template.FindName("AddChoiceCheckBox", dataEditor) is CheckBox addChoiceCheckBox && dataEditor.Template.FindName("ChoiceStackPanel", dataEditor) is StackPanel choiceStackPanel)
            {
                Grid grid = new Grid()
                {
                    Background = Application.Current.TryFindResource("BackgroundDarkBrush") as SolidColorBrush,
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsChecked = isAnswer
                };

                TextBlock choiceTextbox = new TextBlock()
                {
                    Text = text,
                    Foreground = Application.Current.TryFindResource("ForegroundVeryLightBrush") as SolidColorBrush,
                };

                Button removeChoiceButton = new Button()
                {
                    Width = choiceTextbox.LineHeight,
                    Style = Application.Current.TryFindResource("WindowCloseButton") as Style,
                    Content = "✕",
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(choiceStackPanel.ActualWidth - 40) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                grid.Children.Add(checkBox);
                grid.Children.Add(choiceTextbox);
                grid.Children.Add(removeChoiceButton);

                Grid.SetColumn(choiceTextbox, 1);
                Grid.SetColumn(removeChoiceButton, 2);

                int index = choiceStackPanel.Children.Add(grid);

                void RemoveChoiceButton_Click(object sender, RoutedEventArgs e)
                {
                    choiceStackPanel.Children.Remove(grid);
                }

                void CheckBox_Checked(object sender, RoutedEventArgs e)
                {
                    isAnswer = (bool)checkBox.IsChecked;
                }

                checkBox.Checked += CheckBox_Checked;
                removeChoiceButton.Click += RemoveChoiceButton_Click;
                addChoiceTextbox.Text = string.Empty;
                addChoiceCheckBox.IsChecked = false;
            }
        }

        public void AddChoice()
        {
            if (dataEditor.Template.FindName("AddChoiceTextbox", dataEditor) is TextBox addChoiceTextbox && dataEditor.Template.FindName("AddChoiceCheckBox", dataEditor) is CheckBox addChoiceCheckBox && dataEditor.Template.FindName("ChoiceStackPanel", dataEditor) is StackPanel choiceStackPanel)
            {
                string text = addChoiceTextbox.Text;
                bool isAnswer = (bool)addChoiceCheckBox.IsChecked;
                _AddChoice(text, isAnswer);
            }
        }

        public void AddQuestion()
        {
            if (dataEditor.Template.FindName("ChoiceStackPanel", dataEditor) is StackPanel choiceStackPanel && dataEditor.Template.FindName("QuestionTextBox", dataEditor) is TextBox questionTextBox)
            {
                List<string> answers = new List<string>();
                List<string> choices = new List<string>();
                for (int i = 0; i < choiceStackPanel.Children.Count; i++)
                {
                    Grid choiceGrid = (Grid)choiceStackPanel.Children[i];
                    bool isAnswer = (bool)((CheckBox)choiceGrid.Children[0]).IsChecked;
                    string choiceText = (string)((TextBlock)choiceGrid.Children[1]).Text;
                    if (isAnswer)
                    {
                        answers.Add(choiceText);
                    }
                    else
                    {
                        choices.Add(choiceText);
                    }
                }
                question.Answers = answers.ToArray();
                question.Choices = choices.ToArray();
                question.Text = questionTextBox.Text;
                WindowViewModel.AddQuestion(question);
                dataEditor.Close();
            }
        }
    }
}
