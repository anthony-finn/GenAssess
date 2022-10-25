using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GenAssess.Controls
{
    /// <summary>
    /// Interaction logic for DataLister.xaml
    /// </summary>
    public partial class DataLister : Window
    {
        private Question[] array;
        private bool remove;
        public DataLister(Question[] array, bool remove)
        {
            this.array = array;
            this.remove = remove;
            InitializeComponent();
            StackPanel stackPanel = Template.FindName("DataListerItems", this) as StackPanel;
            TextBlock dataListerTitleTextBlock = Template.FindName("DataListerTitleTextBlock", this) as TextBlock;
            Loaded += DataLister_Loaded;
        }

        public void DataLister_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = Template.FindName("DataListerItems", this) as StackPanel;
            TextBlock dataListerTitleTextBlock = Template.FindName("DataListerTitleTextBlock", this) as TextBlock;
            this.DataContext = new DataListerViewModel(this, stackPanel, dataListerTitleTextBlock, array, remove);
        }
    }
}
