using NinjaTrader.Custom.Indicators.JiraiyaIndicators;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwingOLD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public partial class PriceActionSwingOLD : Indicator
	{
        private TickCalculation tickCalculation;
        private SwingForwardCalculationOne swingForwardCalculationOne;
        private SwingForwardCalculationTwo swingForwardCalculationTwo;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "PriceActionSwingOLD";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= true;
				DisplayInDataBox							= false;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= false;
				DrawVerticalGridLines						= false;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				IsSuspendedWhileInactive					= true;

                CalculationType                             = CalculationTypeList.Tick;
				Strength					                = 5;
				UseHighLow					                = true;
                ShowLog                                     = true;
            }
			else if (State == State.Configure)
			{
			}
            else if (State == State.DataLoaded)
            {
                // Required reference to acess some indicator informations
                OwnerReference.PriceActionSwing = this;
                // Required reference to control chart graphical elements
                OwnerReference.NinjaScriptBase = this;

                if (!ShowLog)
                {
                    LogPrinter.SetIndicatorAsInvisible(this);
                }

                tickCalculation = new TickCalculation();
                swingForwardCalculationOne = new SwingForwardCalculationOne();
                swingForwardCalculationTwo = new SwingForwardCalculationTwo();
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
                        /*
                    case CalculationTypeList.SwingForwardOne:
                        swingForwardCalculationOne.Calculate();
                        break;

                    case CalculationTypeList.SwingForwardTwo:
                        swingForwardCalculationTwo.Calculate();
                        break;
                        */
                }
                
            }
            catch (Exception e)
            {
                Code.Output.Process(CurrentBar + "    " + e.ToString(), PrintTo.OutputTab2);
            }
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
		[Display(Name="Use HighLow", Order=2, GroupName="Parameters")]
		public bool UseHighLow
		{ get; set; }

        [Display(Name ="Show log in output window", Order = 3, GroupName = "Parameters")]
        public bool ShowLog
        { get; set; }

        /// <summary>
        /// This propertie returns the list data off zigzag points
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public List<Point> PointsList
        {   get
            {
                Update();

                switch (CalculationType)
                {
                    case CalculationTypeList.Tick:
                        return tickCalculation.GetPointsList();
                        /*
                    case CalculationTypeList.SwingForwardOne:
                        return swingForwardCalculationOne.GetPointsList();

                    case CalculationTypeList.SwingForwardTwo:
                        return swingForwardCalculationTwo.GetPointsList();       
                        */
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
                Update();

                switch (CalculationType)
                {
                    case CalculationTypeList.Tick:
                        return tickCalculation.GetLastPoint(0);
                        /*
                    case CalculationTypeList.SwingForwardOne:
                        return swingForwardCalculationOne.GetLastPoint(0);

                    case CalculationTypeList.SwingForwardTwo:
                        return swingForwardCalculationTwo.GetLastPoint(0);
                        */
                }
                return null;
            }
        }

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.PriceActionSwingOLD[] cachePriceActionSwingOLD;
		public JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return PriceActionSwingOLD(Input, calculationType, strength, useHighLow);
		}

		public JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(ISeries<double> input, CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			if (cachePriceActionSwingOLD != null)
				for (int idx = 0; idx < cachePriceActionSwingOLD.Length; idx++)
					if (cachePriceActionSwingOLD[idx] != null && cachePriceActionSwingOLD[idx].CalculationType == calculationType && cachePriceActionSwingOLD[idx].Strength == strength && cachePriceActionSwingOLD[idx].UseHighLow == useHighLow && cachePriceActionSwingOLD[idx].EqualsInput(input))
						return cachePriceActionSwingOLD[idx];
			return CacheIndicator<JiraiyaIndicators.PriceActionSwingOLD>(new JiraiyaIndicators.PriceActionSwingOLD(){ CalculationType = calculationType, Strength = strength, UseHighLow = useHighLow }, input, ref cachePriceActionSwingOLD);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingOLD(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingOLD(input, calculationType, strength, useHighLow);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingOLD(Input, calculationType, strength, useHighLow);
		}

		public Indicators.JiraiyaIndicators.PriceActionSwingOLD PriceActionSwingOLD(ISeries<double> input , CalculationTypeList calculationType, double strength, bool useHighLow)
		{
			return indicator.PriceActionSwingOLD(input, calculationType, strength, useHighLow);
		}
	}
}

#endregion
