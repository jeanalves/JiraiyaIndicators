using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class TickCalculation : Calculation
    {
        public TickCalculation(NinjaScriptBase owner, PriceActionSwingClass priceActionSwingClass) : base(owner, priceActionSwingClass) { }

        protected override CalculationData CalculateEachBarSwingPoint()
        {
            //logPrinter.Print(owner, "TickCalculation.CalculateEachBarSwingPoint()");

            bool isRising = highs[0] > highs[1];
            bool isFalling = lows[0] < lows[1];
            //logPrinter.Print(owner, "isRising : " + isRising + ", isFalling: " + isFalling);
            
            bool isOverHighStrength = highs[0] > (LastLow().Price + (priceActionSwingClass.Strength * owner.TickSize));
            bool isOverLowStrength = lows[0] < (LastHigh().Price - (priceActionSwingClass.Strength * owner.TickSize));
            //logPrinter.Print(owner, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, highs[0], owner.CurrentBar, Point.SidePoint.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, lows[0], owner.CurrentBar, Point.SidePoint.Low);
            
            return new CalculationData(false);
        }
    }
}
