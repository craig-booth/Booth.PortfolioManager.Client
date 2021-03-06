﻿using System.Linq;
using System.Collections.ObjectModel;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.Client.Utilities;


namespace Booth.PortfolioManager.Client.ViewModels
{

    class TaxableIncomeViewModel : PortfolioViewModel
    {

        public ObservableCollection<IncomeItemViewModel> Income { get; private set; }

        private string _Heading;
        new public string Heading
        {
            get
            {
                return _Heading;
            }
            private set
            {
                _Heading = value;
                OnPropertyChanged();
            }
        }

        public TaxableIncomeViewModel(string label, ViewParameter parameter)
            : base(label, parameter)
        {
            Options.AllowStockSelection = false;
            Options.DateSelection = DateSelectionType.FinancialYear;

            Income = new ObservableCollection<IncomeItemViewModel>();
        }

        public async override void RefreshView()
        {
            Heading = string.Format("Taxable Income Report for {0}/{1} Financial Year", _Parameter.FinancialYear - 1, _Parameter.FinancialYear);

            var response = await _Parameter.RestClient.Portfolio.GetIncome(new DateRange(DateUtils.StartOfFinancialYear(_Parameter.FinancialYear), DateUtils.EndOfFinancialYear(_Parameter.FinancialYear)));
            if (response == null)
                return;

            Income.Clear();
            foreach (var incomeItem in response.Income.OrderBy(x => x.Stock.Name))
                Income.Add(new IncomeItemViewModel(incomeItem));

            OnPropertyChanged("");
        }

    }

    class IncomeItemViewModel
    {
        public StockViewItem Stock { get; private set; }

        public decimal UnfrankedAmount { get; private set; }
        public decimal FrankedAmount { get; private set; }
        public decimal FrankingCredits { get; private set; }
        public decimal NetIncome { get; private set; }
        public decimal GrossIncome { get; private set; }

        public IncomeItemViewModel(IncomeResponse.IncomeItem income)
        {
            Stock = new StockViewItem(income.Stock);

            UnfrankedAmount = income.UnfrankedAmount;
            FrankedAmount = income.FrankedAmount;
            FrankingCredits = income.FrankingCredits;
            NetIncome = income.NetIncome;
            GrossIncome = income.GrossIncome;
        }
    }

}
