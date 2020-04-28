using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;
using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public abstract class Calculation
    {
        // Fields

        protected readonly NinjaScriptBase owner;

        protected List<MatrixPoints> matrixPointsList = new List<MatrixPoints>();
        protected CalculationData currentCalculationData = new CalculationData(false);

        // Initialization

        protected Calculation(NinjaScriptBase owner)
        {
            this.owner = owner;
        }

        public void Calculate(PriceActionSwingClass priceActionSwingClass)
        {
            currentCalculationData = OnNewUpdateEvent(priceActionSwingClass);

            if(currentCalculationData.isNewMatrixPoints)
            {
                AddOrUpdateIfNewMatrixPoints();
            }
        }

        // Protected (methods)

        protected abstract CalculationData OnNewUpdateEvent(PriceActionSwingClass priceActionSwingClass);

        // Private (methods)

        private void AddOrUpdateIfNewMatrixPoints()
        {

        }

        // Miscellaneous

        public struct CalculationData
        {
            public bool isNewMatrixPoints;
            public MatrixPoints currentMatrixPoint;

            public CalculationData(bool isNewMatrixPoints)
            {
                this.isNewMatrixPoints = isNewMatrixPoints;
                this.currentMatrixPoint = null;
            }

            public CalculationData(bool isNewMatrixPoints, MatrixPoints currentMatrixPoint)
            {
                this.isNewMatrixPoints = isNewMatrixPoints;
                this.currentMatrixPoint = currentMatrixPoint;
            }
        }
    }
}
