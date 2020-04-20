using NinjaTrader.NinjaScript;
using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public abstract class Calculation
    {
        // Fields

        protected Series<double> highs;
        protected Series<double> lows;
        protected List<Point> points = new List<Point>();

        protected readonly NinjaScriptBase owner;
        protected readonly PriceActionSwingClass priceActionSwingClass;

        private CalculationData calculationData = new CalculationData(false);
        private CalculationStage calculationStage = CalculationStage.FirstPoint;

        // Initialization

        protected Calculation(NinjaScriptBase owner, PriceActionSwingClass priceActionSwingClass)
        {
            this.owner = owner;
            this.priceActionSwingClass = priceActionSwingClass;

            highs = new Series<double>(owner);
            lows = new Series<double>(owner);
        }

        public void Calculate()
        {
            SetValues();

            calculationData = new CalculationData(false);

            if (points.Count == 0)
            {
                calculationData = CalculateFirstSwingPoint();
            }
            else if (owner.State == State.Historical)
            {
                calculationData = CalculateEachBarSwingPoint();
                calculationStage = CalculationStage.EachBarSwingPoint;
            }
            else if (owner.State == State.Realtime)
            {
                calculationData = CalculateEachTickSwingPoint();
                calculationStage = CalculationStage.EachTickSwingPoint;
            }

            if (calculationData.isNewSwing)
            {
                AddOrUpdatePointsIfNewSwing(calculationData, calculationStage);
            }
        }

        // Public (methods)

        public Point.SidePoint LastSideTrend()
        {
            return points[points.Count - 1].CurrentSideSwing;
        }

        public double LastPrice()
        {
            return points.Count < 0 ? 0 : points[points.Count - 1].Price;
        }

        public Point LastHigh()
        {
            for (int i = points.Count - 1; i > 0; i--)
            {
                if (points[i].CurrentSideSwing == Point.SidePoint.High)
                {
                    return points[i];
                }
            }

            return points[points.Count - 1];
        }

        public Point LastLow()
        {
            for (int i = points.Count - 1; i > 0; i--)
            {
                if (points[i].CurrentSideSwing == Point.SidePoint.Low)
                {
                    return points[i];
                }
            }

            return points[points.Count - 1];
        }

        public List<Point> GetPointsList()
        {
            return points;
        }

        public Point GetPoint(int pointsAgo)
        {
            return points.Count < pointsAgo + 1 ? null : points[(points.Count - 1) - pointsAgo];
        }

        // Protected (methods)

        protected virtual CalculationData CalculateFirstSwingPoint()
        {
            //logPrinter.Print(owner, "Virtual Calculation.CalculateFirstSwingPoint()");

            Point.SidePoint sideSwing = owner.Close.GetValueAt(0) > owner.Open.GetValueAt(0) ?
                Point.SidePoint.Low : Point.SidePoint.High;

            if (priceActionSwingClass.UseHighLow)
            {
                switch (sideSwing)
                {
                    case Point.SidePoint.High:
                        return new CalculationData(true, owner.High.GetValueAt(0), 0, sideSwing);

                    case Point.SidePoint.Low:
                        return new CalculationData(true, owner.Low.GetValueAt(0), 0, sideSwing);
                }
            }
            else
            {
                return new CalculationData(true, owner.Open.GetValueAt(0), 0, sideSwing);
            }

            return new CalculationData(false);
        }

        protected abstract CalculationData CalculateEachBarSwingPoint();

        protected virtual CalculationData CalculateEachTickSwingPoint()
        {
            //logPrinter.Print(owner, "virtual Calculation.CalculateEachTickSwing()");

            return new CalculationData(false);
        }

        // Private (methods)

        private void SetValues()
        {
            if (priceActionSwingClass.UseHighLow)
            {
                highs[0] = owner.High[0];
                lows[0] = owner.Low[0];
            }
            else
            {
                highs[0] = owner.Close[0];
                lows[0] = owner.Close[0];
            }
        }

        private void AddOrUpdatePointsIfNewSwing(CalculationData calculationData, CalculationStage calculationStage)
        {
            switch (calculationStage)
            {
                case CalculationStage.FirstPoint:

                    if (calculationData.sideSwing == Point.SidePoint.High)
                    {
                        AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == Point.SidePoint.Low)
                    {
                        AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
                    }
                    else if (calculationData.sideSwing == Point.SidePoint.Unknow && points.Count == 0)
                    {
                        AddUnknow(owner.Open.GetValueAt(0), 0, points.Count, calculationData.sideSwing);
                    }

                    break;

                case CalculationStage.EachBarSwingPoint:

                    DefaultAddUpdatePointsManagement(calculationData);

                    break;

                case CalculationStage.EachTickSwingPoint:

                    DefaultAddUpdatePointsManagement(calculationData);

                    break;
            }
        }

        private void DefaultAddUpdatePointsManagement(CalculationData calculationData)
        {
            if (calculationData.sideSwing == Point.SidePoint.High && LastSideTrend() != Point.SidePoint.High)
            {
                AddHigh(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == Point.SidePoint.Low && LastSideTrend() != Point.SidePoint.Low)
            {
                AddLow(calculationData.price, calculationData.barIndex, points.Count, calculationData.sideSwing);
            }
            else if (calculationData.sideSwing == Point.SidePoint.High && LastSideTrend() == Point.SidePoint.High &&
                calculationData.price > LastPrice())
            {
                UpdateHigh(calculationData.price, calculationData.barIndex);
            }
            else if (calculationData.sideSwing == Point.SidePoint.Low && LastSideTrend() == Point.SidePoint.Low &&
                calculationData.price < LastPrice())
            {
                UpdateLow(calculationData.price, calculationData.barIndex);
            }
        }

        private void AddHigh(double price, int barIndex, int pointIndex, Point.SidePoint sideSwing)
        {
            //logPrinter.Print(owner, "Calculation.AddHigh()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));
        }

        private void AddLow(double price, int barIndex, int pointIndex, Point.SidePoint sideSwing)
        {
            //logPrinter.Print(owner, "Calculation.AddLow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));
        }

        private void AddUnknow(double price, int barIndex, int pointIndex, Point.SidePoint sideSwing)
        {
            //logPrinter.Print(owner, "Calculation.AddUnknow()");

            points.Add(new Point(price, barIndex, pointIndex, sideSwing));
        }

        private void UpdateHigh(double price, int barIndex)
        {
            //logPrinter.Print(owner, "Calculation.UpdateHigh()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;
        }

        private void UpdateLow(double price, int barIndex)
        {
            //logPrinter.Print(owner, "Calculation.UpdateLow()");

            Point temp = points[points.Count - 1];

            temp.Price = price;
            temp.BarIndex = barIndex;

            points[points.Count - 1] = temp;
        }

        // Properties

        public CalculationData CalcData
        {
            get
            {
                return calculationData;
            }
        }

        public CalculationStage CalcStage
        {
            get
            {
                return calculationStage;
            }
        }

        // Miscellaneous

        public struct CalculationData
        {
            public bool isNewSwing;
            public double price;
            public int barIndex;
            public Point.SidePoint sideSwing;

            /// <summary>
            /// Constructor used to pass informations
            /// </summary>
            /// <param name="isNewSwing"></param>
            /// <param name="price"></param>
            /// <param name="barIndex"></param>
            /// <param name="sideSwing"></param>
            public CalculationData(bool isNewSwing, double price, int barIndex, Point.SidePoint sideSwing)
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
                sideSwing = Point.SidePoint.Unknow;
            }
        }

        public enum CalculationStage
        {
            FirstPoint,
            EachBarSwingPoint,
            EachTickSwingPoint
        }
    }
}
