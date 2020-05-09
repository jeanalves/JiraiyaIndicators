﻿using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class DowTheoryClass
    {
        // Fields

        private readonly NinjaScriptBase owner;
        private readonly PriceActionSwingClass priceActionSwingClass;
        private readonly PivotCalculation pivotCalculation;
        private readonly TrendCalculation trendCalculation;

        // Initialization

        public DowTheoryClass(NinjaScriptBase owner, CalculationTypeListDowTheory calculationTypeList, double strength)
        {
            this.owner = owner;
            CalculationType = calculationTypeList;

            priceActionSwingClass = new PriceActionSwingClass(owner, CalculationTypeList.SwingForward, strength, true, true);
            trendCalculation = new TrendCalculation(owner);
            pivotCalculation = new PivotCalculation(owner);

            /*
            if (!ShowLog)
            {
                logPrinter.SetIndicatorAsInvisible(owner);
            }
            */
        }

        // Public (methods)

        public void Compute()
        {
            priceActionSwingClass.Compute();
            GetChosenCalculationObject().Calculate(priceActionSwingClass);


            if (GetChosenCalculationObject().CalcData.isNewMatrixPoints)
            {
                OnCalculationUpdate(GetChosenCalculationObject());
            }
        }

        // Private (methods)

        private void OnCalculationUpdate(Calculation chosenCalculationObject)
        {
            // Code used for Pivots signals and Trend Signals
            switch(chosenCalculationObject.CalcData.currentMatrixPoints.trendSideSignal)
            {
                case MatrixPoints.WhichTrendSideSignal.Bullish:
                    // Enter a long signal
                    break;

                case MatrixPoints.WhichTrendSideSignal.Bearish:
                    // Enter a short signal
                    break;
            }

            Drawing.DrawPivot(owner, chosenCalculationObject.GetMatrixPoints(0));
        }

        private Calculation GetChosenCalculationObject()
        {
            switch(CalculationType)
            {
                case CalculationTypeListDowTheory.Trend:
                    return trendCalculation;
                case CalculationTypeListDowTheory.Pivot:
                    return pivotCalculation;
            }

            return null;
        }

        // Properties

        public CalculationTypeListDowTheory CalculationType { get; private set; }
    }
}

public enum CalculationTypeListDowTheory
{
    Trend,
    Pivot
}