
namespace Poc1.Universe
{
	public static class UniUnits
	{
		public const double MetresToUnits = 1000;
		public const double UnitsToMetres = 1.0 / MetresToUnits;

		public static long FromMetres( double metres )
		{
			return ( long )( metres * MetresToUnits );
		}

		public static double ToMetres( long units )
		{
			return units * UnitsToMetres;
		}
	}
}
