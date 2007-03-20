using System;
using System.Collections;

namespace RbEngine
{
	/// <summary>
	/// Works as a TextWriterTraceListener, except that the output file is backed up and emptied
	/// </summary>
	public class NiceTextWriterTraceListener : System.Diagnostics.TextWriterTraceListener
	{
		/// <summary>
		/// Sets up the output file
		/// </summary>
		/// <param name="outputFilePath">Path to the output file</param>
		public NiceTextWriterTraceListener( string outputFilePath )
		{
			DateTime	now				= DateTime.Now;
			string		directory		= string.Format( "Outputs {0}-{1}-{2}", now.Year, now.Month, now.Day );
			if ( !System.IO.Directory.Exists( directory ) )
			{
				System.IO.Directory.CreateDirectory( directory );
			}

			int			extIndex		= outputFilePath.LastIndexOfAny( new char[] { '.' } );
			string		basePath		= outputFilePath.Substring( 0, extIndex );
			string		extension		= outputFilePath.Substring( extIndex );
			int			outputFileCount = 0;

			do
			{
				outputFilePath = directory + "/" + basePath + ( outputFileCount++ ).ToString( ) + extension;
			} while ( System.IO.File.Exists( outputFilePath ) );

			//	Open a stream writer to stomp all over the existing file
			Writer = new System.IO.StreamWriter( outputFilePath, false );

			//	Write a nice introductory message
			Writer.WriteLine( "Running \"{0}\" at {1}", System.Reflection.Assembly.GetExecutingAssembly( ).FullName, System.DateTime.Now.ToString( ) );
		}
	}
}
