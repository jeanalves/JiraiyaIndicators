using NinjaTrader.Custom.Indicators.JiraiyaIndicators.DowPivot;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators;

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class DowPivotIndicator : Indicator
	{
        DowPivot dowPivot;

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
                dowPivot = new DowPivot(this, true);

                // Everytime the F5 key is pressed automatically will clear the output window.
                LogPrinter.ResetOuputTabs();
            }
		}

		protected override void OnBarUpdate()
		{
            dowPivot.Calculate();
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.DowPivotIndicator[] cacheDowPivotIndicator;
		public JiraiyaIndicators.DowPivotIndicator DowPivotIndicator()
		{
			return DowPivotIndicator(Input);
		}

		public JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input)
		{
			if (cacheDowPivotIndicator != null)
				for (int idx = 0; idx < cacheDowPivotIndicator.Length; idx++)
					if (cacheDowPivotIndicator[idx] != null &&  cacheDowPivotIndicator[idx].EqualsInput(input))
						return cacheDowPivotIndicator[idx];
			return CacheIndicator<JiraiyaIndicators.DowPivotIndicator>(new JiraiyaIndicators.DowPivotIndicator(), input, ref cacheDowPivotIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator()
		{
			return indicator.DowPivotIndicator(Input);
		}

		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input )
		{
			return indicator.DowPivotIndicator(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator()
		{
			return indicator.DowPivotIndicator(Input);
		}

		public Indicators.JiraiyaIndicators.DowPivotIndicator DowPivotIndicator(ISeries<double> input )
		{
			return indicator.DowPivotIndicator(input);
		}
	}
}

#endregion
