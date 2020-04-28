using NinjaTrader.NinjaScript;
using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class PriceActionSwingClass
    {
        // Fields

        private readonly NinjaScriptBase owner;
        private readonly TickCalculation tickCalculation;
        private readonly SwingForwardCalculation swingForwardCalculation;
        private readonly SwingForwardCalculationOld swingForwardCalculationOld;

        // Initialization

        public PriceActionSwingClass(NinjaScriptBase owner, CalculationTypeList calculationType, double strength, bool useHighLow, bool showLog)
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

        public void Calculate()
        {
            GetChosenCalculationObject().Calculate();

            // Every time a new point event happens, it will be drawn in this 3 lines code
            if (GetChosenCalculationObject().CalcData.isNewSwing)
            {
                OnPointCalculationUpdate();
            }
        }

        // Public (methods)

        public Point GetPoint(int pointsAgo)
        {
            GetChosenCalculationObject().Calculate();
            return GetChosenCalculationObject().GetPoint(pointsAgo);
        }

        // Private (methods)

        private void OnPointCalculationUpdate()
        {
            SwingDrawing.DrawPoint(owner, GetChosenCalculationObject().GetPoint(0));

            // Test if there is more than two points to be able in draw a line
            if (GetChosenCalculationObject().GetPointsList().Count > 1)
            {
                SwingDrawing.DrawZigZag(owner,
                                        GetChosenCalculationObject().GetPoint(1),
                                        GetChosenCalculationObject().GetPoint(0));
            }
        }

        private Calculation GetChosenCalculationObject()
        {
            switch (CalculationType)
            {
                case CalculationTypeList.Tick:
                    return tickCalculation;

                case CalculationTypeList.SwingForward:
                    return swingForwardCalculation;

                case CalculationTypeList.SwingForwardOld:
                    return swingForwardCalculationOld;
            }

            return null;
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
                GetChosenCalculationObject().Calculate();
                return GetChosenCalculationObject().GetPointsList();
            }
            private set { }
        }

        public Point GetLastPoint
        {
            get
            {
                GetChosenCalculationObject().Calculate();
                return GetChosenCalculationObject().GetPoint(0);
            }
            private set { }
        }
    }
}

public enum CalculationTypeList
{
    Tick,
    SwingForward,
    SwingForwardOld
}
