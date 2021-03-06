﻿using System;

using Booth.WpfControls;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class SettingsViewModel : PageViewModel
    {
        public ApplicationSettings _Settings;

        public string PortfolioDatabasePath { get; set; }
        public string StockDatabasePath { get; set; }

        public SettingsViewModel(string label, ApplicationSettings settings)
            : base(label)
        {
            SaveSettingsCommand = new RelayCommand(SaveSettings);

            _Settings = settings;

            PortfolioDatabasePath = _Settings.PortfolioDatabase;
            StockDatabasePath = _Settings.StockDatabase;
        }

        public RelayCommand SaveSettingsCommand { get; private set; }
        private void SaveSettings()
        {
            bool databaseChanged = false;

            if ((PortfolioDatabasePath != _Settings.PortfolioDatabase)
                || (StockDatabasePath != _Settings.StockDatabase))
            {
                _Settings.PortfolioDatabase = PortfolioDatabasePath;
                _Settings.StockDatabase = StockDatabasePath;

                databaseChanged = true;
            }

            _Settings.Save();

            if (databaseChanged)
                _Settings.OnDatabaseChanged();
        }
    }
}
