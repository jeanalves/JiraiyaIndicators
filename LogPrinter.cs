using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.DrawingTools;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class LogPrinter
    {
        private static int lastBarIndex = 0;

        public static void Print(NinjaScriptBase owner, object text)
        {
            if (owner.CurrentBar != lastBarIndex || owner.CurrentBar == 0)
                Code.Output.Process(owner.CurrentBar + " " + text, PrintTo.OutputTab1);
            else
                Code.Output.Process(GetStringSpace(owner.CurrentBar) + " " + text, PrintTo.OutputTab1);

            lastBarIndex = owner.CurrentBar;
        }

        public static  void PrintError(NinjaScriptBase owner, object text)
        {
            Draw.TextFixed(owner, "Error", text.ToString(), TextPosition.BottomRight);
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
