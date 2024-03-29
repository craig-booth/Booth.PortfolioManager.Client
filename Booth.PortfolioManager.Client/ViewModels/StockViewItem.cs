﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booth.PortfolioManager.Client.ViewModels
{
    public class StockViewItem
    {
        public Guid Id { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }

        public StockViewItem(Guid id, string asxCode, string name)
        {
            Id = id;
            AsxCode = asxCode;
            Name = name;
        }

        public StockViewItem(RestApi.Portfolios.Stock stock)
        {
            if (stock != null)
            {
                Id = stock.Id;
                AsxCode = stock.AsxCode;
                Name = stock.Name;
            }
            else
            {
                Id = Guid.Empty;
                AsxCode = "";
                Name = "";
            }
        }

        public string FormattedCompanyName
        {
            get
            {
                if (AsxCode != "")
                    return string.Format("{0} ({1})", Name, AsxCode);
                else
                    return Name;
            }
        }

        public string FormattedAsxCode
        {
            get
            {
                if (Name != "")
                    return string.Format("{0} ({1})", AsxCode, Name);
                else
                    return AsxCode;
            }
        }
    }
}
