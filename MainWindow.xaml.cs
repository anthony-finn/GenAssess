using Microsoft.Win32;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using GenAssess.Controls;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace GenAssess
{
    /// <summary>
    ///  MainWindow: Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WebBrowser pdfViewer = Template.FindName("PdfViewer", this) as WebBrowser;
            Loaded += MainWindow_Loaded;
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowser pdfViewer = Template.FindName("PdfViewer", this) as WebBrowser;
            this.DataContext = new WindowViewModel(this, pdfViewer);
        }
    }
}
