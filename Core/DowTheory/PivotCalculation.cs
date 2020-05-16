using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class PivotCalculation : Calculation
    {
        //----Bearish----|---Bullish---
        //----1----------|----------4--
        //-----\---3-----|-----2---/---
        //------\-/-\----|----/-\-/----
        //-------2---\---|---/---3-----
        //------------4--|--1----------

        const int firstPoint    = 3;
        const int secondPoint   = 2;
        const int thirdPoint    = 1;
        const int fourthPoint   = 0;

        public PivotCalculation(NinjaScriptBase owner) : base(owner) { }

        protected override CalculationData OnCalculationRequest(PriceActionSwingClass priceActionSwingClass)
        {
            MatrixPoints matrixPoints = new MatrixPoints(MatrixPoints.WhichGraphicPatternType.Pivot);

            bool isNewMatrixPoints = true;

            if (priceActionSwingClass.GetPoint(firstPoint) == null)
            {
                return new CalculationData(false);
            }

            matrixPoints.AddPoint(priceActionSwingClass.GetPoint(fourthPoint));
            matrixPoints.AddPoint(priceActionSwingClass.GetPoint(thirdPoint));
            matrixPoints.AddPoint(priceActionSwingClass.GetPoint(secondPoint));
            matrixPoints.AddPoint(priceActionSwingClass.GetPoint(firstPoint));

            // Test a long pivot
            if (matrixPoints.PointsList[firstPoint].CurrentSideSwing == Point.SidePoint.Low)
            {
                isNewMatrixPoints = matrixPoints.PointsList[firstPoint].Price < matrixPoints.PointsList[thirdPoint].Price &&
                                    matrixPoints.PointsList[secondPoint].Price < matrixPoints.PointsList[fourthPoint].Price;

                if (isNewMatrixPoints)
                {
                    matrixPoints.TrendSideSignal = MatrixPoints.WhichTrendSideSignal.Bullish;
                }
            }
            // Test a short pivot
            else if (matrixPoints.PointsList[firstPoint].CurrentSideSwing == Point.SidePoint.High)
            {
                isNewMatrixPoints = matrixPoints.PointsList[firstPoint].Price > matrixPoints.PointsList[thirdPoint].Price &&
                                    matrixPoints.PointsList[secondPoint].Price > matrixPoints.PointsList[fourthPoint].Price;

                if (isNewMatrixPoints)
                {
                    matrixPoints.TrendSideSignal = MatrixPoints.WhichTrendSideSignal.Bearish;
                }
            }

            return isNewMatrixPoints == true ? new CalculationData(true, matrixPoints) : new CalculationData(false);
        }
    }
}
