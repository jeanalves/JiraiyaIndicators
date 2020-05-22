using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript;
using System.Collections.Generic;

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
            List<Point> pointsList = new List<Point>();
            MatrixPoints.WhichTrendSideSignal whichTrend = MatrixPoints.WhichTrendSideSignal.None;

            bool isNewMatrixPoints = true;

            if (priceActionSwingClass.GetPoint(firstPoint) == null)
            {
                return new CalculationData();
            }

            pointsList.Add(priceActionSwingClass.GetPoint(fourthPoint));
            pointsList.Add(priceActionSwingClass.GetPoint(thirdPoint));
            pointsList.Add(priceActionSwingClass.GetPoint(secondPoint));
            pointsList.Add(priceActionSwingClass.GetPoint(firstPoint));

            // Test a long pivot
            if (pointsList[firstPoint].CurrentSideSwing == Point.SidePoint.Low)
            {
                isNewMatrixPoints = pointsList[firstPoint].Price < pointsList[thirdPoint].Price &&
                                    pointsList[secondPoint].Price < pointsList[fourthPoint].Price;

                whichTrend = MatrixPoints.WhichTrendSideSignal.Bullish;
            }
            // Test a short pivot
            else if (pointsList[firstPoint].CurrentSideSwing == Point.SidePoint.High)
            {
                isNewMatrixPoints = pointsList[firstPoint].Price > pointsList[thirdPoint].Price &&
                                    pointsList[secondPoint].Price > pointsList[fourthPoint].Price;

                whichTrend = MatrixPoints.WhichTrendSideSignal.Bearish;
            }

            return isNewMatrixPoints == true ? new CalculationData(pointsList, whichTrend, MatrixPoints.WhichGraphicPatternType.Pivot) : 
                new CalculationData();
        }
    }
}
