using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwingOLD
{
    public static class SwingDrawing
    {
        public static void DrawPoint(Point point)
        {
            switch(point.CurrentSideSwing)
            {
                case Point.SidePoint.High:
                    DrawWrapper.DrawDot(OwnerReference.NinjaScriptBase, point.PointIndex, 
                                        point.BarIndex, point.Price, Brushes.Green);
                    DrawWrapper.DrawText(OwnerReference.NinjaScriptBase, point.PointIndex, 
                                         point.BarIndex, point.Price, 15);
                    break;

                case Point.SidePoint.Low:
                    DrawWrapper.DrawDot(OwnerReference.NinjaScriptBase, point.PointIndex,
                                        point.BarIndex, point.Price, Brushes.Red);
                    DrawWrapper.DrawText(OwnerReference.NinjaScriptBase, point.PointIndex,
                                         point.BarIndex, point.Price, -15);
                    break;

                case Point.SidePoint.Unknow:
                    DrawWrapper.DrawDot(OwnerReference.NinjaScriptBase, point.PointIndex,
                                        point.BarIndex, point.Price, Brushes.Gray);
                    break;
            }
        }

        public static void DrawZigZag(Point pointOne, Point pointTwo)
        {
            DrawWrapper.DrawLine(OwnerReference.NinjaScriptBase, 
                                 pointTwo.PointIndex,
                                 pointTwo.BarIndex, pointTwo.Price,
                                 pointOne.BarIndex, pointOne.Price);
        }
    }
}
