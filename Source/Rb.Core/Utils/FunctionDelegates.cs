
namespace Rb.Core.Utils
{
	public static class FunctionDelegates
	{
		public delegate R Function< R >( );
		public delegate R Function< R, P0 >( P0 p0 );
		public delegate R Function< R, P0, P1 >( P0 p0, P1 p1 );
		public delegate R Function< R, P0, P1, P2 >( P0 p0, P1 p1, P2 p2 );
		public delegate R Function< R, P0, P1, P2, P3 >( P0 p0, P1 p1, P2 p2, P3 p3 );
		public delegate R Function< R, P0, P1, P2, P3, P4 >( P0 p0, P1 p1, P2 p2, P3 p3, P4 p4 );
		public delegate R Function< R, P0, P1, P2, P3, P4, P5 >( P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5 );
	}

	public static class ActionDelegates
	{
		public delegate void Action( );
		public delegate void Action<P0>(P0 p0);
		public delegate void Action<P0, P1>(P0 p0, P1 p1);
		public delegate void Action<P0, P1, P2>(P0 p0, P1 p1, P2 p2);
		public delegate void Action<P0, P1, P2, P3>(P0 p0, P1 p1, P2 p2, P3 p3);
		public delegate void Action<P0, P1, P2, P3, P4>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
		public delegate void Action<P0, P1, P2, P3, P4, P5>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
	}

}
