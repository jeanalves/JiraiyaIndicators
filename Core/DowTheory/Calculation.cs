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
            currentCalculationData = OnCalculationRequest(priceActionSwingClass);

            if(currentCalculationData.isNewMatrixPoints)
            {
                AddOrUpdateIfNewMatrixPoints(currentCalculationData);
            }
        }

        // Public (methods)

        public MatrixPoints GetMatrixPoints(int matrixPointsAgo)
        {
            return matrixPointsList.Count < matrixPointsAgo + 1 ? null : matrixPointsList[(matrixPointsList.Count - 1) - matrixPointsAgo];
        }

        // Protected (methods)

        protected abstract CalculationData OnCalculationRequest(PriceActionSwingClass priceActionSwingClass);

        // Private (methods)

        private void AddOrUpdateIfNewMatrixPoints(CalculationData calculationData)
        {
            switch(calculationData.currentMatrixPoints.trendSideSignal)
            {
                // Add long pattern if the first point was an low
                case MatrixPoints.WhichTrendSideSignal.Bullish:
                    matrixPointsList.Add(new MatrixPoints(calculationData.currentMatrixPoints.PointsList,
                                                          matrixPointsList.Count,
                                                          MatrixPoints.WhichTrendSideSignal.Bullish,
                                                          calculationData.currentMatrixPoints.graphicPatternType));
                    break;

                // Add short pattern if the first point was an high
                case MatrixPoints.WhichTrendSideSignal.Bearish:
                    matrixPointsList.Add(new MatrixPoints(calculationData.currentMatrixPoints.PointsList,
                                                          matrixPointsList.Count,
                                                          MatrixPoints.WhichTrendSideSignal.Bearish,
                                                          calculationData.currentMatrixPoints.graphicPatternType));
                    break;
            }

            // Update long pivot
            if (calculationData.currentMatrixPoints.PointsList[3].CurrentSideSwing == Point.SidePoint.Low &&
                calculationData.currentMatrixPoints.PointsList[0].Price > matrixPointsList[matrixPointsList.Count -1].PointsList[0].Price)
            {
                //matrixPointsList[matrixPointsList.Count - 1] = calculationData.currentMatrixPoint;
            }

        }

        // Properties

        public CalculationData CalcData
        {
            get
            {
                return currentCalculationData;
            }
        }

        // Miscellaneous

        public struct CalculationData
        {
            public bool isNewMatrixPoints;
            public MatrixPoints currentMatrixPoints;

            public CalculationData(bool isNewMatrixPoints)
            {
                this.isNewMatrixPoints = isNewMatrixPoints;
                this.currentMatrixPoints = null;
            }

            public CalculationData(bool isNewMatrixPoints, MatrixPoints currentMatrixPoint)
            {
                this.isNewMatrixPoints = isNewMatrixPoints;
                this.currentMatrixPoints = currentMatrixPoint;
            }
        }
    }
}
