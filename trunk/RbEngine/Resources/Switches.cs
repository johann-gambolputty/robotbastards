using System;
using System.Diagnostics;


namespace RbEngine.Resources
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

		private static BooleanSwitch	ms_Info		= new BooleanSwitch( "resourceInfo",		"Resource information" );
		private static BooleanSwitch	ms_Warnings	= new BooleanSwitch( "resourceWarnings",	"Resource warnings" );
		private static BooleanSwitch	ms_Errors	= new BooleanSwitch( "resourceErrors",		"Resource errors" );
	}
}
