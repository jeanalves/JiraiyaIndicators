using NinjaTrader.NinjaScript;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public abstract partial class Calculation
    {
        protected readonly NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher;
        protected Series<double> highs;
        protected Series<double> lows;
        protected List<Point> points = new List<Point>();
        
        protected Calculation(NinjaScript.Indicators.JiraiyaIndicators.PriceActionSwing launcher)
        {
            this.launcher = launcher;
            highs = new Series<double>(launcher);
            lows = new Series<double>(launcher);
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
            else if (launcher.State == State.Historical)
            {
                calculationData = CalculateEachBarSwingPoint();
                calculationStage = CalculationStage.EachBarSwingPoint;
            }
            else if (launcher.State == State.Realtime)
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
            LogPrinter.Print("Virtual Calculation.CalculateFirstSwingPoint()");

            Point.SideSwing sideSwing = launcher.Close.GetValueAt(0) > launcher.Open.GetValueAt(0) ?
                Point.SideSwing.Low : Point.SideSwing.High;

            if (launcher.UseHighLow)
            {
                switch (sideSwing)
                {
                    case Point.SideSwing.High:
                        return new CalculationData(true, launcher.High.GetValueAt(0), 0, sideSwing);

                    case Point.SideSwing.Low:
                        return new CalculationData(true, launcher.Low.GetValueAt(0), 0, sideSwing);
                }
            }
            else
            {
                return new CalculationData(true, launcher.Open.GetValueAt(0), 0, sideSwing);
            }

            return new CalculationData(false);
        }

        protected abstract CalculationData CalculateEachBarSwingPoint();

        protected virtual CalculationData CalculateEachTickSwing()
        {
            LogPrinter.Print("virtual Calculation.CalculateEachTickSwing()");

            return new CalculationData(false);
        }

        // Private (methods)

        private void SetValues()
        {
            if (launcher.UseHighLow)
            {
                highs[0] = launcher.High[0];
                lows[0] = launcher.Low[0];
            }
            else
            {
                highs[0] = launcher.Close[0];
                lows[0] = launcher.Close[0];
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
                        AddUnknow(launcher.Open.GetValueAt(0), 0, points.Count, calculationData.sideSwing);
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
                ToDraw.DrawZigZagWrapper(launcher, points[points.Count - 2].PointIndex,
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
            LogPrinter.Print("Calculation.AddHigh()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Green);
            ToDraw.DrawTextWrapper(launcher, pointIndex, barIndex, price, 15);
        }

        private void AddLow(double price, int barIndex, int pointIndex, Point.SideSwing sideSwing)
        {
            LogPrinter.Print("Calculation.AddLow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Red);
            ToDraw.DrawTextWrapper(launcher, pointIndex, barIndex, price, -15);
        }

        private void AddUnknow(double price, int barIndex, int pointIndex, Point.SideSwing sideSwing)
        {
            LogPrinter.Print("Calculation.AddUnknow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Gray);
        }

        private void UpdateHigh(double price, int barIndex)
        {
            LogPrinter.Print("Calculation.UpdateHigh()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;

            ToDraw.DrawDotWrapper(launcher, temp.PointIndex, barIndex, price, Brushes.Green);
            ToDraw.DrawTextWrapper(launcher, temp.PointIndex, barIndex, price, 15);
        }

        private void UpdateLow(double price, int barIndex)
        {
            LogPrinter.Print("Calculation.UpdateLow()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;

            ToDraw.DrawDotWrapper(launcher, temp.PointIndex, barIndex, price, Brushes.Red);
            ToDraw.DrawTextWrapper(launcher, temp.PointIndex, barIndex,price, -15);
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
