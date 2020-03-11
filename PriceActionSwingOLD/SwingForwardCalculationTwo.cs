namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwingOLD
{
    class SwingForwardCalculationTwo : Calculation
    {
        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(ninjaScriptBase, "SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            return DefaultLogicCalculation();
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print(ninjaScriptBase, "SwingForwardCalculationOne.CalculateEachTickSwing()");

            return DefaultLogicCalculation();
        }

        private CalculationData DefaultLogicCalculation()
        {
            bool newHigh = true;
            bool newLow = true;

            // For a new swing high in an uptrend, Highs[BarsInProgress][0] must be 
            // greater than the current swing high
            if (LastSideTrend() == Point.SideSwing.High)
            {
                if (LastHigh().Price > highs[0])
                    newHigh = false;
            }

            // For a new swing low in a downtrend, Lows[BarsInProgress][0] must be 
            // smaller than the current swing low
            if (LastSideTrend() == Point.SideSwing.Low)
            {
                if (LastLow().Price < lows[0])
                    newLow = false;
            }

            // Calculates if the current high value is a new swing
            if (newHigh)
            {
                for (int i = 1; i < priceActionSwing.Strength + 1; i++)
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
                for (int i = 1; i < priceActionSwing.Strength + 1; i++)
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
                if (LastSideTrend() == Point.SideSwing.High)
                {
                    return new CalculationData(true, highs[0], priceActionSwing.CurrentBar, Point.SideSwing.High);
                }
                else
                {
                    return new CalculationData(true, lows[0], priceActionSwing.CurrentBar, Point.SideSwing.Low);
                }
            }
            else if (newHigh)
            {
                return new CalculationData(true, highs[0], priceActionSwing.CurrentBar, Point.SideSwing.High);
            }
            else if (newLow)
            {
                return new CalculationData(true, lows[0], priceActionSwing.CurrentBar, Point.SideSwing.Low);
            }
            return new CalculationData(false);
        }
    }
}
