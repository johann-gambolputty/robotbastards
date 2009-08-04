using System;

namespace Goo.Test
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			new TestHost( ).Run( new TestUnit( ) );
		}
	}
}