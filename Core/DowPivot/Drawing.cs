using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot
{
    public static class Drawing
    {
        public static void DrawPivot(NinjaScriptBase owner, MatrixPoints matrixPoints)
        {
            DrawWrapper.DrawLine(owner,
                                 matrixPoints.PointsList[3].PointIndex,
                                 matrixPoints.PointsList[3].BarIndex,
                                 matrixPoints.PointsList[3].Price,
                                 matrixPoints.PointsList[2].BarIndex,
                                 matrixPoints.PointsList[2].Price,
                                 System.Windows.Media.Brushes.Green);

            DrawWrapper.DrawLine(owner,
                                 matrixPoints.PointsList[2].PointIndex,
                                 matrixPoints.PointsList[2].BarIndex,
                                 matrixPoints.PointsList[2].Price,
                                 matrixPoints.PointsList[1].BarIndex,
                                 matrixPoints.PointsList[1].Price,
                                 System.Windows.Media.Brushes.Green);

            DrawWrapper.DrawLine(owner,
                                 matrixPoints.PointsList[1].PointIndex,
                                 matrixPoints.PointsList[1].BarIndex,
                                 matrixPoints.PointsList[1].Price,
                                 matrixPoints.PointsList[0].BarIndex,
                                 matrixPoints.PointsList[0].Price,
                                 System.Windows.Media.Brushes.Green);
        }
    }
}
