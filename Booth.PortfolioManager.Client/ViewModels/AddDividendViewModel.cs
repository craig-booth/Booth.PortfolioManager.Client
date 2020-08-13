using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Booth.Common;
using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.CorporateActions;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class AddDividendViewModel : ViewModel, IPageViewModel, IEditableObject
    {
        private bool _BeingEdited;
        private RestClient _RestClient;

        private StockViewItem _Stock;
        public StockViewItem Stock
        {
            get
            {
                return _Stock;
            }
            set
            {
                if (_Stock != value)
                {
                    _Stock = value;

                    OnPropertyChanged();
                }

                ClearErrors();

                if (_Stock == null)
                    AddError("Company is required");
            }
        }

        private Date _PaymentDate;
        public Date PaymentDate
        {
            get
            {
                return _PaymentDate;
            }
            set
            {
                _PaymentDate = value;

                ClearErrors();

                if (_PaymentDate < RecordDate)
                    AddError("Payment Date must be after the Record date");
            }
        }

        public Date RecordDate { get; set; }
        public string Description { get; set; }

        private decimal _Amount;
        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;

                ClearErrors();

                if (_Amount <= 0.00m)
                    AddError("Amount must not be greater than 0");
            }
        }

        private decimal _PercentFranked;
        public decimal PercentFranked
        {
            get
            {
                return _PercentFranked;
            }
            set
            {
                _PercentFranked = value;

                ClearErrors();

                if (_PercentFranked < 0.00m)
                    AddError("Percent Franked must not be less than 0");
            }
        }

        private decimal _DRPPrice;
        public decimal DRPPrice
        {
            get
            {
                return _DRPPrice;
            }
            set
            {
                _DRPPrice = value;

                ClearErrors();

                if (_DRPPrice < 0.00m)
                    AddError("DRP Price must not be less than 0");
            }
        }

        public ObservableCollection<StockViewItem> AvailableStocks { get; private set; }

        public AddDividendViewModel(RestClient restClient)
        {
            _RestClient = restClient;

            AvailableStocks = new ObservableCollection<StockViewItem>();

            SaveDividendCommand = new RelayCommand(SaveTransaction, CanSaveTransaction);
        }

        public RelayCommand SaveDividendCommand { get; private set; }

        public string Label => "Add Dividend";

        public string Heading => "Add Dividend";

        public PageViewOptions Options => new PageViewOptions()
        {
            AllowStockSelection = false,
            DateSelection = DateSelectionType.None
        };

        private async void SaveTransaction()
        {         
            EndEdit();

            var dividend = new Dividend()
            {
                Id = Guid.NewGuid(),
                Stock = Stock.Id,
                ActionDate = RecordDate,
                Description = Description,
                PaymentDate = PaymentDate,
                Amount = Amount,
                PercentFranked = PercentFranked / 100,
                DrpPrice = DRPPrice
            };

            await _RestClient.CorporateActions.Add(Stock.Id, dividend);

            ClearFields();
            BeginEdit();
        }

        private bool CanSaveTransaction()
        {
            return !HasErrors;
        }

        public void Activate()
        {
            ClearFields();

            BeginEdit();
        }

        public void Deactivate()
        {
            EndEdit();
        }

        public void BeginEdit()
        {
            _BeingEdited = true;

            PopulateAvailableStocks();
        }

        public void EndEdit()
        {
            _BeingEdited = false;
        }

        public void CancelEdit()
        {
            _BeingEdited = false;
        }

        private async void PopulateAvailableStocks()
        {
            AvailableStocks.Clear();

            var stocks = await _RestClient.Stocks.Get();

            foreach (var stock in stocks.OrderBy(x => x.AsxCode))
            {
                var stockItem = new StockViewItem(stock.Id, stock.AsxCode, stock.Name);
                AvailableStocks.Add(stockItem);
                if (!stock.StapledSecurity)
                {
                    foreach (var childSecurity in stock.ChildSecurities)
                    {
                        stockItem = new StockViewItem(stock.Id, childSecurity.AsxCode, childSecurity.Name);
                        AvailableStocks.Add(stockItem);
                    }
                }
            }
        }

        private void ClearFields()
        {
            Stock = null;
            PaymentDate = Date.Today;
            RecordDate = Date.Today;
            Description = "";
            Amount = 0.00m;
            PercentFranked = 0.0m;
            DRPPrice = 0.00m;

            OnPropertyChanged("");
        }
    }
}
