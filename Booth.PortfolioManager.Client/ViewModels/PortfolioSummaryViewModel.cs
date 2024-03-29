﻿using System;
using System.Linq;
using System.Collections.ObjectModel;

using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.Client.Utilities;
using Booth.Common;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class PortfolioSummaryViewModel : PortfolioViewModel
    {
        public ChangeInValue PortfolioValue { get; private set; }
        public PortfolioReturn Return1Year { get; private set; }
        public PortfolioReturn Return3Year { get; private set; }
        public PortfolioReturn Return5Year { get; private set; }
        public PortfolioReturn ReturnAll { get; private set; }

        public ObservableCollection<HoldingItemViewModel> Holdings { get; private set; }

        public PortfolioSummaryViewModel(string label, ViewParameter parameter)
            : base(label, parameter)
        {
            Holdings = new ObservableCollection<HoldingItemViewModel>();

            PortfolioValue = new ChangeInValue();
            Return1Year = new PortfolioReturn("1 Year");
            Return3Year = new PortfolioReturn("3 Years");
            Return5Year = new PortfolioReturn("5 Years");
            ReturnAll = new PortfolioReturn("All"); 
        }

        public async override void RefreshView()
        {
            PortfolioSummaryResponse response;
            try
            {
                response = await _Parameter.RestClient.Portfolio.GetSummary(_Parameter.Date);
            }
            catch (Exception e)
            {
                response = null;
            }

            if (response == null)
                return;

            PortfolioValue.InitialValue = response.PortfolioCost;
            PortfolioValue.Value = response.PortfolioValue;

            if (response.Return1Year != null)
            {
                Return1Year.NotApplicable = false;
                Return1Year.Value = (decimal)response.Return1Year;
            } 
            else
            {
                Return1Year.NotApplicable = true;
                Return1Year.Value = 0.00m;
            }

            if (response.Return3Year != null)
            {
                Return3Year.NotApplicable = false;
                Return3Year.Value = (decimal)response.Return3Year;
            }
            else
            {
                Return3Year.NotApplicable = true;
                Return3Year.Value = 0.00m;
            }

            if (response.Return5Year != null)
            {
                Return5Year.NotApplicable = false;
                Return5Year.Value = (decimal)response.Return5Year;
            }
            else
            {
                Return5Year.NotApplicable = true;
                Return5Year.Value = 0.00m;
            }

            if (response.ReturnAll != null)
            {
                ReturnAll.NotApplicable = false;
                ReturnAll.Value = (decimal)response.ReturnAll;
            }
            else
            {
                ReturnAll.NotApplicable = true;
                ReturnAll.Value = 0.00m;
            }

            Holdings.Clear();
            foreach (var holding in response.Holdings.OrderBy(x => x.Stock.Name))
                Holdings.Add(new HoldingItemViewModel(holding));


            Holdings.Add(new HoldingItemViewModel("Cash Account", 0, response.CashBalance, response.CashBalance));

            OnPropertyChanged("");
        }

    }

    class HoldingItemViewModel
    {
        public StockViewItem Stock { get; private set; }
        public int Units { get; private set; }
        public ChangeInValue ChangeInValue { get; private set; }

        public HoldingItemViewModel(string companyName, int units, decimal cost, decimal marketValue)
        {
            Stock = new StockViewItem(Guid.Empty, "", companyName);
            Units = units;
            ChangeInValue = new ChangeInValue(cost, marketValue);
        }

        public HoldingItemViewModel(Holding holding)
        {
            Stock = new StockViewItem(holding.Stock);
            Units = holding.Units;
            ChangeInValue = new ChangeInValue(holding.Cost, holding.Value);
        }

    }

    enum DirectionChange { Increase, Decrease, Neutral };

    class ChangeInValue : NotifyClass
    {
        private decimal _InitialValue;
        public decimal InitialValue
        {
            get
            {
                return _InitialValue;
            }
            set
            {
                if (value != _InitialValue)
                {
                    _InitialValue = value;
                    OnPropertyChanged("");
                }
            }
        }

        private decimal _Value;
        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                    OnPropertyChanged("");
                }
            }
        }

        public decimal Change
        {
            get
            {
                return (Value - InitialValue);
            }
        }

        public decimal PercentageChange
        {
            get
            {
                if (InitialValue == 0)
                    return 0;
                else
                    return Change / InitialValue;
            }
        }

        public DirectionChange Direction
        {
            get
            {
                if (Change < 0)
                    return DirectionChange.Decrease;
                else if (Change > 0)
                    return DirectionChange.Increase;
                else
                    return DirectionChange.Neutral;
            }
        }

        public ChangeInValue(decimal intialValue, decimal value)
        {
            InitialValue = intialValue;
            Value = value;
        }

        public ChangeInValue(decimal value)
            : this(0, value)
        {
        }

        public ChangeInValue()
            : this(0,0)
        {
        }
    }

    class PortfolioReturn : ChangeInValue
    {
        public string Period { get; private set; }
        public bool NotApplicable { get; set; }

        public string ValueText
        {
            get
            {
                if (NotApplicable)
                    return "--%";
                else
                    return Value.ToString("p");
            }
        }       

        public PortfolioReturn(string period, decimal value)
            : base(value)
        {
            Period = period;
        }

        public PortfolioReturn(string period)
            : this(period, 0)

        {
            NotApplicable = true;
        }
    } 

}
