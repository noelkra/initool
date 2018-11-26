﻿using System.Windows;

namespace iniTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly FileHandler _fileHandler;
        private readonly DialogHandler _dialogHandler;
        private readonly ResourceEdit _resEdit;
        private WaitingDialog _waitingDialog;
        private EntitySelector _entitySelector;
        private EntityFixer _entityFixer;

        public MainWindow()
        {
            _fileHandler = new FileHandler();
            _dialogHandler = new DialogHandler();
            _resEdit = new ResourceEdit();
            InitializeComponent();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            GridMainData.Visibility = Visibility.Hidden;
            UcSettings.Visibility = Visibility.Hidden;
            UcAbout.Visibility = Visibility.Visible;
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            GridMainData.Visibility = Visibility.Hidden;
            UcSettings.Visibility = Visibility.Visible;
            UcAbout.Visibility = Visibility.Hidden;
        }
        private void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            GridMainData.Visibility = Visibility.Visible;
            UcSettings.Visibility = Visibility.Hidden;
            UcAbout.Visibility = Visibility.Hidden;
        }
        private void BtnOpenNewWorkspace_Click(object sender, RoutedEventArgs e)
        {
            //If folder chosen successfully, save to ini-file
            var dir = _dialogHandler.OpenFolderDialog();
            _resEdit.SetWorkspace(dir);
            OpenFiles(dir);
        }
        private void Window_Loaded(object sender, System.EventArgs e)
        {
            if (!_resEdit.GetPreferencesStatus())
            {
                _resEdit.SetDefaultPreferences();
            }
        }
        private void btnConfirmAction_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This operation cannot be undone. Are you sure you want to continue?", "Confirm action", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;
            //Repair the Files and reload them afterwards.
            _fileHandler.RepairFiles();
            OpenFiles(_resEdit.GetWorkspace());
        }
        public void OpenFiles(string dir)
        {
            _waitingDialog = null;
            if (string.IsNullOrEmpty(dir)) return;
            _waitingDialog = new WaitingDialog();
            SetLoading(true);
            _fileHandler.LoadFileContent(dir);
            DgListFileContent.ItemsSource = null;
            DgListFileContent.ItemsSource = _fileHandler.GetFileContent();
            SetLoading(false);
        }
        private void SetLoading(bool loading)
        {
            if (loading)
            {
                _waitingDialog.Show();
            }
            else
            {
                _waitingDialog.Close();
            }
        }
    }
}
