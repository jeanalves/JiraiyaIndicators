namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class SwingForwardCalculationOne : Calculation
    {
        public SwingForwardCalculationOne(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher) : base(launcher) { }

        protected override CalculationData CalculateFirstSwingPoint()
        {
            LogPrinter.Print("SwingForwardCalculationOne.CalculateFirstSwingPoint()");

            double highCandidateValue = highs.GetValueAt(0);
            double lowCandidateValue = lows.GetValueAt(0);
            int highCandidateIndex = 0;
            int lowCandidateIndex = 0;

            if (launcher.CurrentBar == launcher.Strength)
            {
                LogPrinter.Print("Testing the high values to find the highest one");
                // Test the high values to find the highest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (highs.GetValueAt(i) > highCandidateValue)
                    {
                        highCandidateValue = highs.GetValueAt(i);
                        highCandidateIndex = i;
                        LogPrinter.Print("High index : " + i);
                    }
                }

                LogPrinter.Print("Testing the low values to find the lowest one");
                // Test the low values to find the lowest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (lows.GetValueAt(i) < lowCandidateValue)
                    {
                        lowCandidateValue = lows.GetValueAt(i);
                        lowCandidateIndex = i;
                        LogPrinter.Print("Low index : " + i);
                    }
                }

                if (highCandidateIndex < lowCandidateIndex)
                {
                    LogPrinter.Print("Add high," +
                        " highCandidateValue: " + highCandidateValue +
                        ", highCandidateIndex: " + highCandidateIndex);
                    return new CalculationData(true, highCandidateValue, highCandidateIndex, Point.SideSwing.High);
                }
                else if (highCandidateIndex > lowCandidateIndex)
                {
                    LogPrinter.Print("Add low," +
                        " lowCandidateValue: " + lowCandidateValue +
                        ", lowCandidateIndex: " + lowCandidateIndex);
                    return new CalculationData(true, lowCandidateValue, lowCandidateIndex, Point.SideSwing.Low);
                }
                else if(highCandidateIndex == lowCandidateIndex)
                {
                    LogPrinter.Print("Error: The two indexes are equal.");
                    LogPrinter.PrintError("Error: The two indexes are equal. " +
                        "High bar index: " + highCandidateIndex + " Low bar index: " + lowCandidateIndex);
                    return new CalculationData(true, 0, 0, Point.SideSwing.Unknow);
                }
                else
                {
                    LogPrinter.Print("Error: No point was found");
                    LogPrinter.PrintError("Error: No point was found");
                }
            }

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print("SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            bool isRising= true;
            bool isFalling = true;
            bool isOverHighStrength = (launcher.CurrentBar - LastLow().BarIndex) >= launcher.Strength;
            bool isOverLowStrength = (launcher.CurrentBar - LastHigh().BarIndex) >= launcher.Strength;

            double swingHighCandidateValue = highs[0];
            double swingLowCandidateValue = lows[0];

            int initForIndex = launcher.CurrentBar - (int)launcher.Strength;

            LogPrinter.Print("isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            // High calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingHighCandidateValue < highs.GetValueAt(i))
                    isRising = false;

            // Low calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingLowCandidateValue > lows.GetValueAt(i))
                    isFalling = false;

            LogPrinter.Print("isRising : " + isRising + ", isFalling : " + isFalling);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, swingHighCandidateValue, launcher.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, swingLowCandidateValue, launcher.CurrentBar, Point.SideSwing.Low);

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print("SwingForwardCalculationOne.CalculateEachTickSwing()");
            return base.CalculateEachTickSwing();
        }
    }
}
