﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HtmlAgilityPack;

namespace Mago
{
    public class FindByViewModel : BaseViewModel
    {
        private string _mangaName;
        private string _mangaURL;
        private string _searchBox;
        private ObservableCollection<string> _suitableMangaSources;
        private int _selectedIndex = 0;
        private Visibility _warningIconVisibility = Visibility.Hidden;
        private bool _URLIsIndeternimate;
        private bool _URLButtonEnabled = true;
        private bool _NameIsIndeternimate;
        private bool _progressEnabled = false;

        private MainViewModel mainView;
        private HtmlPageLoader PageLoader;

        public ICommand FindByName { get; set; }
        public ICommand FindByURL { get; set; }
        public ICommand FindCommand { get; set; }

        public FindByViewModel(MainViewModel mainViewModel, HtmlPageLoader pageLoader)
        {
            _suitableMangaSources = new ObservableCollection<string> { "Mangakakalot.com" };

            //FindByName = new RelayCommand(() => Task.Run(SearchWithName));
            //FindByURL = new RelayCommand(() => Task.Run(SearchWithURL));
            FindCommand = new RelayCommand(Find);

            mainView = mainViewModel;

        }

        public void Find()
        {
            if (_searchBox == string.Empty) return;
            ProgressEnabled = true;
            if (_searchBox.StartsWith("https://"))
                Task.Run(() => SearchWithURL(_searchBox));
            else
                Task.Run(() => SearchWithName(_searchBox));
        }

        async Task SearchWithName(string MangaName)
        {
            if (MangaName == null)
                return;
            string url = ComposeURL(MangaName);
            bool isWebsiteValid = await RemoteFileExists(url);
            if (!isWebsiteValid) { NameIsIndeterminate = false; WarningIconVisibility = Visibility.Visible; return; }
            WarningIconVisibility = Visibility.Hidden;
            string n_url = url;
            OpenManga(n_url);
        }

        async Task SearchWithURL(string MangaURL)
        {
            if (MangaURL == null)
                return;
            bool isWebsiteValid = await RemoteFileExists(MangaURL);
            if (!isWebsiteValid) { URLIsIndeterminate = false; WarningIconVisibility = Visibility.Visible; return; }
            WarningIconVisibility = Visibility.Hidden;
            string n_url = MangaURL;
            OpenManga(n_url);
        }

        private async Task OpenManga(string url)
        {
            //wait will page is loaded
            await mainView.HtmlPageLoader.LoadData(url);

            //apply the data
            await mainView.HtmlPageLoader.ApplyData();

            //clear button loading
            ProgressEnabled = false;

            //Clear search box text
            SearchBox = string.Empty;

            //Set transitional index to Open Manga viewer
            mainView.MenuViewModel.TransitionIndex = mainView.MenuViewModel.page.MangaInfoView;
        }

        async Task<bool> RemoteFileExists(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlNodeCollection attributes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'chapter-list')]");
                if (attributes != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private string ComposeURL(string name)
        {
            name = name.Replace(' ', '_').ToLower();
            return "https://" + SuitableMangaSources[SelectedIndex].ToLower() + (SuitableMangaSources[SelectedIndex] == "Mangakakalot.com" ? "/manga" : "") + "/" + name;
        }

        public ObservableCollection<string> SuitableMangaSources => _suitableMangaSources;

        public string MangaName
        {
            get { return _mangaName; }
            set
            {
                if (_mangaName == value) return;
                _mangaName = value;
            }
        }
        public string MangaURL
        {
            get { return _mangaURL; }
            set
            {
                if (_mangaURL == value) return;
                _mangaURL = value;
            }
        }
        public string SearchBox
        {
            get { return _searchBox; }
            set
            {
                if (_searchBox == value) return;
                _searchBox = value;
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
            }
        }
        public Visibility WarningIconVisibility
        {
            get { return _warningIconVisibility; }
            set
            {
                if (_warningIconVisibility == value) return;
                _warningIconVisibility = value;
            }
        }
        public bool URLIsIndeterminate
        {
            get { return _URLIsIndeternimate; }
            set
            {
                if (_URLIsIndeternimate == value) return;
                _URLIsIndeternimate = value;
                URLButtonEnabled = !_URLIsIndeternimate;
            }
        }
        public bool NameIsIndeterminate
        {
            get { return _NameIsIndeternimate; }
            set
            {
                if (_NameIsIndeternimate == value) return;
                _NameIsIndeternimate = value;

            }
        }
        public bool URLButtonEnabled
        {
            get { return _URLButtonEnabled; }
            set
            {
                if (_URLButtonEnabled == value) return;
                _URLButtonEnabled = value;
            }
        }
        public bool ProgressEnabled
        {
            get { return _progressEnabled; }
            set
            {
                if (_progressEnabled == value) return;
                _progressEnabled = value;
            }
        }
    }
}
