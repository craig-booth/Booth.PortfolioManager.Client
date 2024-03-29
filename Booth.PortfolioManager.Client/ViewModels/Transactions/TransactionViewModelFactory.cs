﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.ViewModels;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class TransactionViewModelFactory
    {
        private RestClient _RestClient;

        public Dictionary<string, TransactionType> TransactionTypes { get; private set; }

        public TransactionViewModelFactory(RestClient restClient)
        {
            _RestClient = restClient;

            TransactionTypes = new Dictionary<string, TransactionType>();
            TransactionTypes.Add("Buy", TransactionType.Aquisition);
            TransactionTypes.Add("Cash Transaction", TransactionType.CashTransaction);
            TransactionTypes.Add("Adjust Cost Base", TransactionType.CostBaseAdjustment);
            TransactionTypes.Add("Sell", TransactionType.Disposal);
            TransactionTypes.Add("Income Received", TransactionType.IncomeReceived);
            TransactionTypes.Add("Opening Balance", TransactionType.OpeningBalance);
            TransactionTypes.Add("Return Of Capital", TransactionType.ReturnOfCapital);
            TransactionTypes.Add("Adjust Unit Count", TransactionType.UnitCountAdjustment);
        }

        public TransactionViewModel CreateTransactionViewModel(TransactionType type)
        {
            if (type == TransactionType.Aquisition)
                return new AquisitionViewModel(null, _RestClient);
            else if (type == TransactionType.CashTransaction)
                return new CashTransactionViewModel(null, _RestClient);
            else if (type == TransactionType.CostBaseAdjustment)
                return new CostBaseAdjustmentViewModel(null, _RestClient);
            else if (type == TransactionType.Disposal)
                return new DisposalViewModel(null, _RestClient);
            else if (type == TransactionType.IncomeReceived)
                return new IncomeReceivedViewModel(null, _RestClient);
            else if (type == TransactionType.OpeningBalance)
                return new OpeningBalanceViewModel(null, _RestClient);
            else if (type == TransactionType.ReturnOfCapital)
                return new ReturnOfCapitalViewModel(null, _RestClient);
            else if (type == TransactionType.UnitCountAdjustment)
                return new UnitCountAdjustmentViewModel(null, _RestClient);
            else
                throw new NotSupportedException();
        }

        public async Task<TransactionViewModel> CreateTransactionViewModel(Guid id)
        {
            var transaction = await _RestClient.Transactions.Get(id);

            return CreateTransactionViewModel(transaction);
        }

        public TransactionViewModel CreateTransactionViewModel(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Aquisition)
                return new AquisitionViewModel(transaction as Aquisition, _RestClient);
            else if (transaction.Type == TransactionType.CashTransaction)
                return new CashTransactionViewModel(transaction as CashTransaction, _RestClient);
            else if (transaction.Type == TransactionType.CostBaseAdjustment)
                return new CostBaseAdjustmentViewModel(transaction as CostBaseAdjustment, _RestClient);
            else if (transaction.Type == TransactionType.Disposal)
                return new DisposalViewModel(transaction as Disposal, _RestClient);
            else if (transaction.Type == TransactionType.IncomeReceived)
                return new IncomeReceivedViewModel(transaction as IncomeReceived, _RestClient);
            else if (transaction.Type == TransactionType.OpeningBalance)
                return new OpeningBalanceViewModel(transaction as OpeningBalance, _RestClient);
            else if (transaction.Type == TransactionType.ReturnOfCapital)
                return new ReturnOfCapitalViewModel(transaction as ReturnOfCapital, _RestClient);
            else if (transaction.Type == TransactionType.UnitCountAdjustment)
                return new UnitCountAdjustmentViewModel(transaction as UnitCountAdjustment, _RestClient);
            else
                throw new NotSupportedException();
        }
    }
}
