using GenAssess.Controls;
using Microsoft.Win32;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenAssess
{
    /// <summary>
    ///  WindowViewModel: The View Model for the custom window.
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        /// <summary>
        ///  private MainWindow window: Window that View Model controls.
        /// </summary>
        private static MainWindow mainWindow;

        /// <summary>
        ///  private DataEditor dataEditor: Window that Data Editor controls.
        /// </summary>
        public static DataEditor dataEditor;

        /// <summary>
        ///  private DataLister dataLister: Window that Data Lister controls.
        /// </summary>
        private DataLister dataLister;

        /// <summary>
        ///  private PdfViewer pdfViewer: PdfViewer of the Window.
        /// </summary>
        private static WebBrowser pdfViewer;

        /// <summary>
        ///  public static Test test: Test which is currently being edited.
        /// </summary>
        public static Test test;

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
        ///  public ICommand MaximizeCommand: Command to maximize the window.
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        ///  public ICommand CloseCommand: Command to close the window.
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        ///  public ICommand MenuCommand: Command to open the system menu of the window.
        /// </summary>
        public ICommand MenuCommand { get; set; }

        /// <summary>
        ///  public ICommand RenderCommand: Command render the current Test object to the PdfViewer control.
        /// </summary>
        public ICommand RenderCommand { get; set; }

        /// <summary>
        ///  public ICommand ShuffleCommand: Command shuffle the current Test object.
        /// </summary>
        public ICommand ShuffleCommand { get; set; }

        /// <summary>
        ///  public ICommand NewCommand: Command to create a new GenAssess Test.
        /// </summary>
        public ICommand NewCommand { get; set; }

        /// <summary>
        ///  public ICommand OpenCommand: Command to open an existing GenAssess Test or Pdf.
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        ///  public ICommand SaveCommand: Command to save a GenAssess Test.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        ///  public ICommand PrintCommand: Command to print a GenAssess Test.
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        ///  public ICommand AddCommand: Command to add a question to a Test.
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        ///  public ICommand RemoveCommand: Command to remove a question from a Test.
        /// </summary>
        public ICommand RemoveCommand { get; set; }

        /// <summary>
        ///  public ICommand EditCommand: Command to edit a question from a Test.
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        ///  public WindowViewModel: Constructor.
        /// </summary>
        /// <param name="window">Window that View Model controls.</param>
        /// <param name="pdfViewer">PdfViewer control</param>
        public WindowViewModel(MainWindow window, WebBrowser pdfViewerControl)
        {
            // Set Instance and Static variables
            mainWindow = window;
            pdfViewer = pdfViewerControl;
            test = new Test();

            // Add Event Handlers
            window.StateChanged += new EventHandler(Window_StateChanged);
            window.Closed += new EventHandler(Window_Closed);

            // Add RelayCommands
            MinimizeCommand = new RelayCommand(() => window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => window.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(window, window.PointToScreen(Mouse.GetPosition(window))));
            RenderCommand = new RelayCommand(() => Render());
            ShuffleCommand = new RelayCommand(() => Shuffle());
            NewCommand = new RelayCommand(() => New());
            OpenCommand = new RelayCommand(() => Open());
            SaveCommand = new RelayCommand(() => Save());
            PrintCommand = new RelayCommand(() => Print());
            AddCommand = new RelayCommand(() => Add());
            RemoveCommand = new RelayCommand(() => Remove());
            EditCommand = new RelayCommand(() => Edit());
        }

        /// <summary>
        ///  public void Window_StateChanged: Event method for StateChanged.
        /// </summary>
        /// <param name="sender">Sender object for event.</param>
        /// <param name="e">StateChanged event data.</param>
        public void Window_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ResizeBorderThickness));

            if (mainWindow.Template.FindName("MaximizeButton", mainWindow) is Button maximizeButton && mainWindow.Template.FindName("ControlArea", mainWindow) is Border controlArea)
            {
                bool isMaximized = mainWindow.WindowState == WindowState.Maximized;
                maximizeButton.Content = isMaximized ? "\xE923" : "\xE739";

                // Adjust window size as a maximized window is larger than the screen resolution.
                controlArea.Padding = new Thickness(isMaximized ? 8 : 0);

                maximizeButton.ToolTip = isMaximized ? "Restore Down" : "Maximize";
            }
        }

        /// <summary>
        ///  public void Window_Closed: Event method for Closed.
        /// </summary>
        /// <param name="sender">Sender object for event.</param>
        /// <param name="e">StateChanged event data.</param>
        public void Window_Closed(object sender, EventArgs e)
        {
            if (Application.Current.Windows.Cast<Window>().Any(x => (x == dataEditor)))
            {
                dataEditor.Close();
            }
        }

        public void New()
        {

        }


        public void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Pdf files (*.pdf)|*.pdf|GenAsses Data (*.GAD)|*.GAD|All files (*.*)|*.*",
                RestoreDirectory = true,
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                PrepareWebBrowser();
                Test.RenderPdf(mainWindow, pdfViewer, filePath);
            }
        }

        public static void PrepareWebBrowser()
        {
            if (mainWindow.Template.FindName("WebBrowserContainer", mainWindow) is Viewbox viewbox)
            {
                pdfViewer.Dispose();
                WebBrowser webBrowser = new WebBrowser() { Name = "PdfViewer", Margin = new Thickness(10), Height = 0, Width = 0, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                pdfViewer = webBrowser;
                viewbox.Child = webBrowser;
            }
        }

        public void Save()
        {
        }

        public void Render()
        {
            test.DrawTest();
            test.RenderPdf(mainWindow, pdfViewer);
        }

        public void Shuffle()
        {
            test.Shuffle();
            Render();
        }

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ProcessStartInfo printProcessInfo = new ProcessStartInfo()
                {
                    Verb = "print",
                    CreateNoWindow = true,
                    FileName = test.FileName,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process printProcess = new Process();
                printProcess.StartInfo = printProcessInfo;
                printProcess.Start();

                printProcess.WaitForInputIdle();

                if (printProcess.CloseMainWindow() == false)
                {
                    printProcess.Kill();
                }
            }
        }

        public static void Add(Question question = null)
        {
            if (!Application.Current.Windows.Cast<Window>().Any(x => (x == dataEditor)))
            {
                dataEditor = new DataEditor(question);
            }
            dataEditor.Show();
            dataEditor.Activate();
        }

        public static void AddQuestion(Question question)
        {
            test.AddQuestion(question);
            test.DrawTest();
            test.RenderPdf(mainWindow, pdfViewer);
        }

        public static void RemoveQuestion(Question question)
        {
            test.RemoveQuestion(question);
            test.DrawTest();
            test.RenderPdf(mainWindow, pdfViewer);
        }

        public void Remove()
        {
            if (!Application.Current.Windows.Cast<Window>().Any(x => (x == dataLister)))
            {
                this.dataLister = new DataLister(test.Questions.ToArray(), true);
            }
            this.dataLister.Show();
            this.dataLister.Activate();
        }

        public void Edit()
        {
            if (!Application.Current.Windows.Cast<Window>().Any(x => (x == dataLister)))
            {
                this.dataLister = new DataLister(test.Questions.ToArray(), false);
            }
            this.dataLister.Show();
            this.dataLister.Activate();
        }
    }
}
