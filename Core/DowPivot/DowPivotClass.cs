using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class DowPivotClass
    {
        // Fields

        private readonly NinjaScriptBase owner;
        private readonly PriceActionSwingClass priceActionSwingClass;
        private readonly PivotCalculation pivotCalculation;

        // Initialization

        public DowPivotClass(NinjaScriptBase owner)
        {
            this.owner = owner;

            priceActionSwingClass = new PriceActionSwingClass(owner, CalculationTypeList.SwingForward, 2, true, true);
            pivotCalculation = new PivotCalculation(owner);

            /*
            if (!ShowLog)
            {
                logPrinter.SetIndicatorAsInvisible(owner);
            }
            */
        }

        // Public (methods)

        public void Calculate()
        {
            priceActionSwingClass.Calculate();
            pivotCalculation.Calculate(priceActionSwingClass);
        }
    }
}
