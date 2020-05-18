using System.Windows.Media;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Tools;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class DrawingProperties
    {
        public DrawingProperties(bool isDotAutoScale, Brush upDotColor, Brush downDotColor, 
            Brush dotOutlineBrush, bool isTextAutoScale, int textYPixelOffSet, Brush textColor, 
            SimpleFont textSimpleFont, Brush textOutlineBrush, Brush textAreaBrush, int textAreaOpacity, 
            bool isLineAutoScale, Brush lineColor, DashStyleHelper lineDashStyle, int lineWidth)
        {
            IsDotAutoScale = isDotAutoScale;
            UpDotColor = upDotColor;
            DownDotColor = downDotColor;
            DotOutlineBrush = dotOutlineBrush;
            IsTextAutoScale = isTextAutoScale;
            TextYPixelOffSet = textYPixelOffSet;
            TextColor = textColor;
            TextSimpleFont = textSimpleFont;
            TextOutlineBrush = textOutlineBrush;
            TextAreaBrush = textAreaBrush;
            TextAreaOpacity = textAreaOpacity;
            IsLineAutoScale = isLineAutoScale;
            LineColor = lineColor;
            LineDashStyle = lineDashStyle;
            LineWidth = lineWidth;
        }

        // Dot properties

        public bool IsDotAutoScale { get; set; }
        public Brush UpDotColor { get; set; }
        public Brush DownDotColor { get; set; }
        public Brush DotOutlineBrush { get; set; }

        // Text properties

        public bool IsTextAutoScale { get; set; }
        public int TextYPixelOffSet { get; set; }
        public Brush TextColor { get; set; }
        public Gui.Tools.SimpleFont TextSimpleFont { get; set; }
        public Brush TextOutlineBrush { get; set; }
        public Brush TextAreaBrush { get; set; }
        public int TextAreaOpacity { get; set; }

        // Line properties

        public bool IsLineAutoScale { get; set; }
        public Brush LineColor { get; set; }
        public Gui.DashStyleHelper LineDashStyle { get; set; }
        public int LineWidth { get; set; }

    }
}
