using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class DowPivot
    {
        private readonly NinjaScriptBase owner;
        private readonly PriceActionSwing.PriceActionSwing priceActionSwing;

        // Initialization

        public DowPivot(NinjaScriptBase owner, LogPrinter logPrinter ,bool showLog)
        {
            this.owner = owner;
            ShowLog = showLog;

            priceActionSwing = new PriceActionSwing.PriceActionSwing(owner, logPrinter ,CalculationTypeList.SwingForward, 2, true, false);

            if (!ShowLog)
            {
                logPrinter.SetIndicatorAsInvisible(owner);
            }
        }

        // Public (methods)

        public void Calculate()
        {
            priceActionSwing.Calculate();
        }

        // Properties

        public bool ShowLog { get; set; }
    }
}
