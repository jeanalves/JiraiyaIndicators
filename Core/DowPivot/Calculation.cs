using NinjaTrader.NinjaScript;
using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class Calculation
    {
        // Fields

        protected List<Pivot> pivotList = new List<Pivot>();

        protected readonly NinjaScriptBase owner;
        protected readonly LogPrinter logPrinter;

        // Initialization

        protected Calculation(NinjaScriptBase owner, LogPrinter logPrinter)
        {
            this.owner = owner;
            this.logPrinter = logPrinter;
        }

        public void Calculate()
        {

        }

        // Miscellaneous
        
        private enum CalculationStage
        {
            FirstPoint,
            EachBarSwingPoint,
            EachTickSwingPoint
        }
    }
}
