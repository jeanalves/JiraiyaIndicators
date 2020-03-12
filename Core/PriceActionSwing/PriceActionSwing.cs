using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class PriceActionSwing
    {
        private readonly NinjaScriptBase owner;
        private readonly TickCalculation tickCalculation;
        private readonly SwingForwardCalculation swingForwardCalculation;

        public CalculationTypeList CalculationType { get; set; }
        public double Strength { get; set; }
        public bool UseHighLow { get; set; }
        public bool ShowLog { get; set; }

        public PriceActionSwing(NinjaScriptBase owner, CalculationTypeList calculationType, double strength, bool useHighLow, bool showLog)
        {
            this.owner = owner;
            tickCalculation = new TickCalculation(owner, this);
            swingForwardCalculation = new SwingForwardCalculation(owner, this);

            CalculationType = calculationType;
            Strength = strength;
            UseHighLow = useHighLow;
            ShowLog = showLog;

            if (!ShowLog)
            {
                LogPrinter.SetIndicatorAsInvisible(owner);
            }

            //Everytime the F5 key is pressed automatically will clear the output window.
            LogPrinter.ResetOuputTabs();
        }

        public Calculation Calculate()
        {
            switch (CalculationType)
            {
                case CalculationTypeList.Tick:
                    tickCalculation.Calculate();
                    return tickCalculation;

                case CalculationTypeList.SwingForward:
                    swingForwardCalculation.Calculate();
                    return swingForwardCalculation;
            }

            return null;
        }

        public void OnPointCalculationUpdate(int pointsCount, Point pointOne, Point pointTwo)
        {
            // Every time a new point event happens, it will be drawn in this method
            SwingDrawing.DrawPoint(owner, pointTwo);

            if (pointsCount > 1)
            {
                // Every time a new point event happens, it will be drawn in this method
                SwingDrawing.DrawZigZag(owner, pointOne, pointTwo);
            }
        }
    }
}

public enum CalculationTypeList
{
    Tick,
    SwingForward
}
