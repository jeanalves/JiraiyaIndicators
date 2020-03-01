using NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class TickCalculation : Calculation
    {
        public TickCalculation(PriceActionSwingLauncher launcher) : base(launcher) { }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            launcher.PrintLog("TickCalculation.CalculateEachBarSwingPoint()");

            bool isRising = highs[0] > highs[1];
            bool isFalling = lows[0] < lows[1];
            launcher.PrintLog("isRising : " + isRising + ", isFalling: " + isFalling);
            
            bool isOverHighStrength = highs[0] > (LastLow().price + (launcher.Strength * launcher.TickSize));
            bool isOverLowStrength = lows[0] < (LastHigh().price - (launcher.Strength * launcher.TickSize));
            launcher.PrintLog("isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, highs[0], launcher.CurrentBar, SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, lows[0], launcher.CurrentBar, SideSwing.Low);
            
            return new CalculationData(false);
        }
    }
}
