using System;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    class CashTransactionViewModel : TransactionViewModel
    {
        public CashTransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }

        public CashTransactionViewModel(CashTransaction cashTransaction, RestClient restClient)
            : base(cashTransaction, "Cash Transaction", TransactionStockSelection.None, restClient)
        {
            
        }

        protected override void CopyTransactionToFields()
        {
            base.CopyTransactionToFields();

            if (_Transaction != null)
            {
                TransactionType = ((CashTransaction)_Transaction).CashTransactionType;
                Amount = ((CashTransaction)_Transaction).Amount;
             }
            else
            {
                TransactionType = CashTransactionType.Deposit;
                Amount = 0.00m;
            }

            Stock = new StockViewItem(Guid.Empty, "", "");
        }

        protected override void CopyFieldsToTransaction()
        {
            if (_Transaction == null)
                _Transaction = new CashTransaction();

            base.CopyFieldsToTransaction();

            var cashTransaction = (CashTransaction)_Transaction;
            cashTransaction.TransactionDate = RecordDate;
            cashTransaction.CashTransactionType = TransactionType;
            cashTransaction.Amount = Amount;
        }
    }
}
