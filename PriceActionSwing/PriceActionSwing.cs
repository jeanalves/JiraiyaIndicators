using NinjaTrader.Custom.Indicators.JiraiyaIndicators;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript.DrawingTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class PriceActionSwing : Indicator
	{
        private TickCalculation tickCalculation;
        private SwingForwardCalculationOne swingForwardCalculationOne;
        private SwingForwardCalculationTwo swingForwardCalculationTwo;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "PriceActionSwing";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= true;
				DisplayInDataBox							= false;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= false;
				DrawVerticalGridLines						= false;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				IsSuspendedWhileInactive					= true;

                CalculationType                             = CalculationTypeList.SwingForwardOne;
				Strength					                = 5;
				UseHighLow					                = true;
			}
			else if (State == State.Configure)
			{
			}
            else if (State == State.DataLoaded)
            {
                tickCalculation = new TickCalculation(this);
                swingForwardCalculationOne = new SwingForwardCalculationOne(this);
                swingForwardCalculationTwo = new SwingForwardCalculationTwo(this);
                LogPrinter.SetLauncher(this);

                // Everytime the F5 key is pressed automatically will clear the output window.
                Code.Output.Reset(PrintTo.OutputTab1);
                Code.Output.Reset(PrintTo.OutputTab2);
            }
        }

		protected override void OnBarUpdate()
		{
            try
            {
                switch(CalculationType)
                {
                    case CalculationTypeList.Tick:
                        tickCalculation.Calculate();
                        break;

                    case CalculationTypeList.SwingForwardOne:
                        swingForwardCalculationOne.Calculate();
                        break;

                    case CalculationTypeList.SwingForwardTwo:
                        swingForwardCalculationTwo.Calculate();
                        break;
                }
                
            }
            catch (Exception e)
            {
                Code.Output.Process(CurrentBar + "    " + e.ToString(), PrintTo.OutputTab2);
            }
		}

        public void PrintLog(object text)
        {
            LogPrinter.Print(text);
        }

        #region Properties

        [NinjaScriptProperty]
        [Display(Name = "Calculation type", Order = 0, GroupName = "Parameters")]
        public CalculationTypeList CalculationType
        { get; set; }

        [NinjaScriptProperty]
		[Range(0.1, double.MaxValue)]
		[Display(Name="Strength", Order=1, GroupName="Parameters")]
		public double Strength
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="UseHighLow", Order=2, GroupName="Parameters")]
		public bool UseHighLow
		{ get; set; }

        /// <summary>
        /// This propertie returns the list data off zigzag points
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public List<Point> PointsList
        {   get
            {
                switch (CalculationType)
                {
                    case CalculationTypeList.Tick:
                        return tickCalculation.GetPointsList();

                    case CalculationTypeList.SwingForwardOne:
                        return swingForwardCalculationOne.GetPointsList();

                    case CalculationTypeList.SwingForwardTwo:
                        return swingForwardCalculationTwo.GetPointsList();                        
                }
                return null;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public Point LastPoint
        {
            get
            {
                switch (CalculationType)
                {
                    case CalculationTypeList.Tick:
                        return tickCalculation.GetLastPoint();

                    case CalculationTypeList.SwingForwardOne:
                        return swingForwardCalculationOne.GetLastPoint();

                    case CalculationTypeList.SwingForwardTwo:
                        return swingForwardCalculationTwo.GetLastPoint();
                }
                return null;
            }
        }


        #endregion
    }
}

public enum CalculationTypeList
{
    Tick,
    SwingForwardOne,
    SwingForwardTwo
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.PriceActionSwing[] cachePriceActionSwing;
		public JiraiyaIndicators.PriceActionSwing PriceActionSwing(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return PriceActionSwing(Input, calculationType, strength, useHighLow);
		}

		public JiraiyaIndicators.PriceActionSwing PriceActionSwing(ISeries<double> input, CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			if (cachePriceActionSwing != null)
				for (int idx = 0; idx < cachePriceActionSwing.Length; idx++)
					if (cachePriceActionSwing[idx] != null && cachePriceActionSwing[idx].CalculationType == calculationType && cachePriceActionSwing[idx].Strength == strength && cachePriceActionSwing[idx].UseHighLow == useHighLow && cachePriceActionSwing[idx].EqualsInput(input))
						return cachePriceActionSwing[idx];
			return CacheIndicator<JiraiyaIndicators.PriceActionSwing>(new JiraiyaIndicators.PriceActionSwing(){ CalculationType = calculationType, Strength = strength, UseHighLow = useHighLow }, input, ref cachePriceActionSwing);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwing PriceActionSwing(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwing(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwing PriceActionSwing(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwing(input, calculationType, strength, useHighLow);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwing PriceActionSwing(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwing(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwing PriceActionSwing(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwing(input, calculationType, strength, useHighLow);
		}
	}
}

#endregion
