using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;
using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public class PivotCalculation : Calculation
    {
        private List<Point> pointsList = new List<Point>();
        private bool isNewMatrixPoints = true;
        private MatrixPoints.WhichTrendSideSignal whichTrend = MatrixPoints.WhichTrendSideSignal.None;

        public PivotCalculation(NinjaScriptBase owner) : base(owner) { }

        protected override CalculationData OnCalculationRequest(PriceActionSwingClass priceActionSwingClass)
        {
            if (priceActionSwingClass.GetPoint(3) == null)
            {
                return new CalculationData();
            }

            pointsList.Clear();
            isNewMatrixPoints = false;
            ResetWhichTrendVariableBySMA(20);

            //----Bearish----|---Bullish---
            //----0----------|----------3--
            //-----\---2-----|-----1---/---
            //------\-/-\----|----/-\-/----
            //-------1---\---|---/---2-----
            //------------3--|--0----------

            pointsList.Add(priceActionSwingClass.GetPoint(3)); // First point or  pointsList[0]
            pointsList.Add(priceActionSwingClass.GetPoint(2)); // Second point or pointsList[1]
            pointsList.Add(priceActionSwingClass.GetPoint(1)); // Third point or  pointsList[2]
            pointsList.Add(priceActionSwingClass.GetPoint(0)); // Fourth point or pointsList[3]

            // Test a long pivot
            if ((pointsList[0].CurrentSideSwing == Point.SidePoint.Low && whichTrend != MatrixPoints.WhichTrendSideSignal.Bullish) ||
                (pointsList[0].CurrentSideSwing == Point.SidePoint.Low && IsNewMatrixTheSameTheLastOne(pointsList)))
            {
                isNewMatrixPoints = pointsList[0].Price < pointsList[2].Price &&
                                    pointsList[1].Price < pointsList[3].Price;
                if (isNewMatrixPoints)
                {
                    whichTrend = MatrixPoints.WhichTrendSideSignal.Bullish;
                }
            }
            // Test a short pivot
            else if ((pointsList[0].CurrentSideSwing == Point.SidePoint.High && whichTrend != MatrixPoints.WhichTrendSideSignal.Bearish) ||
                     (pointsList[0].CurrentSideSwing == Point.SidePoint.High && IsNewMatrixTheSameTheLastOne(pointsList)))
            {
                isNewMatrixPoints = pointsList[0].Price > pointsList[2].Price &&
                                    pointsList[1].Price > pointsList[3].Price;
                if (isNewMatrixPoints)
                {
                    whichTrend = MatrixPoints.WhichTrendSideSignal.Bearish;
                }
            }

            return isNewMatrixPoints == true ? new CalculationData(pointsList, whichTrend, MatrixPoints.WhichGraphicPatternType.Pivot) : 
                new CalculationData();
        }

        private void ResetWhichTrendVariableBySMA(int smaPeriod)
        {

        }
    }
}
