using NinjaTrader.NinjaScript;
using System.Collections.Generic;

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
            tickCalculation = new TickCalculation(owner, this);
            swingForwardCalculation = new SwingForwardCalculation(owner, this);
            swingForwardCalculationOld = new SwingForwardCalculationOld(owner, this);

            CalculationType = calculationType;
            Strength = strength;
            UseHighLow = useHighLow;
            ShowLog = showLog;

            if (!ShowLog)
            {
                //logPrinter.SetIndicatorAsInvisible(owner);
            }
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

        public Point GetPoint(int pointsAgo)
        {
            return Calculate().GetPoint(pointsAgo);
        }

        // Properties

        public CalculationTypeList CalculationType { get; private set; }
        public double Strength { get; private set; }
        public bool UseHighLow { get; private set; }
        public bool ShowLog { get; private set; }

        public List<Point> GetPointsList
        {
            get
            {
                return Calculate().GetPointsList();
            }
        }

        public Point GetLastPoint
        {
            get
            {
                return Calculate().GetPoint(0);
            }
        }
    }
}

public enum CalculationTypeList
{
    Tick,
    SwingForward,
    SwingForwardOld
}
