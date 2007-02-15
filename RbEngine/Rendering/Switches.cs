using System;
using System.Diagnostics;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for Switches.
	/// </summary>
	public class Switches
	{
		public static BooleanSwitch		Info
		{
			get
			{
				return ms_Info;
			}
		}
		
		public static BooleanSwitch		Warnings
		{
			get
			{
				return ms_Warnings;
			}
		}
		
		public static BooleanSwitch		Errors
		{
			get
			{
				return ms_Errors;
			}
		}

		private static BooleanSwitch	ms_Info		= new BooleanSwitch( "renderingInfo",		"Rendering information" );
		private static BooleanSwitch	ms_Warnings	= new BooleanSwitch( "renderingWarnings",	"Rendering warnings" );
		private static BooleanSwitch	ms_Errors	= new BooleanSwitch( "renderingErrors",		"Rendering errors" );
	}
}
