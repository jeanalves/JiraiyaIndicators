using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public abstract partial class Calculation
    {
        protected readonly PriceActionSwingLauncher launcher;
        protected Series<double> highs;
        protected Series<double> lows;
        protected List<Point> points = new List<Point>();
        private List<Tuple<double, int, int, int>> pointsListTuple = new List<Tuple<double, int, int, int>>();
        
        protected Calculation(PriceActionSwingLauncher launcher)
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

        public SideSwing LastSideTrend()
        {
            return points[points.Count - 1].sideSwing;
        }

        public double LastPrice()
        {
            return points[points.Count - 1].price;
        }

        public Point LastHigh()
        {
            if (points.Count > 0)
            {
                for (int i = points.Count - 1; i > 0; i--)
                    if (points[i].sideSwing == SideSwing.High)
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
                    if (points[i].sideSwing == SideSwing.Low)
                        return points[i];
            }
            else if (points.Count == 0)
            {
                throw new System.Exception();
            }

            return points[points.Count - 1];
        }

        public List<Tuple<double, int, int, int>> GetPointsList()
        {
            return pointsListTuple;
        }

        // Protected (methods)

        protected virtual CalculationData CalculateFirstSwingPoint()
        {
            launcher.PrintLog("Virtual Calculation.CalculateFirstSwingPoint()");

            SideSwing sideSwing = launcher.Close.GetValueAt(0) > launcher.Open.GetValueAt(0) ?
                SideSwing.Low : SideSwing.High;

            if (launcher.UseHighLow)
            {
                switch (sideSwing)
                {
                    case SideSwing.High:
                        return new CalculationData(true, launcher.High.GetValueAt(0), 0, sideSwing);

                    case SideSwing.Low:
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
            launcher.PrintLog("virtual Calculation.CalculateEachTickSwing()");

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

                    if (calculationData.sideSwing == SideSwing.High)
                    {
                        AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == SideSwing.Low)
                    {
                        AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == SideSwing.Unknow && points.Count == 0)
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
                ToDraw.DrawZigZagWrapper(launcher, points[points.Count - 2].pointIndex,
                    points[points.Count - 2].barIndex, points[points.Count - 2].price,
                    points[points.Count -1].barIndex, points[points.Count -1].price);
            }
        }

        private void DefaultAddUpdatePointsManagement(CalculationData calculationData)
        {
            if (calculationData.sideSwing == SideSwing.High && LastSideTrend() != SideSwing.High)
            {
                AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == SideSwing.Low && LastSideTrend() != SideSwing.Low)
            {
                AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == SideSwing.High && LastSideTrend() == SideSwing.High &&
                calculationData.price > LastPrice())
            {
                UpdateHigh(calculationData.price, calculationData.barIndex);
            }
            else if (calculationData.sideSwing == SideSwing.Low && LastSideTrend() == SideSwing.Low &&
                calculationData.price < LastPrice())
            {
                UpdateLow(calculationData.price, calculationData.barIndex);
            }
        }

        private void AddHigh(double price, int barIndex, int pointIndex, SideSwing sideSwing)
        {
            launcher.PrintLog("Calculation.AddHigh()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));
            AddToListTuple(price, barIndex, pointIndex, sideSwing);

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Green);
            ToDraw.DrawTextWrapper(launcher, pointIndex, barIndex, price, 15);
        }

        private void AddLow(double price, int barIndex, int pointIndex, SideSwing sideSwing)
        {
            launcher.PrintLog("Calculation.AddLow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));
            AddToListTuple(price, barIndex, pointIndex, sideSwing);

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Red);
            ToDraw.DrawTextWrapper(launcher, pointIndex, barIndex, price, -15);
        }

        private void AddUnknow(double price, int barIndex, int pointIndex, SideSwing sideSwing)
        {
            launcher.PrintLog("Calculation.AddUnknow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));

            ToDraw.DrawDotWrapper(launcher, pointIndex, barIndex, price, Brushes.Gray);
        }

        private void UpdateHigh(double price, int barIndex)
        {
            launcher.PrintLog("Calculation.UpdateHigh()");

            Point temp = points[points.Count - 1];

            temp.price = price;
            temp.barIndex = barIndex;

            points[points.Count - 1] = temp;

            ToDraw.DrawDotWrapper(launcher, temp.pointIndex, barIndex, price, Brushes.Green);
            ToDraw.DrawTextWrapper(launcher, temp.pointIndex, barIndex, price, 15);
        }

        private void UpdateLow(double price, int barIndex)
        {
            launcher.PrintLog("Calculation.UpdateLow()");

            Point temp = points[points.Count - 1];

            temp.price = price;
            temp.barIndex = barIndex;

            points[points.Count - 1] = temp;

            ToDraw.DrawDotWrapper(launcher, temp.pointIndex, barIndex, price, Brushes.Red);
            ToDraw.DrawTextWrapper(launcher, temp.pointIndex, barIndex,price, -15);
        }

        private void AddToListTuple(double price, int barIndex, int pointIndex, SideSwing sideSwing)
        {
            int sideSwingNumber = 0;

            if (sideSwing == SideSwing.High)
            {
                sideSwingNumber = 1;
            }
            else if (sideSwing == SideSwing.Low)
            {
                sideSwingNumber = -1;
            }

            pointsListTuple.Add(new Tuple<double, int, int, int>(price, barIndex, pointIndex, sideSwingNumber));
        }

        // Miscellaneous

        public struct Point
        {
            public double price;
            public int barIndex;
            public int pointIndex;
            public SideSwing sideSwing;

            public Point(double price, int barIndex, int pointIndex, SideSwing sideSwing)
            {
                this.price = price;
                this.barIndex = barIndex;
                this.pointIndex = pointIndex;
                this.sideSwing = sideSwing;
            }
        }

        protected struct CalculationData
        {
            public bool isNewSwing;
            public double price;
            public int barIndex;
            public SideSwing sideSwing;

            /// <summary>
            /// Constructor used to pass informations
            /// </summary>
            /// <param name="isNewSwing"></param>
            /// <param name="price"></param>
            /// <param name="barIndex"></param>
            /// <param name="sideSwing"></param>
            public CalculationData(bool isNewSwing, double price, int barIndex, SideSwing sideSwing)
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
                sideSwing = SideSwing.Unknow;
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
