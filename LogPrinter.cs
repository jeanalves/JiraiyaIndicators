using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.DrawingTools;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class LogPrinter
    {
        private static int lastBarIndex = 0;

        public static void Print(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher, object text)
        {
            if (launcher.CurrentBar != lastBarIndex || launcher.CurrentBar == 0)
                Code.Output.Process(launcher.CurrentBar + " " + text, PrintTo.OutputTab1);
            else
                Code.Output.Process(GetStringSpace(launcher.CurrentBar) + " " + text, PrintTo.OutputTab1);

            lastBarIndex = launcher.CurrentBar;
        }

        public static  void PrintError(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher, object text)
        {
            Draw.TextFixed(launcher, "Error", text.ToString(), TextPosition.BottomRight);
        }

        private static string GetStringSpace(object text)
        {
            // The multiplication number was found doing 7 divided by 3,
            // this means that each number printed equals to 2.3333333333 spaces.
            double charCount = text.ToString().Length * 2.3333333333;

            string space = "";

            for (int i = 0; i < charCount; i++)
                space += " ";

            return space;
        }
    }
}
