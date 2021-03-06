﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.Client.ViewModels;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels.Transactions
{
    enum TransactionStockSelection { None, Holdings, TradeableHoldings, Stocks, TradeableStocks }

    abstract class TransactionViewModel : ViewModel, IEditableObject
    {
        protected bool _BeingEdited;
        protected TransactionStockSelection _StockSelection;
        protected RestClient _RestClient;
        public Transaction _Transaction { get; protected set; }

        public string Description { get; private set; }    

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

                if (_StockSelection != TransactionStockSelection.None)
                {
                    ClearErrors();

                    if (_Stock == null)
                        AddError("Company is required");
                }
            }
        }

        public Date TransactionDate { get; set; }

        private Date _RecordDate;
        public Date RecordDate
        {
            get
            {
                return _RecordDate;
            }

            set
            {
                if (_RecordDate != value)
                {
                    _RecordDate = value;

                    if (_BeingEdited)
                        PopulateAvailableStocks(_RecordDate);
                }
            }
        }

        public string Comment { get; set; }

        public string Heading { get; set; }

        public ObservableCollection<StockViewItem> AvailableStocks { get; private set; }

        public bool IsStockEditable
        {
            get
            {
                return (_Transaction == null);
            }
        }

        public TransactionViewModel(Transaction transaction, string heading, TransactionStockSelection stockSeletion, RestClient restClient)
        {
            _StockSelection = stockSeletion;
            _RestClient = restClient;
            Heading = heading;
            _Transaction = transaction;

            if (_StockSelection != TransactionStockSelection.None)
                AvailableStocks = new ObservableCollection<StockViewItem>();

            CopyTransactionToFields();
        }

        public void BeginEdit()
        {
            _BeingEdited = true;

            if (_StockSelection != TransactionStockSelection.None)
                PopulateAvailableStocks(RecordDate);
        }

        public void EndEdit()
        {
            _BeingEdited = false;

            CopyFieldsToTransaction();
        }

        public void CancelEdit()
        {
            _BeingEdited = false;
        }

        protected virtual void CopyTransactionToFields()
        {
            if (_Transaction != null)
            {
                Stock = null;
                Description = _Transaction.Description;
                TransactionDate = _Transaction.TransactionDate;
                RecordDate = _Transaction.TransactionDate;
                Comment = _Transaction.Comment;
            }
            else
            {
                Stock = null;
                Description = "";
                TransactionDate = Date.Today;
                RecordDate = Date.Today;
                Comment = "";
            }
        }

        protected virtual void CopyFieldsToTransaction()
        {
            if (_Transaction != null)
            { 
                _Transaction.Stock = Stock.Id;
                _Transaction.TransactionDate = TransactionDate;
                _Transaction.Comment = Comment;
            }
        }

        private async void PopulateAvailableStocks(Date date)
        {
            if (_StockSelection == TransactionStockSelection.None)
                return;

            AvailableStocks.Clear();

            if (_StockSelection == TransactionStockSelection.Holdings)
            {
                var response = await _RestClient.Holdings.Get(date);

                var stocks = response.Select(x => new StockViewItem(x.Stock));
                foreach (var stock in stocks.OrderBy(x => x.FormattedCompanyName))
                    AvailableStocks.Add(stock);
            }
            else if (_StockSelection == TransactionStockSelection.TradeableHoldings)
            {
                var response = await _RestClient.Holdings.Get(date);

                var stocks = response.Select(x => new StockViewItem(x.Stock));
                foreach (var stock in stocks.OrderBy(x => x.FormattedCompanyName))
                    AvailableStocks.Add(stock);
            }
            else if (_StockSelection == TransactionStockSelection.Stocks)
            {
                var stocks = await _RestClient.Stocks.Get(date);
                
                foreach (var stock in stocks)
                {
                    var stockItem = new StockViewItem(stock.Id, stock.AsxCode, stock.Name);
                    AvailableStocks.Add(stockItem);
                    if (!stock.StapledSecurity)
                    {
                        foreach (var childSecurity in stock.ChildSecurities)
                        {
                            stockItem = new StockViewItem(stock.Id, childSecurity.AsxCode, childSecurity.Name);
                            AvailableStocks.Add(stockItem);
                        }
                    }
                }
            }
            else if (_StockSelection == TransactionStockSelection.TradeableStocks)
            {
                var response = await _RestClient.Stocks.Get(date);

                var stocks = response.Select(x => new StockViewItem(x.Id, x.AsxCode, x.Name));
                foreach (var stock in stocks.OrderBy(x => x.FormattedCompanyName))
                    AvailableStocks.Add(stock);
            }

            if (_Transaction != null)
                Stock = AvailableStocks.FirstOrDefault(x => x.Id == _Transaction.Stock);
        }
    }


}
