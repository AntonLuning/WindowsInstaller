using Microsoft.Win32;
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

namespace MyInstaller.GUI
{
    public partial class MainWindow : Window
    {
        private string? _projectPath;
        private readonly List<string> ImageExtensions = new() { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".ICO" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save the project before closing?", "Save Project", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                if (!SaveProject())
                    e.Cancel = true;
        }

        private void SetupIconPathTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(SetupIconPathTB.Text) && ImageExtensions.Contains(Path.GetExtension(SetupIconPathTB.Text).ToUpperInvariant()))
                SetupIconImg.Source = new BitmapImage(new Uri(SetupIconPathTB.Text));
            else
                SetupIconImg.Source = null;
        }

        private void SaveProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }

        private void SaveAsProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveAsProject();
        }

        private void OpenProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenProject();
        }

        private void BuildProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            BuildProject();
        }

        private bool SaveProject()
        {
            if (string.IsNullOrEmpty(_projectPath) || !File.Exists(_projectPath))
                return SaveAsProject();

            MyProject.Save(GetGUIData(), _projectPath);

            return true;
        }

        private bool SaveAsProject()
        {
            SaveFileDialog sfDialog = new();
            sfDialog.Title = "Save Project";
            sfDialog.Filter = "Installer Project (*.wi)|*.wi";
            if (string.IsNullOrEmpty(Properties.Settings.Default.RootProjectPath) || !Directory.Exists(Properties.Settings.Default.RootProjectPath))
                sfDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                sfDialog.InitialDirectory = Properties.Settings.Default.RootProjectPath;

            if (sfDialog.ShowDialog() != true)
                return false;

            _projectPath = sfDialog.FileName;
            Properties.Settings.Default.RootProjectPath = Path.GetDirectoryName(_projectPath);
            Properties.Settings.Default.Save();

            this.Title = $"Windows Installer Builder | {Path.GetFileNameWithoutExtension(_projectPath)}";
            MyProject.Save(GetGUIData(), _projectPath);

            return true;
        }

        private bool OpenProject()
        {
            if (MessageBox.Show("Do you want to save the current project?", "Save Project", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                SaveProject();

            OpenFileDialog ofDialog = new();
            ofDialog.Title = "Open Project";
            ofDialog.Filter = "Installer Project (*.wi)|*.wi";
            if (string.IsNullOrEmpty(Properties.Settings.Default.RootProjectPath) || !Directory.Exists(Properties.Settings.Default.RootProjectPath))
                ofDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                ofDialog.InitialDirectory = Properties.Settings.Default.RootProjectPath;

            if (ofDialog.ShowDialog() != true)
                return false;

            _projectPath = ofDialog.FileName;
            Properties.Settings.Default.RootProjectPath = Path.GetDirectoryName(_projectPath);
            Properties.Settings.Default.Save();

            this.Title = $"Windows Installer Builder | {Path.GetFileNameWithoutExtension(_projectPath)}";
            SetGUIData(MyProject.Open(_projectPath));

            return true;
        }

        private void BuildProject()
        {
            if (!SaveProject())
                return;

            if (BuildSetup.GenerateFile())
                MessageBox.Show("Setup file generated");
            else
                MessageBox.Show("Failed to generate setup file");
        }

        private Dictionary<string, string> GetGUIData()
        {
            Dictionary<string, string> result = new();

            result.Add("AppNameTB", AppNameTB.Text);
            result.Add("CompanyNameTB", CompanyNameTB.Text);
            result.Add("AppVersionTB", AppVersionTB.Text);
            result.Add("SetupIconPathTB", SetupIconPathTB.Text);

            return result;
        }

        private void SetGUIData(Dictionary<string, string> GUIData)
        {
            AppNameTB.Text = GUIData.ContainsKey("AppNameTB") ? GUIData["AppNameTB"] : string.Empty;
            CompanyNameTB.Text = GUIData.ContainsKey("CompanyNameTB") ? GUIData["CompanyNameTB"] : string.Empty;
            AppVersionTB.Text = GUIData.ContainsKey("AppVersionTB") ? GUIData["AppVersionTB"] : string.Empty;
            SetupIconPathTB.Text = GUIData.ContainsKey("SetupIconPathTB") ? GUIData["SetupIconPathTB"] : string.Empty;
        }






        //    TextBox element = (TextBox)this.FindName(elementName);
    }
}
