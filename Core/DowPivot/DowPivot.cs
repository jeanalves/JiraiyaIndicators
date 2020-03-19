using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class DowPivot
    {
        // Fields

        public LogPrinter logPrinter = new LogPrinter();

        private readonly NinjaScriptBase owner;
        private readonly PriceActionSwing.PriceActionSwing priceActionSwing;

        // Initialization

        public DowPivot(NinjaScriptBase owner, bool showLog)
        {
            this.owner = owner;
            ShowLog = showLog;

            priceActionSwing = new PriceActionSwing.PriceActionSwing(owner, CalculationTypeList.SwingForward, 2, true, true);

            if (!ShowLog)
            {
                logPrinter.SetIndicatorAsInvisible(owner);
            }
        }

        // Public (methods)

        public void Calculate()
        {
            var unused = priceActionSwing.GetPoint(5);
        }

        // Properties

        public bool ShowLog { get; set; }
    }
}
