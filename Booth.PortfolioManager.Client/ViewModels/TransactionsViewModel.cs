﻿using System;
using System.Collections.ObjectModel;

using Booth.Common;
using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.Utilities;
using Booth.PortfolioManager.Client.ViewModels.Transactions;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class TransactionsViewModel : PortfolioViewModel
    {
        public TransactionsViewModel(string label, ViewParameter parameter, EditTransactionViewModel editTransactionViewModel)
            : base(label, parameter)
        {
            Options.AllowStockSelection = true;
            Options.DateSelection = DateSelectionType.Range;

            _Heading = label;

            Transactions = new ObservableCollection<TransactionViewItem>();
            TransactionCommands = new ObservableCollection<RelayUICommand>();

            _EditTransactionViewModel = editTransactionViewModel;

            EditTransactionCommand = new RelayCommand<TransactionViewItem>(EditTransaction);
            CreateTransactionCommand = new RelayCommand<TransactionType>(CreateTransaction);
        }

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

        public TransactionViewModelFactory TransactionViewModelFactory { get; private set; }

        public ObservableCollection<TransactionViewItem> Transactions { get; private set; }

        private EditTransactionViewModel _EditTransactionViewModel;

        public ObservableCollection<RelayUICommand> TransactionCommands { get; private set; }

        public override void Activate()
        {
            if (_Parameter != null)
            {
                TransactionViewModelFactory = new TransactionViewModelFactory(_Parameter.RestClient);

                TransactionCommands.Clear();
                foreach (var transactionType in TransactionViewModelFactory.TransactionTypes)
                    TransactionCommands.Add(new RelayUICommand(transactionType.Key, () => CreateTransaction(transactionType.Value)));
            }

            base.Activate();
        }

        public async override void RefreshView()
        {
            RestApi.Portfolios.TransactionsResponse response;

            try
            {
                if (_Parameter.Stock.Id == Guid.Empty)
                    response = await _Parameter.RestClient.Portfolio.GetTransactions(_Parameter.DateRange);
                else
                    response = await _Parameter.RestClient.Holdings.GetTransactions(_Parameter.Stock.Id, _Parameter.DateRange);
            }
            catch (Exception e)
            {
                response = null;
            }
     
            if (response == null)
                return;

            Transactions.Clear();

            for (var i = response.Transactions.Count - 1; i >= 0; i--)
                Transactions.Add(new TransactionViewItem(response.Transactions[i]));

            OnPropertyChanged("");            
        }

        public RelayCommand<TransactionType> CreateTransactionCommand { get; private set; }
        private void CreateTransaction(TransactionType transactionType)
        {
            _EditTransactionViewModel.CreateTransaction(transactionType);
        }

        public RelayCommand<TransactionViewItem> EditTransactionCommand { get; private set; }
        private void EditTransaction(TransactionViewItem transaction)
        {
            _EditTransactionViewModel.EditTransaction(transaction.Id);
        }

    }

    class TransactionViewItem
    {
        public Guid Id { get; private set; }
        public StockViewItem Stock { get; private set; }
        public Date TransactionDate { get; private set; }
        public string Description { get; private set; }

        public TransactionViewItem(RestApi.Portfolios.TransactionsResponse.TransactionItem transaction)
        {
            Id = transaction.Id;
            Stock = new StockViewItem(transaction.Stock);
            TransactionDate = transaction.TransactionDate;
            Description = transaction.Description;
        }
    }

}
