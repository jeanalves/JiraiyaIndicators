#region Using declarations
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing;
using NinjaTrader.NinjaScript.DrawingTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class PriceActionSwingLauncher : Indicator
	{
        private TickCalculation tickCalculation;
        private SwingForwardCalculationOne swingForwardCalculationOne;
        private SwingForwardCalculationTwo swingForwardCalculationTwo;
        private Log log;

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
                log = new Log(this);

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
            log.Print(text);
        }

        public void PrintError(object text)
        {
            Draw.TextFixed(this, "Error", text.ToString(), TextPosition.BottomRight);
        }

        public int ConvertBarIndexToBarsAgo(int barIndex)
        {
            return (barIndex - CurrentBar) < 0 ? (barIndex - CurrentBar) * -1 : barIndex - CurrentBar;
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
        /// This propertie returns the list data in the following sequence
        /// double price, int barIndex, int pointIndex, int sideSwing,
        /// when side swing is equal 1 means a high point and -1 a low one
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public List<Tuple<double, int, int, int>> PointsList
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
		private JiraiyaIndicators.PriceActionSwingLauncher[] cachePriceActionSwingLauncher;
		public JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return PriceActionSwingLauncher(Input, calculationType, strength, useHighLow);
		}

		public JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(ISeries<double> input, CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			if (cachePriceActionSwingLauncher != null)
				for (int idx = 0; idx < cachePriceActionSwingLauncher.Length; idx++)
					if (cachePriceActionSwingLauncher[idx] != null && cachePriceActionSwingLauncher[idx].CalculationType == calculationType && cachePriceActionSwingLauncher[idx].Strength == strength && cachePriceActionSwingLauncher[idx].UseHighLow == useHighLow && cachePriceActionSwingLauncher[idx].EqualsInput(input))
						return cachePriceActionSwingLauncher[idx];
			return CacheIndicator<JiraiyaIndicators.PriceActionSwingLauncher>(new JiraiyaIndicators.PriceActionSwingLauncher(){ CalculationType = calculationType, Strength = strength, UseHighLow = useHighLow }, input, ref cachePriceActionSwingLauncher);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingLauncher(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingLauncher(input, calculationType, strength, useHighLow);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingLauncher(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwingLauncher PriceActionSwingLauncher(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingLauncher(input, calculationType, strength, useHighLow);
		}
	}
}

#endregion
