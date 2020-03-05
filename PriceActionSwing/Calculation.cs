using NinjaTrader.NinjaScript;
using System.Collections.Generic;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public abstract class Calculation
    {
        protected readonly NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing priceActionSwing = OwnerReference.PriceActionSwing;
        protected readonly NinjaScriptBase ninjaScriptBase = OwnerReference.NinjaScriptBase;
        protected Series<double> highs;
        protected Series<double> lows;
        protected List<Point> points = new List<Point>();
        
        protected Calculation()
        {
            highs = new Series<double>(priceActionSwing);
            lows = new Series<double>(priceActionSwing);
        }

        // Initialization

        public void Calculate()
        {
            SetValues();

            CalculationData calculationData = new CalculationData(false);
            CalculationStage calculationStage = CalculationStage.FirstPoint;

            if (points.Count == 0)
            {
                calculationData = CalculateFirstSwingPoint();
            }
            else if (priceActionSwing.State == State.Historical)
            {
                calculationData = CalculateEachBarSwingPoint();
                calculationStage = CalculationStage.EachBarSwingPoint;
            }
            else if (priceActionSwing.State == State.Realtime)
            {
                calculationData = CalculateEachTickSwing();
                calculationStage = CalculationStage.EachTickSwingPoint;
            }

            if (calculationData.isNewSwing)
            {
                AddUpdatePoints(calculationData, calculationStage);
            }
        }

        // Public (methods)

        public Point.SideSwing LastSideTrend()
        {
            return points[points.Count - 1].CurrentSideSwing;
        }

        public double LastPrice()
        {
            return points[points.Count - 1].Price;
        }

        public Point LastHigh()
        {
            if (points.Count > 0)
            {
                for (int i = points.Count - 1; i > 0; i--)
                    if (points[i].CurrentSideSwing == Point.SideSwing.High)
                        return points[i];
            }
            else if (points.Count == 0)
            {
                throw new System.Exception();
            }

            return points[points.Count - 1];
        }

        public Point LastLow()
        {
            if (points.Count > 0)
            {
                for (int i = points.Count - 1; i > 0; i--)
                    if (points[i].CurrentSideSwing == Point.SideSwing.Low)
                        return points[i];
            }
            else if (points.Count == 0)
            {
                throw new System.Exception();
            }

            return points[points.Count - 1];
        }

        public List<Point> GetPointsList()
        {
            return points;
        }

        public Point GetLastPoint()
        {
            return points[points.Count - 1];
        }

        // Protected (methods)

        protected virtual CalculationData CalculateFirstSwingPoint()
        {
            LogPrinter.Print(ninjaScriptBase, "Virtual Calculation.CalculateFirstSwingPoint()");

            Point.SideSwing sideSwing = priceActionSwing.Close.GetValueAt(0) > priceActionSwing.Open.GetValueAt(0) ?
                Point.SideSwing.Low : Point.SideSwing.High;

            if (priceActionSwing.UseHighLow)
            {
                switch (sideSwing)
                {
                    case Point.SideSwing.High:
                        return new CalculationData(true, priceActionSwing.High.GetValueAt(0), 0, sideSwing);

                    case Point.SideSwing.Low:
                        return new CalculationData(true, priceActionSwing.Low.GetValueAt(0), 0, sideSwing);
                }
            }
            else
            {
                return new CalculationData(true, priceActionSwing.Open.GetValueAt(0), 0, sideSwing);
            }

            return new CalculationData(false);
        }

        protected abstract CalculationData CalculateEachBarSwingPoint();

        protected virtual CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print(ninjaScriptBase, "virtual Calculation.CalculateEachTickSwing()");

            return new CalculationData(false);
        }

        // Private (methods)

        private void SetValues()
        {
            if (priceActionSwing.UseHighLow)
            {
                highs[0] = priceActionSwing.High[0];
                lows[0] = priceActionSwing.Low[0];
            }
            else
            {
                highs[0] = priceActionSwing.Close[0];
                lows[0] = priceActionSwing.Close[0];
            }
        }

        private void AddUpdatePoints(CalculationData calculationData, CalculationStage calculationStage)
        {
            switch (calculationStage)
            {
                case CalculationStage.FirstPoint:

                    if (calculationData.sideSwing == Point.SideSwing.High)
                    {
                        AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == Point.SideSwing.Low)
                    {
                        AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == Point.SideSwing.Unknow && points.Count == 0)
                    {
                        AddUnknow(priceActionSwing.Open.GetValueAt(0), 0, points.Count, calculationData.sideSwing);
                    }

                    break;

                case CalculationStage.EachBarSwingPoint:

                    DefaultAddUpdatePointsManagement(calculationData);

                    break;

                case CalculationStage.EachTickSwingPoint:

                    DefaultAddUpdatePointsManagement(calculationData);

                    break;
            }

            if (points.Count > 1)
            {
                DrawWrapper.DrawZigZag(ninjaScriptBase, points[points.Count - 2].PointIndex,
                    points[points.Count - 2].BarIndex, points[points.Count - 2].Price,
                    points[points.Count -1].BarIndex, points[points.Count -1].Price);
            }
        }

        private void DefaultAddUpdatePointsManagement(CalculationData calculationData)
        {
            if (calculationData.sideSwing == Point.SideSwing.High && LastSideTrend() != Point.SideSwing.High)
            {
                AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == Point.SideSwing.Low && LastSideTrend() != Point.SideSwing.Low)
            {
                AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == Point.SideSwing.High && LastSideTrend() == Point.SideSwing.High &&
                calculationData.price > LastPrice())
            {
                UpdateHigh(calculationData.price, calculationData.barIndex);
            }
            else if (calculationData.sideSwing == Point.SideSwing.Low && LastSideTrend() == Point.SideSwing.Low &&
                calculationData.price < LastPrice())
            {
                UpdateLow(calculationData.price, calculationData.barIndex);
            }
        }

        private void AddHigh(double price, int barIndex, int pointIndex, Point.SideSwing sideSwing)
        {
            LogPrinter.Print(ninjaScriptBase, "Calculation.AddHigh()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            DrawWrapper.DrawDot(ninjaScriptBase, pointIndex, barIndex, price, Brushes.Green);
            DrawWrapper.DrawText(ninjaScriptBase, pointIndex, barIndex, price, 15);
        }

        private void AddLow(double price, int barIndex, int pointIndex, Point.SideSwing sideSwing)
        {
            LogPrinter.Print(ninjaScriptBase, "Calculation.AddLow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            DrawWrapper.DrawDot(ninjaScriptBase, pointIndex, barIndex, price, Brushes.Red);
            DrawWrapper.DrawText(ninjaScriptBase, pointIndex, barIndex, price, -15);
        }

        private void AddUnknow(double price, int barIndex, int pointIndex, Point.SideSwing sideSwing)
        {
            LogPrinter.Print(ninjaScriptBase, "Calculation.AddUnknow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            DrawWrapper.DrawDot(ninjaScriptBase, pointIndex, barIndex, price, Brushes.Gray);
        }

        private void UpdateHigh(double price, int barIndex)
        {
            LogPrinter.Print(ninjaScriptBase, "Calculation.UpdateHigh()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;

            DrawWrapper.DrawDot(ninjaScriptBase, temp.PointIndex, barIndex, price, Brushes.Green);
            DrawWrapper.DrawText(ninjaScriptBase, temp.PointIndex, barIndex, price, 15);
        }

        private void UpdateLow(double price, int barIndex)
        {
            LogPrinter.Print(ninjaScriptBase, "Calculation.UpdateLow()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;

            DrawWrapper.DrawDot(ninjaScriptBase, temp.PointIndex, barIndex, price, Brushes.Red);
            DrawWrapper.DrawText(ninjaScriptBase, temp.PointIndex, barIndex,price, -15);
        }

        // Miscellaneous

        protected struct CalculationData
        {
            public bool isNewSwing;
            public double price;
            public int barIndex;
            public Point.SideSwing sideSwing;

            /// <summary>
            /// Constructor used to pass informations
            /// </summary>
            /// <param name="isNewSwing"></param>
            /// <param name="price"></param>
            /// <param name="barIndex"></param>
            /// <param name="sideSwing"></param>
            public CalculationData(bool isNewSwing, double price, int barIndex, Point.SideSwing sideSwing)
            {
                this.isNewSwing = isNewSwing;
                this.price = price;
                this.barIndex = barIndex;
                this.sideSwing = sideSwing;
            }

            /// <summary>
            ///  This method constructor should only be used for situations where you do not have data to pass
            /// </summary>
            /// <param name="isNewSwing"></param>
            public CalculationData(bool isNewSwing)
            {
                if (isNewSwing)
                    throw new System.Exception("isNewSwing must be false");

                this.isNewSwing = isNewSwing;
                price = 0;
                barIndex = 0;
                sideSwing = Point.SideSwing.Unknow;
            }
        }

        private enum CalculationStage
        {
            FirstPoint,
            EachBarSwingPoint,
            EachTickSwingPoint
        }
    }
}
