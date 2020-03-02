namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class SwingForwardCalculationOne : Calculation
    {
        protected override CalculationData CalculateFirstSwingPoint()
        {
            LogPrinter.Print(launcher, "SwingForwardCalculationOne.CalculateFirstSwingPoint()");

            double highCandidateValue = highs.GetValueAt(0);
            double lowCandidateValue = lows.GetValueAt(0);
            int highCandidateIndex = 0;
            int lowCandidateIndex = 0;

            if (launcher.CurrentBar == launcher.Strength)
            {
                LogPrinter.Print(launcher, "Testing the high values to find the highest one");
                // Test the high values to find the highest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (highs.GetValueAt(i) > highCandidateValue)
                    {
                        highCandidateValue = highs.GetValueAt(i);
                        highCandidateIndex = i;
                        LogPrinter.Print(launcher, "High index : " + i);
                    }
                }

                LogPrinter.Print(launcher, "Testing the low values to find the lowest one");
                // Test the low values to find the lowest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (lows.GetValueAt(i) < lowCandidateValue)
                    {
                        lowCandidateValue = lows.GetValueAt(i);
                        lowCandidateIndex = i;
                        LogPrinter.Print(launcher, "Low index : " + i);
                    }
                }

                if (highCandidateIndex < lowCandidateIndex)
                {
                    LogPrinter.Print(launcher, "Add high," +
                        " highCandidateValue: " + highCandidateValue +
                        ", highCandidateIndex: " + highCandidateIndex);
                    return new CalculationData(true, highCandidateValue, highCandidateIndex, Point.SideSwing.High);
                }
                else if (highCandidateIndex > lowCandidateIndex)
                {
                    LogPrinter.Print(launcher, "Add low," +
                        " lowCandidateValue: " + lowCandidateValue +
                        ", lowCandidateIndex: " + lowCandidateIndex);
                    return new CalculationData(true, lowCandidateValue, lowCandidateIndex, Point.SideSwing.Low);
                }
                else if(highCandidateIndex == lowCandidateIndex)
                {
                    LogPrinter.Print(launcher, "Error: The two indexes are equal.");
                    LogPrinter.PrintError(launcher, "Error: The two indexes are equal. " +
                        "High bar index: " + highCandidateIndex + " Low bar index: " + lowCandidateIndex);
                    return new CalculationData(true, 0, 0, Point.SideSwing.Unknow);
                }
                else
                {
                    LogPrinter.Print(launcher, "Error: No point was found");
                    LogPrinter.PrintError(launcher, "Error: No point was found");
                }
            }

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(launcher, "SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            bool isRising= true;
            bool isFalling = true;
            bool isOverHighStrength = (launcher.CurrentBar - LastLow().BarIndex) >= launcher.Strength;
            bool isOverLowStrength = (launcher.CurrentBar - LastHigh().BarIndex) >= launcher.Strength;

            double swingHighCandidateValue = highs[0];
            double swingLowCandidateValue = lows[0];

            int initForIndex = launcher.CurrentBar - (int)launcher.Strength;

            LogPrinter.Print(launcher, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            // High calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingHighCandidateValue < highs.GetValueAt(i))
                    isRising = false;

            // Low calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingLowCandidateValue > lows.GetValueAt(i))
                    isFalling = false;

            LogPrinter.Print(launcher, "isRising : " + isRising + ", isFalling : " + isFalling);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, swingHighCandidateValue, launcher.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, swingLowCandidateValue, launcher.CurrentBar, Point.SideSwing.Low);

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print(launcher, "SwingForwardCalculationOne.CalculateEachTickSwing()");
            return base.CalculateEachTickSwing();
        }
    }
}
