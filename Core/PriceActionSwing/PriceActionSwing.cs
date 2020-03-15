using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class PriceActionSwing
    {
        // Fields

        private readonly NinjaScriptBase owner;
        private readonly TickCalculation tickCalculation;
        private readonly SwingForwardCalculation swingForwardCalculation;
        private readonly SwingForwardCalculationOld swingForwardCalculationOld;

        // Initialization

        public PriceActionSwing(NinjaScriptBase owner, CalculationTypeList calculationType, double strength, bool useHighLow, bool showLog)
        {
            this.owner = owner;
            CalculationType = calculationType;
            Strength = strength;
            UseHighLow = useHighLow;
            ShowLog = showLog;

            tickCalculation = new TickCalculation(owner, this);
            swingForwardCalculation = new SwingForwardCalculation(owner, this);
            swingForwardCalculationOld = new SwingForwardCalculationOld(owner, this);

            if (!ShowLog)
            {
                LogPrinter.SetIndicatorAsInvisible(owner);
            }

            // Everytime the F5 key is pressed automatically will clear the output window.
            LogPrinter.ResetOuputTabs();
        }

        // Public (methods)
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

                case CalculationTypeList.SwingForwardOld:
                    swingForwardCalculation.Calculate();
                    return swingForwardCalculationOld;
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

        // Properties

        public CalculationTypeList CalculationType { get; set; }
        public double Strength { get; set; }
        public bool UseHighLow { get; set; }
        public bool ShowLog { get; set; }
    }
}

public enum CalculationTypeList
{
    Tick,
    SwingForward,
    SwingForwardOld
}
