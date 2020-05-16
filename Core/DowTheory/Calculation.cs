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
            // Here, no code is needed to update the matrix due to the object
            // reference already passed in the list of points, from MatrixPoints.PointsList
            if (!IsNewMatrixTheSameTheLastOne(calculationData.currentMatrixPoints))
            {
                switch (calculationData.currentMatrixPoints.trendSideSignal)
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
            }
        }

        private bool IsNewMatrixTheSameTheLastOne(MatrixPoints newMatrixPoints)
        {
            if (matrixPointsList.Count <= 0)
            {
                return false;
            }
            
            MatrixPoints lastMatrix = matrixPointsList[matrixPointsList.Count - 1];

            // Test all points except the last one
            for (int i = 0; i<= newMatrixPoints.PointsList.Count - 2; i++)
            {
                if (lastMatrix.PointsList[i].Index != newMatrixPoints.PointsList[i].Index)
                {
                    return false;
                }
            }

            return true;
        }

        // Properties

        public CalculationData CalcData
        {
            get
            {
                return currentCalculationData;
            }
        }

        public MatrixPoints LastMatrixPoints
        {
            get
            {
                return GetMatrixPoints(0);
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
