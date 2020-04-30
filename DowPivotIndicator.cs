using System;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot;

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class DowPivotIndicator : Indicator
	{
        DowPivotClass dowPivot;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "Dow Pivot";
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
			}
			else if (State == State.Configure)
			{
			}
            else if(State == State.DataLoaded)
            {
                dowPivot = new DowPivotClass(this, CalculationType);

                // Everytime the F5 key is pressed automatically will clear the output window.
                // LogPrinter.ResetOuputTabs();
            }
		}

		protected override void OnBarUpdate()
		{
            dowPivot.Calculate();
		}

        #region Properties
        [NinjaScriptProperty]
        [Display(Name = "Calculation type", Order = 0, GroupName = "Parameters")]
        public CalculationTypeListDowPivot CalculationType
        { get; set; }
        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.DowPivotIndicator[] cacheDowPivotIndicator;
		public JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(CalculationTypeListDowPivot calculationType)
		{
			return DowPivotIndicator(Input, calculationType);
		}

		public JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input, CalculationTypeListDowPivot calculationType)
		{
			if (cacheDowPivotIndicator != null)
				for (int idx = 0; idx < cacheDowPivotIndicator.Length; idx++)
					if (cacheDowPivotIndicator[idx] != null && cacheDowPivotIndicator[idx].CalculationType == calculationType && cacheDowPivotIndicator[idx].EqualsInput(input))
						return cacheDowPivotIndicator[idx];
			return CacheIndicator<JiraiyaIndicators.DowPivotIndicator>(new JiraiyaIndicators.DowPivotIndicator(){ CalculationType = calculationType }, input, ref cacheDowPivotIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(CalculationTypeListDowPivot calculationType)
		{
			return indicator.DowPivotIndicator(Input, calculationType);
		}

		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input , CalculationTypeListDowPivot calculationType)
		{
			return indicator.DowPivotIndicator(input, calculationType);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(CalculationTypeListDowPivot calculationType)
		{
			return indicator.DowPivotIndicator(Input, calculationType);
		}

		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input , CalculationTypeListDowPivot calculationType)
		{
			return indicator.DowPivotIndicator(input, calculationType);
		}
	}
}

#endregion
