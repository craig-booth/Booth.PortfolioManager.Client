﻿
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class CostBaseAdjustmentViewModel : TransactionViewModel
    {
        public decimal _Percentage;
        public decimal Percentage
        {
            get
            {
                return _Percentage;
            }
            set
            {
                _Percentage = value;

                ClearErrors();

                if ((_Percentage <= 0) || (_Percentage > 100))
                    AddError("Percentage must be greater than 0 and less than or equal to 100");
            }
        }

        public CostBaseAdjustmentViewModel(CostBaseAdjustment costBaseAdjustment, RestClient restClient)
            : base(costBaseAdjustment, "Costbase Adjustment", TransactionStockSelection.Holdings, restClient)
        {

        }

        protected override void CopyTransactionToFields()
        {
            base.CopyTransactionToFields();

            if (_Transaction != null)
                Percentage = ((CostBaseAdjustment)_Transaction).Percentage * 100;
            else
                Percentage = 0.00m;
        }

        protected override void CopyFieldsToTransaction()
        {
            if (_Transaction == null)
                _Transaction = new CostBaseAdjustment();

            base.CopyFieldsToTransaction();

            var costBaseAdjustment = (CostBaseAdjustment)_Transaction;
            costBaseAdjustment.TransactionDate = RecordDate;
            costBaseAdjustment.Percentage = Percentage / 100;
        }
    }
}
