﻿using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class DowPivotClass
    {
        // Fields

        private readonly NinjaScriptBase owner;
        private readonly PriceActionSwingClass priceActionSwingClass;

        // Initialization

        public DowPivotClass(NinjaScriptBase owner, bool showLog)
        {
            this.owner = owner;
            ShowLog = showLog;

            priceActionSwingClass = new PriceActionSwingClass(owner, CalculationTypeList.SwingForward, 2, true, true);

            if (!ShowLog)
            {
                //logPrinter.SetIndicatorAsInvisible(owner);
            }
        }

        // Public (methods)

        public void Calculate()
        {
            var unused = priceActionSwingClass.GetPoint(5);
        }

        // Properties

        public bool ShowLog { get; set; }
    }
}