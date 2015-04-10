using SmartQuant;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace SmartQuant.Oanda
{
//	public static class SmartQuantExtensions
//	{
//		public static List<OANDARestLibrary.TradeLibrary.DataTypes.Instrument> ToOandaRestInstrumentList(this InstrumentList instruments)
//		{
//			var list = new List<OANDARestLibrary.TradeLibrary.DataTypes.Instrument> (instruments.Count);
//			foreach (var i in instruments)
//				list.Add (i.ToOandaRestInstrument ());
//			return list;
//		}
//
//		public static OANDARestLibrary.TradeLibrary.DataTypes.Instrument ToOandaRestInstrument(this Instrument instrument)
//		{
//			var i = new OANDARestLibrary.TradeLibrary.DataTypes.Instrument () {
//				instrument = instrument.Symbol
//			};
//			return i;
//		}
//	}
//
	public class OandaRest : Provider, IExecutionProvider, IHistoricalDataProvider, IInstrumentProvider, IDataProvider
	{
		private Timer marketDataTimer;
		private Timer executionTimer;
		private InstrumentList subscribbed;
//		private RatesSession rSession;
//		private EventsSession eSession;
//
		[Category ("Credentials - AccountId")]
		public string AccountId { get; set; }

		[Category ("Credentials - AccessToken")]
		public string AccessToken { get; set; }

//		[Category ("Credentials - Environment")]
//		public EEnvironment Environment { get; set; }
//
		public OandaRest (Framework framework)
			: base (framework)
		{
			this.id = 46;
			this.name = "Oanda Rest";
			this.description = "SmartQuant provider for Oanda Rest API";
			this.url = "http://www.oanda.com";

			this.marketDataTimer = new Timer (new TimerCallback (OnMarketDataTimer));
			this.executionTimer =  new Timer (new TimerCallback (OnExecutionTimer));
		}

		public override void Connect ()
		{
			base.Connect ();
//			rSession = new RatesSession (int.Parse(AccountId), subscribbed.ToOandaRestInstrumentList ());
		}

		public override void Disconnect()
		{
			base.Disconnect ();
		}

		void IHistoricalDataProvider.Cancel (string requestId)
		{
		}

		void IInstrumentProvider.Cancel (string requestId)
		{
		}

		public override void Subscribe (Instrument instrument)
		{
			if (!subscribbed.Contains (instrument)) {
				subscribbed.Add (instrument);
			}
		}

		public override void Unsubscribe (Instrument instrument)
		{
			if (subscribbed.Contains (instrument)) {
				subscribbed.Remove(instrument);
			}	
		}
		private void OnMarketDataTimer(object stateInfo)
		{
		}

		private void OnExecutionTimer(object stateInfo)
		{
		}
	}
}

