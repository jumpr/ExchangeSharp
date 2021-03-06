﻿/*
MIT LICENSE

Copyright 2017 Digital Ruby, LLC - http://www.digitalruby.com

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace ExchangeSharp
{
    using System;

    /// <summary>Result of an exchange order</summary>
    public class ExchangeOrderResult
    {
        /// <summary>Order id</summary>
        public string OrderId { get; set; }

        /// <summary>Result of the order</summary>
        public ExchangeAPIOrderResult Result { get; set; }

        /// <summary>Message if any</summary>
        public string Message { get; set; }

        /// <summary>Original order amount in the market currency. 
        /// E.g. ADA/BTC would be ADA</summary>
        public decimal Amount { get; set; }

        /// <summary>Amount filled in the market currency.</summary>
        public decimal AmountFilled { get; set; }

        /// <summary>The limit price on the order in the ratio of base/market currency.
        /// E.g. 0.000342 ADA/ETH</summary>
        public decimal Price { get; set; }

        /// <summary>Price per unit in the ratio of base/market currency.
        /// E.g. 0.000342 ADA/ETH</summary>
        public decimal AveragePrice { get; set; }

        /// <summary>Order datetime in UTC</summary>
        public DateTime OrderDate { get; set; }

        /// <summary>Symbol. E.g. ADA/ETH</summary>
        public string Symbol { get; set; }

        /// <summary>Whether the order is a buy or sell</summary>
        public bool IsBuy { get; set; }

        /// <summary>The fees on the order (not a percent).
        /// E.g. 0.0025 ETH</summary>
        public decimal Fees { get; set; }

        /// <summary>The currency the fees are in. 
        /// If not set, this is probably the base currency</summary>
        public string FeesCurrency { get; set; }

        /// <summary>Append another order to this order - order id and type must match</summary>
        /// <param name="other">Order to append</param>
        public void AppendOrderWithOrder(ExchangeOrderResult other)
        {
            if ((this.OrderId != null) && (this.Symbol != null) && ((this.OrderId != other.OrderId) || (this.IsBuy != other.IsBuy) || (this.Symbol != other.Symbol)))
            {
                throw new InvalidOperationException("Appending orders requires order id, symbol and is buy to match");
            }

            decimal tradeSum = this.Amount + other.Amount;
            decimal baseAmount = this.Amount;
            this.Amount += other.Amount;
            this.AmountFilled += other.AmountFilled;
            this.Fees += other.Fees;
            this.FeesCurrency = other.FeesCurrency;
            this.AveragePrice = (this.AveragePrice * (baseAmount / tradeSum)) + (other.AveragePrice * (other.Amount / tradeSum));
            this.OrderId = other.OrderId;
            this.OrderDate = this.OrderDate == default(DateTime) ? other.OrderDate : this.OrderDate;
            this.Symbol = other.Symbol;
            this.IsBuy = other.IsBuy;
        }

        /// <summary>Returns a string that represents this instance.</summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            return $"[{this.OrderDate}], {(this.IsBuy ? "Buy" : "Sell")} {this.AmountFilled} of {this.Amount} {this.Symbol} {this.Result} at {this.AveragePrice}, fees paid {this.Fees} {this.FeesCurrency}";
        }
    }
}