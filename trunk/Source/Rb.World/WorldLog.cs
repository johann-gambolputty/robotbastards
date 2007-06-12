using System;
using System.Collections.Generic;
using System.Text;

using Rb.Log;

namespace Rb.World
{
	/// <summary>
	/// Static log class for the world
	/// </summary>
	/// <example>
	/// WorldLog.Error( "No tea" );
	/// </example>
	public class WorldLog : StaticTag< WorldLog >
	{
		public override string TagName
		{
			get { return "World"; }
		}
	}

}
