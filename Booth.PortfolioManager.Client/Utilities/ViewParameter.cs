using System;

using Booth.Common;
using Booth.WpfControls;
using Booth.PortfolioManager.Client.ViewModels;
using Booth.PortfolioManager.RestApi.Client;

namespace Booth.PortfolioManager.Client.Utilities
{
    class ViewParameter : NotifyClass
    {
        public RestClient RestClient;

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
            }
        }

        private DateTime _SelectedDate;
        public DateTime SeletedDate
        {
            get
            {
                return _SelectedDate;
            }

            set
            {
                if (_SelectedDate != value)
                {
                    _SelectedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateRange _DateRange;
        public DateRange DateRange
        {
            get
            {
                return _DateRange;
            }
        }

        public DateTime FromDate
        {
            get
            {
                return _DateRange.FromDate.DateTime;
            }

            set
            {
                if (_DateRange.FromDate.DateTime != value)
                {
                    _DateRange = new DateRange(new Date(value), _DateRange.ToDate);
                    OnPropertyChanged();
                }
            }
        }

        public DateTime ToDate
        {
            get
            {
                return _DateRange.ToDate.DateTime;
            }

            set
            {
                if (_DateRange.ToDate.DateTime != value)
                {
                    _DateRange = new DateRange(_DateRange.FromDate, new Date(value));
                    OnPropertyChanged();
                }
            }
        }


        private int _FinancialYear;
        public int FinancialYear
        {
            get
            {
                return _FinancialYear;
            }

            set
            {
                if (_FinancialYear != value)
                {
                    _FinancialYear = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewParameter()
        {
            _SelectedDate = DateTime.Today;
            _DateRange = new DateRange(Date.Today.AddYears(-1).AddDays(1), Date.Today);
            _FinancialYear = Date.Today.FinancialYear();
        }

    }
}
