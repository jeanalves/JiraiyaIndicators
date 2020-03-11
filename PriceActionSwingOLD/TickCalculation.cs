namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class TickCalculation : Calculation
    {
        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(ninjaScriptBase, "TickCalculation.CalculateEachBarSwingPoint()");

            bool isRising = highs[0] > highs[1];
            bool isFalling = lows[0] < lows[1];
            LogPrinter.Print(ninjaScriptBase, "isRising : " + isRising + ", isFalling: " + isFalling);
            
            bool isOverHighStrength = highs[0] > (LastLow().Price + (priceActionSwing.Strength * priceActionSwing.TickSize));
            bool isOverLowStrength = lows[0] < (LastHigh().Price - (priceActionSwing.Strength * priceActionSwing.TickSize));
            LogPrinter.Print(ninjaScriptBase, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, highs[0], priceActionSwing.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, lows[0], priceActionSwing.CurrentBar, Point.SideSwing.Low);
            
            return new CalculationData(false);
        }
    }
}
