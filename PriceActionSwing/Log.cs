using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class Log
    {
        private readonly NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher;
        private int lastBarIndex = 0;
        
        public Log(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher)
        {
            this.launcher = launcher;
        }

        public void Print(object text)
        {
            if (launcher.CurrentBar != lastBarIndex || launcher.CurrentBar == 0)
                Code.Output.Process(launcher.CurrentBar + " " + text, PrintTo.OutputTab1);
            else
                Code.Output.Process(GetStringSpace(launcher.CurrentBar) + " " + text, PrintTo.OutputTab1);

            lastBarIndex = launcher.CurrentBar;
        }

        private string GetStringSpace(object text)
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
