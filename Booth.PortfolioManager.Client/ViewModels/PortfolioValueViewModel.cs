﻿using System;
using System.Collections.Generic;

using LiveCharts;

using Booth.PortfolioManager.Client.Utilities;

using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.RestApi.Client;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class PortfolioValueViewModel : PortfolioViewModel 
    {

        public ChartValues<double> PortfolioValues { get; private set; } 
        public List<string> DateValues { get; set; }

        public Func<ChartPoint, string> LabelFormatter { get; set; }
        public Func<double, string> YAxisFormatter { get; set; }

        public PortfolioValueViewModel(string label, ViewParameter parameter)
            : base(label, parameter)
        {
            Options.AllowStockSelection = true;
            Options.DateSelection = DateSelectionType.Range;

            PortfolioValues = new ChartValues<double>();
            DateValues = new List<string>();

            YAxisFormatter = value => string.Format("{0:c0}", value);
            LabelFormatter = chartPoint => string.Format("{0:c0}", chartPoint.Y);
        }

        public async override void RefreshView()
        {
            DateValues.Clear();
            PortfolioValues.Clear();

            // Determine frequency to use
            var valueFrequency = ValueFrequency.Day;
            var timeSpan = _Parameter.DateRange.ToDate - _Parameter.DateRange.FromDate;
            if (timeSpan.Days > 365 * 5)
                valueFrequency = ValueFrequency.Month;
            else if (timeSpan.Days > 365)
                valueFrequency = ValueFrequency.Week;

            PortfolioValueResponse response;
            if (_Parameter.Stock.Id == Guid.Empty)
                response = await _Parameter.RestClient.Portfolio.GetValue(_Parameter.DateRange, valueFrequency);
            else
                response = await _Parameter.RestClient.Holdings.GetValue(_Parameter.Stock.Id, _Parameter.DateRange, valueFrequency);
            if (response == null)
                return;

            // create chart data
            var values = new List<double>();
            foreach (var value in response.Values)
            {
                DateValues.Add(value.Date.ToShortDateString());
                values.Add((double)value.Price);
            }

            PortfolioValues.AddRange(values);
        }

    }
    

}
