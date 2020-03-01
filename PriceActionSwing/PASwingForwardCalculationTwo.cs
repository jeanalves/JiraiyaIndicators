using NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    class SwingForwardCalculationTwo : Calculation
    {
        public SwingForwardCalculationTwo(PriceActionSwingLauncher launcher) : base(launcher) { }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            launcher.PrintLog("SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            return DefaultLogicCalculation();
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            launcher.PrintLog("SwingForwardCalculationOne.CalculateEachTickSwing()");

            return DefaultLogicCalculation();
        }

        private CalculationData DefaultLogicCalculation()
        {
            bool newHigh = true;
            bool newLow = true;

            // For a new swing high in an uptrend, Highs[BarsInProgress][0] must be 
            // greater than the current swing high
            if (LastSideTrend() == SideSwing.High)
            {
                if (LastHigh().price > highs[0])
                    newHigh = false;
            }

            // For a new swing low in a downtrend, Lows[BarsInProgress][0] must be 
            // smaller than the current swing low
            if (LastSideTrend() == SideSwing.Low)
            {
                if (LastLow().price < lows[0])
                    newLow = false;
            }

            // Calculates if the current high value is a new swing
            if (newHigh)
            {
                for (int i = 1; i < launcher.Strength + 1; i++)
                {
                    if (highs[0] <= highs[i])
                    {
                        newHigh = false;
                        break;
                    }
                }
            }

            // Calculates if the current low value is a new swing
            if (newLow)
            {
                for (int i = 1; i < launcher.Strength + 1; i++)
                {
                    if (lows[0] >= lows[i])
                    {
                        newLow = false;
                        break;
                    }
                }
            }

            if (newHigh && newLow)
            {
                if (LastSideTrend() == SideSwing.High)
                {
                    return new CalculationData(true, highs[0], launcher.CurrentBar, SideSwing.High);
                }
                else
                {
                    return new CalculationData(true, lows[0], launcher.CurrentBar, SideSwing.Low);
                }
            }
            else if (newHigh)
            {
                return new CalculationData(true, highs[0], launcher.CurrentBar, SideSwing.High);
            }
            else if (newLow)
            {
                return new CalculationData(true, lows[0], launcher.CurrentBar, SideSwing.Low);
            }
            return new CalculationData(false);
        }
    }
}
