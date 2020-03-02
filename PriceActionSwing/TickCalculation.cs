namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class TickCalculation : Calculation
    {
        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(owner, "TickCalculation.CalculateEachBarSwingPoint()");

            bool isRising = highs[0] > highs[1];
            bool isFalling = lows[0] < lows[1];
            LogPrinter.Print(owner, "isRising : " + isRising + ", isFalling: " + isFalling);
            
            bool isOverHighStrength = highs[0] > (LastLow().Price + (owner.Strength * owner.TickSize));
            bool isOverLowStrength = lows[0] < (LastHigh().Price - (owner.Strength * owner.TickSize));
            LogPrinter.Print(owner, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, highs[0], owner.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, lows[0], owner.CurrentBar, Point.SideSwing.Low);
            
            return new CalculationData(false);
        }
    }
}
