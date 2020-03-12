using NinjaTrader.NinjaScript;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class SwingDrawing
    {
        public static void DrawPoint(NinjaScriptBase owner, Point point)
        {
            switch(point.CurrentSideSwing)
            {
                case Point.SideSwing.High:
                    DrawWrapper.DrawDot(owner, point.PointIndex, 
                                        point.BarIndex, point.Price, Brushes.Green);
                    DrawWrapper.DrawText(owner, point.PointIndex, 
                                         point.BarIndex, point.Price, 15);
                    break;

                case Point.SideSwing.Low:
                    DrawWrapper.DrawDot(owner, point.PointIndex,
                                        point.BarIndex, point.Price, Brushes.Red);
                    DrawWrapper.DrawText(owner, point.PointIndex,
                                         point.BarIndex, point.Price, -15);
                    break;

                case Point.SideSwing.Unknow:
                    DrawWrapper.DrawDot(owner, point.PointIndex,
                                        point.BarIndex, point.Price, Brushes.Gray);
                    break;
            }
        }

        public static void DrawZigZag(NinjaScriptBase owner, Point pointOne, Point pointTwo)
        {
            DrawWrapper.DrawLine(owner, 
                                 pointTwo.PointIndex,
                                 pointTwo.BarIndex, pointTwo.Price,
                                 pointOne.BarIndex, pointOne.Price);
        }
    }
}
