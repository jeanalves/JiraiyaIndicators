using NinjaTrader.NinjaScript.DrawingTools;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class ToDraw
    {
        public static void DrawDotWrapper(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher ,int pointIndex ,int barIndex,
            double price ,Brush dotColor)
        {
            Draw.Dot(launcher, ("Dot " + pointIndex), true,
                        launcher.ConvertBarIndexToBarsAgo(barIndex), price,
                        dotColor).OutlineBrush = Brushes.Transparent;
        }

        public static void DrawTextWrapper(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher, int pointIndex, int barIndex,
            double price, int yPixelOffSet)
        {
            Draw.Text(launcher, ("Text " + pointIndex), true, pointIndex.ToString(),
                        launcher.ConvertBarIndexToBarsAgo(barIndex), price, yPixelOffSet, Brushes.White,
                        new Gui.Tools.SimpleFont("Arial", 11), System.Windows.TextAlignment.Center,
                        Brushes.Transparent, Brushes.Transparent, 100);
        }

        public static void DrawZigZagWrapper(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher,
                                             int pointIndex,
                                             int barIndex1,
                                             double price1,
                                             int barIndex0,
                                             double price0)
        {
            Draw.Line(launcher, "Line " + pointIndex, false,
                launcher.ConvertBarIndexToBarsAgo(barIndex1), price1,
                launcher.ConvertBarIndexToBarsAgo(barIndex0), price0,
                Brushes.White, Gui.DashStyleHelper.Solid, 3);
        }
    }
}
