using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.DrawingTools;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public static class ToDraw
    {
        public static void DrawDotWrapper(NinjaScriptBase owner,
                                          int pointIndex,
                                          int barIndex,
                                          double price,
                                          Brush dotColor)
        {
            Draw.Dot(owner, ("Dot " + pointIndex), true,
                        ConvertBarIndexToBarsAgo(owner, barIndex), price,
                        dotColor).OutlineBrush = Brushes.Transparent;
        }

        public static void DrawTextWrapper(NinjaScriptBase owner,
                                           int pointIndex,
                                           int barIndex,
                                           double price,
                                           int yPixelOffSet)
        {
            Draw.Text(owner, ("Text " + pointIndex), true, pointIndex.ToString(),
                        ConvertBarIndexToBarsAgo(owner, barIndex), price, yPixelOffSet, Brushes.White,
                        new Gui.Tools.SimpleFont("Arial", 11), System.Windows.TextAlignment.Center,
                        Brushes.Transparent, Brushes.Transparent, 100);
        }

        public static void DrawZigZagWrapper(NinjaScriptBase owner,
                                             int pointIndex,
                                             int barIndex1,
                                             double price1,
                                             int barIndex0,
                                             double price0)
        {
            Draw.Line(owner, "Line " + pointIndex, false,
                ConvertBarIndexToBarsAgo(owner, barIndex1), price1,
                ConvertBarIndexToBarsAgo(owner, barIndex0), price0,
                Brushes.White, Gui.DashStyleHelper.Solid, 3);
        }

        private static int ConvertBarIndexToBarsAgo(NinjaScriptBase owner, int barIndex)
        {
            return (barIndex - owner.CurrentBar) < 0 ? (barIndex - owner.CurrentBar) * -1 : barIndex - owner.CurrentBar;
        }
    }
}
