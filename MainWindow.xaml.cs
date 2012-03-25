using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using QuickGraph;
using GraphSharp.Controls;

namespace sociogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel vm = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void TextBlock_Drop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            foreach (string filename in filenames)
                GraphText.Text += File.ReadAllText(filename);
            e.Handled = true; 
        }

        private bool CanDrop(DragEventArgs e)
        {
            return e.Data.GetDataPresent(DataFormats.FileDrop, true);
        }

        private void TextBlock_DragEnter(object sender, DragEventArgs e)
        {
            if (CanDrop(e)) {
                e.Effects = DragDropEffects.All;
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void GraphText_TextChanged(object sender, TextChangedEventArgs e) {
            vm.ReadGraph(new StringReader(GraphText.Text));
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            graphLayout.Relayout();
        }
    }
}
