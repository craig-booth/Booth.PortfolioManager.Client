using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Booth.Common;
using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class PortfolioSettingsViewModel : PortfolioViewModel
    {
        public Guid PortfolioId { get; set; }
        public string PortfolioName { get; set; }

        public Date PortfolioStartDate { get ; set;}
        public Date PortfolioEndDate { get; set; }

        public ObservableCollection<HoldingSettingsItem> HoldingSettings { get; set; } = new ObservableCollection<HoldingSettingsItem>();

        public PortfolioSettingsViewModel(string label, ViewParameter parameter)
            : base(label, parameter)
        {

        }

        public async override void RefreshView()
        {
            var response = await _Parameter.RestClient.Portfolio.GetProperties();
            if (response == null)
                return;

            PortfolioId = response.Id;
            PortfolioName = response.Name;
            PortfolioStartDate = response.StartDate;
            PortfolioEndDate = response.EndDate;

            HoldingSettings.Clear();
            foreach (var holding in response.Holdings.OrderBy(x => x.Stock.Name))
                HoldingSettings.Add(new HoldingSettingsItem(holding)
                {
                    ChangeDrpParticipationCommand = new RelayCommand<HoldingSettingsItem>(ChangeDrpParticipation)
                });

            OnPropertyChanged("");
        }

        private async void ChangeDrpParticipation(HoldingSettingsItem holding)
        {
            await _Parameter.RestClient.Holdings.ChangeDrpParticipation(holding.Stock.Id, holding.ParticipateInDrp);
        }
    }

    class HoldingSettingsItem
    {
        public StockViewItem Stock { get; set; }

        public Date StartDate { get; set; }
        public Date EndDate { get; set; }

        public bool ParticipateInDrp { get; set; }

        public RelayCommand<HoldingSettingsItem> ChangeDrpParticipationCommand { get; set; }

        public HoldingSettingsItem(HoldingProperties properties)
        {
            Stock = new StockViewItem(properties.Stock);
            StartDate = properties.StartDate;
            EndDate = properties.EndDate;
            ParticipateInDrp = properties.ParticipatingInDrp;
        }
    }
}
