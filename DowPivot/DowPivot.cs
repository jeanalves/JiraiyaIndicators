//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
	public class DowPivot : Indicator
	{
        private PriceActionSwing priceActionSwing;

        protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "DowPivot";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= false;
				DrawVerticalGridLines						= false;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				IsSuspendedWhileInactive					= true;
			}
			else if (State == State.Configure)
			{
			}
            else if(State == State.DataLoaded)
            {
                priceActionSwing = PriceActionSwing(CalculationTypeList.SwingForwardOne, 5, true);
            }
		}

		protected override void OnBarUpdate()
		{
            
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private JiraiyaIndicators.DowPivot[] cacheDowPivot;
		public JiraiyaIndicators.DowPivot DowPivot()
		{
			return DowPivot(Input);
		}

		public JiraiyaIndicators.DowPivot DowPivot(ISeries<double> input)
		{
			if (cacheDowPivot != null)
				for (int idx = 0; idx < cacheDowPivot.Length; idx++)
					if (cacheDowPivot[idx] != null &&  cacheDowPivot[idx].EqualsInput(input))
						return cacheDowPivot[idx];
			return CacheIndicator<JiraiyaIndicators.DowPivot>(new JiraiyaIndicators.DowPivot(), input, ref cacheDowPivot);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.JiraiyaIndicators.DowPivot DowPivot()
		{
			return indicator.DowPivot(Input);
		}

		public Indicators.JiraiyaIndicators.DowPivot DowPivot(ISeries<double> input )
		{
			return indicator.DowPivot(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.JiraiyaIndicators.DowPivot DowPivot()
		{
			return indicator.DowPivot(Input);
		}

		public Indicators.JiraiyaIndicators.DowPivot DowPivot(ISeries<double> input )
		{
			return indicator.DowPivot(input);
		}
	}
}

#endregion
