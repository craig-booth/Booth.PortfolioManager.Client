﻿using System;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class ReturnOfCapitalViewModel : TransactionViewModel
    {
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

                if (_Amount < 0.00m)
                    AddError("Amount must not be less than 0");
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

        public bool CreateCashTransaction { get; set; }

        public ReturnOfCapitalViewModel(ReturnOfCapital returnOfCapital, RestClient restClient)
            : base(returnOfCapital, "Return of Capital", TransactionStockSelection.Holdings, restClient)
        {

        }

        protected override void CopyTransactionToFields()
        {
            base.CopyTransactionToFields();

            if (_Transaction != null)
            {
                RecordDate = ((ReturnOfCapital)_Transaction).RecordDate;
                PaymentDate = ((ReturnOfCapital)_Transaction).TransactionDate;
                Amount = ((ReturnOfCapital)_Transaction).Amount;
                CreateCashTransaction = ((ReturnOfCapital)_Transaction).CreateCashTransaction;
            }
            else
            {
                RecordDate = Date.Today;
                PaymentDate = Date.Today;
                Amount = 0.00m;
                CreateCashTransaction = true;
            }
        }

        protected override void CopyFieldsToTransaction()
        {
            if (_Transaction == null)
                _Transaction = new ReturnOfCapital();

            base.CopyFieldsToTransaction();

            var returnOfCapital = (ReturnOfCapital)_Transaction;
            returnOfCapital.RecordDate = RecordDate;
            returnOfCapital.TransactionDate = PaymentDate;
            returnOfCapital.Amount = Amount;
            returnOfCapital.CreateCashTransaction = CreateCashTransaction;
        }


    }
}
