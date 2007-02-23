using System;
using System.Collections;

namespace RbEngine
{
	/// <summary>
	/// Works as a TextWriterTraceListener, except that the output file is backed up and emptied
	/// </summary>
	public class NiceTextWriterTraceListener : System.Diagnostics.TextWriterTraceListener
	{
		public NiceTextWriterTraceListener( string outputFilePath )
		{
			//	If the output file exists, then rename it
			if ( System.IO.File.Exists( outputFilePath ) )
			{
				int extIndex = outputFilePath.LastIndexOfAny( new char[] { '.' } );
				string basePath = outputFilePath.Substring( 0, extIndex );
				string extension = outputFilePath.Substring( extIndex );

				string		altPath		= outputFilePath;
				ArrayList	fileNames	= new ArrayList( );
				fileNames.Add( altPath );

				//	Count the number of files with the name [Name]X[extension]
				while ( ( System.IO.File.Exists( altPath ) ) && ( fileNames.Count < 10 ) )
				{
					altPath = basePath + ( fileNames.Count - 1 ).ToString( ) + extension;
					fileNames.Add( altPath );
				}

				for ( int copyIndex = fileNames.Count - 1; copyIndex >= 1; --copyIndex )
				{
					System.IO.File.Copy( ( string )fileNames[ copyIndex - 1 ], ( string )fileNames[ copyIndex ], true );
				}
			}

			//	Open a stream writer to stomp all over the existing file
			Writer = new System.IO.StreamWriter( outputFilePath, false );

			//	Write a nice introductory message
			Writer.WriteLine( "Running \"{0}\" at {1}", System.Reflection.Assembly.GetExecutingAssembly( ).FullName, System.DateTime.Now.ToString( ) );
		}
	}
}
