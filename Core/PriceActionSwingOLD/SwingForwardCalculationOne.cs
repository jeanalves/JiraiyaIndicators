namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwingOLD
{
    public class SwingForwardCalculationOne : Calculation
    {
        protected override CalculationData CalculateFirstSwingPoint()
        {
            LogPrinter.Print(ninjaScriptBase, "SwingForwardCalculationOne.CalculateFirstSwingPoint()");

            double highCandidateValue = highs.GetValueAt(0);
            double lowCandidateValue = lows.GetValueAt(0);
            int highCandidateIndex = 0;
            int lowCandidateIndex = 0;

            if (priceActionSwing.CurrentBar == priceActionSwing.Strength)
            {
                LogPrinter.Print(ninjaScriptBase, "Testing the high values to find the highest one");
                // Test the high values to find the highest one
                for (int i = 0; i < priceActionSwing.Strength; i++)
                {
                    if (highs.GetValueAt(i) > highCandidateValue)
                    {
                        highCandidateValue = highs.GetValueAt(i);
                        highCandidateIndex = i;
                        LogPrinter.Print(ninjaScriptBase, "High index : " + i);
                    }
                }

                LogPrinter.Print(ninjaScriptBase, "Testing the low values to find the lowest one");
                // Test the low values to find the lowest one
                for (int i = 0; i < priceActionSwing.Strength; i++)
                {
                    if (lows.GetValueAt(i) < lowCandidateValue)
                    {
                        lowCandidateValue = lows.GetValueAt(i);
                        lowCandidateIndex = i;
                        LogPrinter.Print(ninjaScriptBase, "Low index : " + i);
                    }
                }

                if (highCandidateIndex < lowCandidateIndex)
                {
                    LogPrinter.Print(ninjaScriptBase, "Add high," +
                        " highCandidateValue: " + highCandidateValue +
                        ", highCandidateIndex: " + highCandidateIndex);
                    return new CalculationData(true, highCandidateValue, highCandidateIndex, Point.SidePoint.High);
                }
                else if (highCandidateIndex > lowCandidateIndex)
                {
                    LogPrinter.Print(ninjaScriptBase, "Add low," +
                        " lowCandidateValue: " + lowCandidateValue +
                        ", lowCandidateIndex: " + lowCandidateIndex);
                    return new CalculationData(true, lowCandidateValue, lowCandidateIndex, Point.SidePoint.Low);
                }
                else if(highCandidateIndex == lowCandidateIndex)
                {
                    LogPrinter.Print(ninjaScriptBase, "Error: The two indexes are equal.");
                    LogPrinter.PrintError(ninjaScriptBase, "Error: The two indexes are equal. " +
                        "High bar index: " + highCandidateIndex + " Low bar index: " + lowCandidateIndex);
                    return new CalculationData(true, 0, 0, Point.SidePoint.Unknow);
                }
                else
                {
                    LogPrinter.Print(ninjaScriptBase, "Error: No point was found");
                    LogPrinter.PrintError(ninjaScriptBase, "Error: No point was found");
                }
            }

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(ninjaScriptBase, "SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            bool isRising= true;
            bool isFalling = true;
            bool isOverHighStrength = (priceActionSwing.CurrentBar - LastLow().BarIndex) >= priceActionSwing.Strength;
            bool isOverLowStrength = (priceActionSwing.CurrentBar - LastHigh().BarIndex) >= priceActionSwing.Strength;

            double swingHighCandidateValue = highs[0];
            double swingLowCandidateValue = lows[0];

            int initForIndex = priceActionSwing.CurrentBar - (int)priceActionSwing.Strength;

            LogPrinter.Print(ninjaScriptBase, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            // High calculation
            for (int i = initForIndex; i < priceActionSwing.CurrentBar; i++)
                if (swingHighCandidateValue < highs.GetValueAt(i))
                    isRising = false;

            // Low calculation
            for (int i = initForIndex; i < priceActionSwing.CurrentBar; i++)
                if (swingLowCandidateValue > lows.GetValueAt(i))
                    isFalling = false;

            LogPrinter.Print(ninjaScriptBase, "isRising : " + isRising + ", isFalling : " + isFalling);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, swingHighCandidateValue, priceActionSwing.CurrentBar, Point.SidePoint.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, swingLowCandidateValue, priceActionSwing.CurrentBar, Point.SidePoint.Low);

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print(ninjaScriptBase, "SwingForwardCalculationOne.CalculateEachTickSwing()");
            return base.CalculateEachTickSwing();
        }
    }
}
