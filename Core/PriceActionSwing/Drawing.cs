﻿using NinjaTrader.NinjaScript;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class Drawing
    {
        public static void DrawPoint(NinjaScriptBase owner, Point point)
        {
            switch(point.CurrentSideSwing)
            {
                case Point.SidePoint.High:
                    DrawWrapper.DrawDot(owner, point.Index, 
                                        point.BarIndex, point.Price, Brushes.Green);
                    DrawWrapper.DrawText(owner, point.Index, 
                                         point.BarIndex, point.Price, 15);
                    break;

                case Point.SidePoint.Low:
                    DrawWrapper.DrawDot(owner, point.Index,
                                        point.BarIndex, point.Price, Brushes.Red);
                    DrawWrapper.DrawText(owner, point.Index,
                                         point.BarIndex, point.Price, -15);
                    break;

                case Point.SidePoint.Unknow:
                    DrawWrapper.DrawDot(owner, point.Index,
                                        point.BarIndex, point.Price, Brushes.Gray);
                    break;
            }
        }

        public static void DrawZigZag(NinjaScriptBase owner, Point pointOne, Point pointTwo)
        {
            DrawWrapper.DrawLine(owner, 
                                 pointTwo.Index,
                                 pointTwo.BarIndex, pointTwo.Price,
                                 pointOne.BarIndex, pointOne.Price);
        }
    }
}
