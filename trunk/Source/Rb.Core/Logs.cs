using System;
using System.Collections.Generic;
using System.Text;

using Rb.Log;

namespace Rb.Core
{
	/// <summary>
	/// Static log class for resources
	/// </summary>
	/// <example>
	/// ResourcesLog.Error( "No tea" );
	/// </example>
	public class ResourcesLog : StaticTag< ResourcesLog >
	{
		public override string TagName
		{
			get { return "Resources"; }
		}
	}

	/// <summary>
	/// Static log class for maths
	/// </summary>
	/// <example>
	/// MathsLog.Error( "No tea" );
	/// </example>
	public class MathsLog : StaticTag< MathsLog >
	{
		public override string TagName
		{
			get { return "Maths"; }
		}
	}

	/// <summary>
	/// Static log class for networking
	/// </summary>
	/// <example>
	/// NetworkLog.Error( "No tea" );
	/// </example>
	public class NetworkLog : StaticTag< NetworkLog >
	{
		public override string TagName
		{
			get { return "Network"; }
		}

		/// <summary>
		/// Static log class for RUNT networking
		/// </summary>
		/// <example>
		/// NetworkLog.RuntLog.Error( "No tea" );
		/// </example>
		public class RuntLog : StaticTag< RuntLog >
		{
			public override Tag ParentTag
			{
				get { return NetworkLog.Tag; }
			}

			public override string TagName
			{
				get { return "Runt"; }
			}
		}

	}

	/// <summary>
	/// Static log class for components
	/// </summary>
	/// <example>
	/// ComponentLog.Error( "No tea" );
	/// </example>
	public class ComponentLog : StaticTag< ComponentLog >
	{
		public override string TagName
		{
			get { return "Components"; }
		}
	}
}