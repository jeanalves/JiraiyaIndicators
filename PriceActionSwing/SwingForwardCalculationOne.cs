namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class SwingForwardCalculationOne : Calculation
    {
        protected override CalculationData CalculateFirstSwingPoint()
        {
            LogPrinter.Print(owner, "SwingForwardCalculationOne.CalculateFirstSwingPoint()");

            double highCandidateValue = highs.GetValueAt(0);
            double lowCandidateValue = lows.GetValueAt(0);
            int highCandidateIndex = 0;
            int lowCandidateIndex = 0;

            if (owner.CurrentBar == owner.Strength)
            {
                LogPrinter.Print(owner, "Testing the high values to find the highest one");
                // Test the high values to find the highest one
                for (int i = 0; i < owner.Strength; i++)
                {
                    if (highs.GetValueAt(i) > highCandidateValue)
                    {
                        highCandidateValue = highs.GetValueAt(i);
                        highCandidateIndex = i;
                        LogPrinter.Print(owner, "High index : " + i);
                    }
                }

                LogPrinter.Print(owner, "Testing the low values to find the lowest one");
                // Test the low values to find the lowest one
                for (int i = 0; i < owner.Strength; i++)
                {
                    if (lows.GetValueAt(i) < lowCandidateValue)
                    {
                        lowCandidateValue = lows.GetValueAt(i);
                        lowCandidateIndex = i;
                        LogPrinter.Print(owner, "Low index : " + i);
                    }
                }

                if (highCandidateIndex < lowCandidateIndex)
                {
                    LogPrinter.Print(owner, "Add high," +
                        " highCandidateValue: " + highCandidateValue +
                        ", highCandidateIndex: " + highCandidateIndex);
                    return new CalculationData(true, highCandidateValue, highCandidateIndex, Point.SideSwing.High);
                }
                else if (highCandidateIndex > lowCandidateIndex)
                {
                    LogPrinter.Print(owner, "Add low," +
                        " lowCandidateValue: " + lowCandidateValue +
                        ", lowCandidateIndex: " + lowCandidateIndex);
                    return new CalculationData(true, lowCandidateValue, lowCandidateIndex, Point.SideSwing.Low);
                }
                else if(highCandidateIndex == lowCandidateIndex)
                {
                    LogPrinter.Print(owner, "Error: The two indexes are equal.");
                    LogPrinter.PrintError(owner, "Error: The two indexes are equal. " +
                        "High bar index: " + highCandidateIndex + " Low bar index: " + lowCandidateIndex);
                    return new CalculationData(true, 0, 0, Point.SideSwing.Unknow);
                }
                else
                {
                    LogPrinter.Print(owner, "Error: No point was found");
                    LogPrinter.PrintError(owner, "Error: No point was found");
                }
            }

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(owner, "SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            bool isRising= true;
            bool isFalling = true;
            bool isOverHighStrength = (owner.CurrentBar - LastLow().BarIndex) >= owner.Strength;
            bool isOverLowStrength = (owner.CurrentBar - LastHigh().BarIndex) >= owner.Strength;

            double swingHighCandidateValue = highs[0];
            double swingLowCandidateValue = lows[0];

            int initForIndex = owner.CurrentBar - (int)owner.Strength;

            LogPrinter.Print(owner, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            // High calculation
            for (int i = initForIndex; i < owner.CurrentBar; i++)
                if (swingHighCandidateValue < highs.GetValueAt(i))
                    isRising = false;

            // Low calculation
            for (int i = initForIndex; i < owner.CurrentBar; i++)
                if (swingLowCandidateValue > lows.GetValueAt(i))
                    isFalling = false;

            LogPrinter.Print(owner, "isRising : " + isRising + ", isFalling : " + isFalling);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, swingHighCandidateValue, owner.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, swingLowCandidateValue, owner.CurrentBar, Point.SideSwing.Low);

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print(owner, "SwingForwardCalculationOne.CalculateEachTickSwing()");
            return base.CalculateEachTickSwing();
        }
    }
}
