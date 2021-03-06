﻿using System;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class OpeningBalanceViewModel : TransactionViewModel
    {
        private int _Units;
        public int Units
        {
            get
            {
                return _Units;
            }
            set
            {
                _Units = value;

                ClearErrors();

                if (_Units <= 0)
                    AddError("Units must be greater than 0");
            }
        }

        private decimal _CostBase;
        public decimal CostBase
        {
            get
            {
                return _CostBase;
            }
            set
            {
                _CostBase = value;

                ClearErrors();

                if (_CostBase < 0.00m)
                    AddError("Cost Base must not be less than 0");
            }
        }

        private Date _AquisitionDate;
        public Date AquisitionDate
        {
            get
            {
                return _AquisitionDate;
            }
            set
            {
                _AquisitionDate = value;

                ClearErrors();

                if (_AquisitionDate > RecordDate)
                    AddError("Aquisition Date must not be after the Opening Balance date");
            }
        }

        public OpeningBalanceViewModel(OpeningBalance openingBalance, RestClient restClient)
            : base(openingBalance, "Opening Balance", TransactionStockSelection.Stocks, restClient)
        {
        }

        protected override void CopyTransactionToFields()
        {
            base.CopyTransactionToFields();

            if (_Transaction != null)
            {
                var openingBalance = (OpeningBalance)_Transaction;

                Units = openingBalance.Units;
                CostBase = openingBalance.CostBase;
                AquisitionDate = openingBalance.AquisitionDate;
            }
            else
            {
                Units = 0;
                CostBase = 0.00m;
                AquisitionDate = Date.Today;
            }
        }

        protected override void CopyFieldsToTransaction()
        {
            if (_Transaction == null)
                _Transaction = new OpeningBalance();

            base.CopyFieldsToTransaction();

            var openingBalance = (OpeningBalance)_Transaction;
            openingBalance.TransactionDate = RecordDate;
            openingBalance.Units = Units;
            openingBalance.CostBase = CostBase;
            openingBalance.AquisitionDate = AquisitionDate;
        }

    }
}
