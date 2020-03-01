﻿using NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class SwingForwardCalculationOne : Calculation
    {
        public SwingForwardCalculationOne(PriceActionSwingLauncher launcher) : base(launcher) { }

        protected override CalculationData CalculateFirstSwingPoint()
        {
            launcher.PrintLog("SwingForwardCalculationOne.CalculateFirstSwingPoint()");

            double highCandidateValue = highs.GetValueAt(0);
            double lowCandidateValue = lows.GetValueAt(0);
            int highCandidateIndex = 0;
            int lowCandidateIndex = 0;

            if (launcher.CurrentBar == launcher.Strength)
            {
                launcher.PrintLog("Testing the high values to find the highest one");
                // Test the high values to find the highest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (highs.GetValueAt(i) > highCandidateValue)
                    {
                        highCandidateValue = highs.GetValueAt(i);
                        highCandidateIndex = i;
                        launcher.PrintLog("High index : " + i);
                    }
                }

                launcher.PrintLog("Testing the low values to find the lowest one");
                // Test the low values to find the lowest one
                for (int i = 0; i < launcher.Strength; i++)
                {
                    if (lows.GetValueAt(i) < lowCandidateValue)
                    {
                        lowCandidateValue = lows.GetValueAt(i);
                        lowCandidateIndex = i;
                        launcher.PrintLog("Low index : " + i);
                    }
                }

                if (highCandidateIndex < lowCandidateIndex)
                {
                    launcher.PrintLog("Add high," +
                        " highCandidateValue: " + highCandidateValue +
                        ", highCandidateIndex: " + highCandidateIndex);
                    return new CalculationData(true, highCandidateValue, highCandidateIndex, SideSwing.High);
                }
                else if (highCandidateIndex > lowCandidateIndex)
                {
                    launcher.PrintLog("Add low," +
                        " lowCandidateValue: " + lowCandidateValue +
                        ", lowCandidateIndex: " + lowCandidateIndex);
                    return new CalculationData(true, lowCandidateValue, lowCandidateIndex, SideSwing.Low);
                }
                else if(highCandidateIndex == lowCandidateIndex)
                {
                    launcher.PrintLog("Error: The two indexes are equal.");
                    launcher.PrintError("Error: The two indexes are equal. " +
                        "High bar index: " + highCandidateIndex + " Low bar index: " + lowCandidateIndex);
                    return new CalculationData(true, 0, 0, SideSwing.Unknow);
                }
                else
                {
                    launcher.PrintLog("Error: No point was found");
                    launcher.PrintError("Error: No point was found");
                }
            }

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            launcher.PrintLog("SwingForwardCalculationOne.CalculateEachBarSwingPoint()");

            bool isRising= true;
            bool isFalling = true;
            bool isOverHighStrength = (launcher.CurrentBar - LastLow().barIndex) >= launcher.Strength;
            bool isOverLowStrength = (launcher.CurrentBar - LastHigh().barIndex) >= launcher.Strength;

            double swingHighCandidateValue = highs[0];
            double swingLowCandidateValue = lows[0];

            int initForIndex = launcher.CurrentBar - (int)launcher.Strength;

            launcher.PrintLog("isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            // High calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingHighCandidateValue < highs.GetValueAt(i))
                    isRising = false;

            // Low calculation
            for (int i = initForIndex; i < launcher.CurrentBar; i++)
                if (swingLowCandidateValue > lows.GetValueAt(i))
                    isFalling = false;

            launcher.PrintLog("isRising : " + isRising + ", isFalling : " + isFalling);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, swingHighCandidateValue, launcher.CurrentBar, SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, swingLowCandidateValue, launcher.CurrentBar, SideSwing.Low);

            return new CalculationData(false);
        }

        protected override CalculationData CalculateEachTickSwing()
        {
            launcher.PrintLog("SwingForwardCalculationOne.CalculateEachTickSwing()");
            return base.CalculateEachTickSwing();
        }
    }
}
