﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mago
{
    public class MenuViewModel : BaseViewModel
    {
        private bool _isMenuOpen = false;
        private bool _isMaximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        private bool _darkThemeEnabled;

        private int _transitionIndex = -1;

        private readonly Uri darkTheme = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
        private readonly Uri lightTheme = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml");
        private readonly Page page = new Page();

        #region Commands

        public ICommand MinimizeCommand { get; set; }
        public ICommand ShutdownCommand { get; set; }

        public ICommand Recents { get; set; }
        public ICommand Home { get; set; }
        public ICommand Settings { get; set; }
        public ICommand Browser { get; set; }
        public ICommand Downloads { get; set; }
        public ICommand Favourites { get; set; }
        public ICommand Name { get; set; }
        public ICommand URL { get; set; }

        #endregion

        public MenuViewModel()
        {
            _darkThemeEnabled = Application.Current.Resources.MergedDictionaries[0].Source == darkTheme;

            MinimizeCommand = new RelayCommand(Minimize);
            ShutdownCommand = new RelayCommand(Shutdown);

            Recents = new RelayCommand(OpenRecents);
            Home = new RelayCommand(OpenHome);
            Settings = new RelayCommand(OpenSettings);
            Browser = new RelayCommand(OpenBrowser);
            Downloads = new RelayCommand(OpenDownloads);
            Favourites = new RelayCommand(OpenFavourites);
            Name = new RelayCommand(OpenByName);
            URL = new RelayCommand(OpenByURL);
        }

        private void OpenRecents()
        {
            TransitionIndex = page.Recents;
        }

        private void OpenHome()
        {
            TransitionIndex = page.Home;
        }

        private void OpenSettings()
        {
            TransitionIndex = page.Settings;
        }

        private void OpenBrowser()
        {
            TransitionIndex = page.Browser;
        }

        private void OpenDownloads()
        {
            TransitionIndex = page.Downloads;
        }

        private void OpenFavourites()
        {
            TransitionIndex = page.Favourites;
        }

        private void OpenByName()
        {
            TransitionIndex = page.ByName;
        }

        private void OpenByURL()
        {
            TransitionIndex = page.ByURL;
        }

        private void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void changeDark()
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
        }

        private void Shutdown()
        {
            Application.Current.Shutdown();
        }

        public bool IsMenuOpen
        {
            get { return _isMenuOpen; }
            set
            {
                if (_isMenuOpen == value) return;
                _isMenuOpen = value;
            }
        }

        public bool IsMaximized
        {
            get { return _isMaximized; }
            set
            {
                if (_isMaximized == value) return;
                _isMaximized = value;
                Application.Current.MainWindow.WindowState = _isMaximized ? WindowState.Maximized : WindowState.Normal;

            }
        }

        public bool DarkThemeEnabled
        {
            get { return _darkThemeEnabled; }
            set
            {
                if (_darkThemeEnabled == value) return;
                _darkThemeEnabled = value;
                Application.Current.Resources.MergedDictionaries[0].Source = _darkThemeEnabled ? darkTheme : lightTheme;

            }
        }

        public int TransitionIndex
        {
            get { return _transitionIndex; }
            set
            {
                if (_transitionIndex == value) return;
                _transitionIndex = value;

            }
        }
    }
}
