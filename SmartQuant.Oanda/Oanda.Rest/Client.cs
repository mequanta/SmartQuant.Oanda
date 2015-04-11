namespace Oanda.Rest
{
	public enum EServer
	{
		Account,
		Rates,
		StreamingRates,
		StreamingEvents,
		Labs
	}
		
	public enum EEnvironment
	{
		Sandbox,
		Practice,
		Trade
	}

	public class Granularity
	{
		public const string S5 = "S5";
		public const string S10 = "S10";
		public const string S15 = "S15";
		public const string S30 = "S30";
		public const string M1 = "M1";
		public const string M2 = "M2";
		public const string M3 = "M3";
		public const string M4 = "M4";
		public const string M5 = "M5";
		public const string M10 = "M10";
		public const string M15 = "M15";
		public const string M30 = "M30";
		public const string H1 = "H1";
		public const string H2 = "H2";
		public const string H3 = "H3";
		public const string H4 = "H4";
		public const string H6 = "H6";
		public const string H8 = "H8";
		public const string H12 = "H12";
		public const string D = "D";
		public const string W = "W";
		public const string M = "M";
	}

	public partial class Client
    {
    }
}
