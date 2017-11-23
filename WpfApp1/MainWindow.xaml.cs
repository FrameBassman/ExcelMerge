using System.Windows;

namespace WpfApp1
{
    using Microsoft.Win32;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFisrtFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                this.FirstFilePath.Text = openFileDialog.FileName;
        }

        private void SelectSecondFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                this.SecondFilePath.Text = openFileDialog.FileName;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            ExcelMerge.Program coreProgram = new ExcelMerge.Program();
            ExcelMerge.MergeSettings settings = new ExcelMerge.MergeSettings(
                                                                this.FirstFilePath.Text, 
                                                                this.SecondFilePath.Text, 
                                                                this.FirstFilePathResult.Text, 
                                                                this.SecondFilePathResult.Text);

            int matches = coreProgram.Execute(settings);

            this.FirstFilePathResult.Text = this.FirstFilePath.Text;
            this.SecondFilePathResult.Text = this.SecondFilePath.Text;
            this.ResultLabel.Content = "Количество совпадений: " + matches;
        }
    }
}
