using System;
using SmartQuant;
using System.IO;
using System.Reflection;
using System.Globalization;
using SmartQuant.Indicators;
using System.Drawing;

namespace MyDemo
{
	public class MyStrategy : InstrumentStrategy
	{
		private SMA fastSMA;
		private SMA slowSMA;
		private Order enterOrder;
		private Order takeProfitOrder;
		private Order stopLossOrder;
		private Group barsGroup;
		private Group fillGroup;
		private Group equityGroup;
		private Group fastSmaGroup;
		private Group slowSmaGroup;

		[Parameter]
		public double AllocationPerInstrument = 100000;

		[Parameter]
		double Qty = 100;

		[Parameter]
		int FastSMALength = 8;

		[Parameter]
		int SlowSMALength = 21;

		[Parameter]
		double TakeProfit = 0.01;

		[Parameter]
		double StopLoss = 0.01;

		public MyStrategy (Framework framework, string name)
			: base (framework, name)
		{
		}

		protected override void OnStrategyStart ()
		{
			Portfolio.Account.Deposit (AllocationPerInstrument, CurrencyId.USD, "Initial allocation");

			// Set up indicators.
			fastSMA = new SMA (Bars, FastSMALength);
			slowSMA = new SMA (Bars, SlowSMALength);

			AddGroups ();
		}

		protected override void OnBar (Instrument instrument, Bar bar)
		{
			// Add bar to bar series.
			Bars.Add (bar);

			Log (bar, barsGroup);

			if (fastSMA.Count > 0)
				Log (fastSMA.Last, fastSmaGroup);

			if (slowSMA.Count > 0)
				Log (slowSMA.Last, slowSmaGroup);

			// Calculate performance.
			Portfolio.Performance.Update ();

			// Add equity to group.
			Log (Portfolio.Value, equityGroup);

			// Check strategy logic.
			if (fastSMA.Count > 0 && slowSMA.Count > 0) {
				Cross cross = fastSMA.Crosses (slowSMA, bar.DateTime);

				// Enter long/short.
				if (!HasPosition (instrument)) {
					if (cross == Cross.Above) {
						enterOrder = BuyOrder (Instrument, Qty, "Enter Long");
						Send (enterOrder);
					} else if (cross == Cross.Below) {
						enterOrder = SellOrder (Instrument, Qty, "Enter Short");
						Send (enterOrder);
					}
				}
			}
		}

		protected override void OnFill (Fill fill)
		{
			Log (fill, fillGroup);
		}

		protected override void OnOrderPartiallyFilled (Order order)
		{
			// Update take profit order.
			if (order == stopLossOrder) {
				// Get take profit price and updated qty.
				double takeProfitPrice = takeProfitOrder.Price;
				double qty = Math.Abs (Position.Amount);

				// Cancel existing take profit order.
				Cancel (takeProfitOrder);

				if (Position.Side == PositionSide.Long) {
					// Create updated order and send it.
					takeProfitOrder = SellLimitOrder (Instrument, qty, takeProfitPrice, "TakeProfit");
					Send (takeProfitOrder);
				} else {
					// Create updated order and send it.
					takeProfitOrder = BuyLimitOrder (Instrument, qty, takeProfitPrice, "TakeProfit");
					Send (takeProfitOrder);
				}
			}
			// Update stop loss order.
			else if (order == takeProfitOrder) {
				// Get take profit price and updated qty.
				double stopLossPrice = stopLossOrder.Price;
				double qty = Math.Abs (Position.Amount);

				// Cancel existing stop loss order.
				Cancel (stopLossOrder);

				if (Position.Side == PositionSide.Long) {
					// Create updated order and send it.
					stopLossOrder = SellStopOrder (Instrument, qty, stopLossPrice, "StopLoss");
					Send (stopLossOrder);
				} else {
					// Create updated order and send it.
					stopLossOrder = BuyStopOrder (Instrument, qty, stopLossPrice, "StopLoss");
					Send (stopLossOrder);
				}
			}
		}

		protected override void OnOrderFilled (Order order)
		{
			if (order == enterOrder) {
				// Send take profit and stop loss orders.
				if (Position.Side == PositionSide.Long) {
					// Calculate prices.
					double takeProfitPrice = Position.EntryPrice * (1 + TakeProfit);
					double stopLossPrice = Position.EntryPrice * (1 - StopLoss);

					// Create orders.
					takeProfitOrder = SellLimitOrder (Instrument, Qty, takeProfitPrice, "TakeProfit");
					stopLossOrder = SellStopOrder (Instrument, Qty, stopLossPrice, "StopLoss");

					// Send orders.
					Send (stopLossOrder);
					Send (takeProfitOrder);
				} else {
					// Calculate prices.
					double takeProfitPrice = Position.EntryPrice * (1 - TakeProfit);
					double stopLossPrice = Position.EntryPrice * (1 + StopLoss);

					// Create orders.
					takeProfitOrder = BuyLimitOrder (Instrument, Qty, takeProfitPrice, "TakeProfit");
					stopLossOrder = BuyStopOrder (Instrument, Qty, stopLossPrice, "StopLoss");

					// Send orders.
					Send (stopLossOrder);
					Send (takeProfitOrder);
				}
			} else if (order == stopLossOrder) {
				// Cancel take profit order.
				if (!takeProfitOrder.IsDone)
					Cancel (takeProfitOrder);
			} else if (order == takeProfitOrder) {
				// Cancel stop loss order.
				if (!stopLossOrder.IsDone)
					Cancel (stopLossOrder);
			}
		}

		private void AddGroups ()
		{
			// Create bars group.
			barsGroup = new Group ("Bars");
			barsGroup.Add ("Pad", DataObjectType.String, 0);
			barsGroup.Add ("SelectorKey", Instrument.Symbol);

			// Create fills group.
			fillGroup = new Group ("Fills");
			fillGroup.Add ("Pad", 0);
			fillGroup.Add ("SelectorKey", Instrument.Symbol);

			// Create equity group.
			equityGroup = new Group ("Equity");
			equityGroup.Add ("Pad", 1);
			equityGroup.Add ("SelectorKey", Instrument.Symbol);

			// Create fast sma values group.
			fastSmaGroup = new Group ("FastSMA");
			fastSmaGroup.Add ("Pad", 0);
			fastSmaGroup.Add ("SelectorKey", Instrument.Symbol);
			fastSmaGroup.Add ("Color", Color.Green);

			// Create slow sma values group.
			slowSmaGroup = new Group ("SlowSMA");
			slowSmaGroup.Add ("Pad", 0);
			slowSmaGroup.Add ("SelectorKey", Instrument.Symbol);
			slowSmaGroup.Add ("Color", Color.Red);

			// Add groups to manager.
			GroupManager.Add (barsGroup);
			GroupManager.Add (fillGroup);
			GroupManager.Add (equityGroup);
			GroupManager.Add (fastSmaGroup);
			GroupManager.Add (slowSmaGroup);
		}
	}

	public class Backtest : Scenario
	{
		private const string symbol = "EUR_USD";

		private long barSize = 60;

		public Backtest (Framework framework)
			: base (framework)
		{
		}

		public override void Run ()
		{
			var instrument = InstrumentManager.Instruments [symbol];
			strategy = new MyStrategy (framework, "SMACrossoverTPSL");
			strategy.AddInstrument (instrument);
			DataSimulator.DateTime1 = new DateTime (2015, 01, 01);
			DataSimulator.DateTime2 = new DateTime (2015, 12, 31);
			BarFactory.Add (instrument, BarType.Time, barSize);
			StartStrategy ();
		}
	}

	class MainClass
	{
		private const string symbol = "EUR_USD";

		public static void Main (string[] args)
		{
			EnsureDataReady ();
			new Backtest (Framework.Current).Run ();
		}

		private static void EnsureDataReady ()
		{
			var f = Framework.Current;
			var i = f.InstrumentManager.Get (symbol);
			if (i == null) {
				i = new Instrument (InstrumentType.FX, symbol, string.Empty, CurrencyId.USD);
				f.InstrumentManager.Add (i, true);
			}
				
			var filename = Path.GetFullPath (Path.Combine (Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location), "..", "..", "..", "Data", string.Format ("{0}.csv", symbol)));
			if (File.Exists (filename)) {
				var name = DataSeriesNameHelper.GetName (i, BarType.Time, 60);
				var bs = new BarSeries (name);
				using (var reader = new StreamReader (filename)) {
					var line = reader.ReadLine ();
					while (line != null) {
						var fields = line.Split (new char[]{ ',' });
						var closeDateTime = DateTime.ParseExact (string.Format ("{0} {1}", fields [0], fields [1]), "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
						var openDateTime = closeDateTime.Subtract (TimeSpan.FromMinutes (1));
						var open = double.Parse (fields [2]);
						var high = double.Parse (fields [3]);
						var low = double.Parse (fields [4]);
						var close = double.Parse (fields [5]);
						var vol = long.Parse (fields [6]);
						bs.Add (new Bar (openDateTime, closeDateTime, i.Id, BarType.Time, 60, open, high, low, close, vol, 0));
						line = reader.ReadLine ();
					}
				}
				f.DataManager.Save (bs, SaveMode.Add);
			}			
		}
	}
}
