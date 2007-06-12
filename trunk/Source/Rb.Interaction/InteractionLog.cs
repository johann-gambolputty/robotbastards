using System;
using System.Collections.Generic;
using System.Text;

using Rb.Log;

namespace Rb.Interaction
{
	/// <summary>
	/// Static log class for interactions
	/// </summary>
	/// <example>
	/// InteractionLog.Error( "No tea" );
	/// </example>
	public class InteractionLog : StaticTag< InteractionLog >
	{
		public override string TagName
		{
			get { return "Interaction"; }
		}
	}

}
