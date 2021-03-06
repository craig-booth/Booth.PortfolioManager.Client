﻿using System;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class IncomeReceivedViewModel : TransactionViewModel
    {

        private decimal _FrankedAmount;
        public decimal FrankedAmount
        {
            get
            {
                return _FrankedAmount;
            }
            set
            {
                _FrankedAmount = value;

                ClearErrors();

                if (_FrankedAmount < 0.00m)
                    AddError("Franked amount must not be less than 0");
            }
        }

        private decimal _UnfrankedAmount;
        public decimal UnfrankedAmount
        {
            get
            {
                return _UnfrankedAmount;
            }
            set
            {
                _UnfrankedAmount = value;

                ClearErrors();

                if (_UnfrankedAmount < 0.00m)
                    AddError("Unfranked amount must not be less than 0");
            }
        }

        private decimal _FrankingCredits;
        public decimal FrankingCredits
        {
            get
            {
                return _FrankingCredits;
            }
            set
            {
                _FrankingCredits = value;

                ClearErrors();

                if (_FrankingCredits < 0.00m)
                    AddError("Franking credits must not be less than 0");
            }
        }

        private decimal _Interest;
        public decimal Interest
        {
            get
            {
                return _Interest;
            }
            set
            {
                _Interest = value;

                ClearErrors();

                if (_Interest < 0.00m)
                    AddError("Interest must not be less than 0");
            }
        }

        private decimal _TaxDeferred;
        public decimal TaxDeferred
        {
            get
            {
                return _TaxDeferred;
            }
            set
            {
                _TaxDeferred = value;

                ClearErrors();

                if (_TaxDeferred < 0.00m)
                    AddError("Tax Deferred must not be less than 0");
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

        private decimal _DrpCashBalance;
        public decimal DrpCashBalance
        {
            get
            {
                return _DrpCashBalance;
            }
            set
            {
                _DrpCashBalance = value;

                ClearErrors();

                if (_DrpCashBalance < 0.00m)
                    AddError("DRP Cash Balance must not be less than 0");
            }
        }

        public bool CreateCashTransaction { get; set; }

        public IncomeReceivedViewModel(IncomeReceived incomeReceived, RestClient restClient)
            : base(incomeReceived, "Income Received", TransactionStockSelection.Holdings, restClient)
        {

        }

        protected override void CopyTransactionToFields()
        {
            base.CopyTransactionToFields();

            if (_Transaction != null)
            {
                RecordDate = ((IncomeReceived)_Transaction).RecordDate;
                PaymentDate = ((IncomeReceived)_Transaction).TransactionDate;
                FrankedAmount = ((IncomeReceived)_Transaction).FrankedAmount;
                UnfrankedAmount = ((IncomeReceived)_Transaction).UnfrankedAmount;
                FrankingCredits = ((IncomeReceived)_Transaction).FrankingCredits;
                TaxDeferred = ((IncomeReceived)_Transaction).TaxDeferred;
                Interest = ((IncomeReceived)_Transaction).Interest;
                CreateCashTransaction = ((IncomeReceived)_Transaction).CreateCashTransaction;
                DrpCashBalance = ((IncomeReceived)_Transaction).DrpCashBalance;
            }
            else
            {
                PaymentDate = Date.Today;
                RecordDate = Date.Today;
                FrankedAmount = 0.00m;
                UnfrankedAmount = 0.00m;
                FrankingCredits = 0.00m;
                TaxDeferred = 0.00m;
                Interest = 0.00m;
                CreateCashTransaction = true;
                DrpCashBalance = 0.00m;
            }
        }

        protected override void CopyFieldsToTransaction()
        {
            if (_Transaction == null)
                _Transaction = new IncomeReceived();

            base.CopyFieldsToTransaction();

            var income = (IncomeReceived)_Transaction;
            income.RecordDate = RecordDate;
            income.TransactionDate = PaymentDate;
            income.FrankedAmount = FrankedAmount;
            income.UnfrankedAmount = UnfrankedAmount;
            income.FrankingCredits = FrankingCredits;
            income.TaxDeferred = TaxDeferred;
            income.Interest = Interest;
            income.CreateCashTransaction = CreateCashTransaction;
            income.DrpCashBalance = DrpCashBalance;
        }

    }
}
