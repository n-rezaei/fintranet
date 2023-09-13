using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace congestion.calculator
{
    public class HowToUse
    {
        public HowToUse()
        {

        }

        void CalculateTax()
        {
            var dates = new Data().Dates.Select(DateTime.Parse).ToArray();

            var v = new Car();
            var ctc = new CongestionTaxCalculator();
            var rule = new Rules();
            var tx = ctc.GetTax(rule.GetTollFee, v, dates, 60);
        }
    }
}
