using System;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot;

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class DowTheoryIndicator : Indicator
	{
        DowTheoryClass dowTheory;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "Dow Theory";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= false;
				DrawVerticalGridLines						= false;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
                CalculationType                             = CalculationTypeListDowTheory.Pivot;
                Strength                                    = 2;
			}
			else if (State == State.Configure)
			{
			}
            else if(State == State.DataLoaded)
            {
                dowTheory = new DowTheoryClass(this, CalculationType, Strength);

                // Everytime the F5 key is pressed automatically will clear the output window.
                // LogPrinter.ResetOuputTabs();
            }
		}

		protected override void OnBarUpdate()
		{
            dowTheory.Compute();
		}

        #region Properties
        [NinjaScriptProperty]
        [Display(Name = "Calculation type", Order = 0, GroupName = "Parameters")]
        public CalculationTypeListDowTheory CalculationType
        { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Strengh", Order = 1, GroupName = "Parameters")]
        public double Strength { get; set; }
        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.DowTheoryIndicator[] cacheDowTheoryIndicator;
		public JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(CalculationTypeListDowTheory calculationType, double strength)
		{
			return DowTheoryIndicator(Input, calculationType, strength);
		}

		public JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(ISeries<double> input, CalculationTypeListDowTheory calculationType, double strength)
		{
			if (cacheDowTheoryIndicator != null)
				for (int idx = 0; idx < cacheDowTheoryIndicator.Length; idx++)
					if (cacheDowTheoryIndicator[idx] != null && cacheDowTheoryIndicator[idx].CalculationType == calculationType && cacheDowTheoryIndicator[idx].Strength == strength && cacheDowTheoryIndicator[idx].EqualsInput(input))
						return cacheDowTheoryIndicator[idx];
			return CacheIndicator<JiraiyaIndicators.DowTheoryIndicator>(new JiraiyaIndicators.DowTheoryIndicator(){ CalculationType = calculationType, Strength = strength }, input, ref cacheDowTheoryIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(CalculationTypeListDowTheory calculationType, double strength)
		{
			return indicator.DowTheoryIndicator(Input, calculationType, strength);
		}

		public Indicators.JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(ISeries<double> input , CalculationTypeListDowTheory calculationType, double strength)
		{
			return indicator.DowTheoryIndicator(input, calculationType, strength);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(CalculationTypeListDowTheory calculationType, double strength)
		{
			return indicator.DowTheoryIndicator(Input, calculationType, strength);
		}

		public Indicators.JiraiyaIndicators.DowTheoryIndicator DowTheoryIndicator(ISeries<double> input , CalculationTypeListDowTheory calculationType, double strength)
		{
			return indicator.DowTheoryIndicator(input, calculationType, strength);
		}
	}
}

#endregion
