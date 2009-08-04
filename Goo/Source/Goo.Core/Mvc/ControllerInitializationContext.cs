using System;
using Goo.Core.Environment;
using Rb.Core.Utils;

namespace Goo.Core.Mvc
{
	/// <summary>
	/// Context base class used for controller initialization
	/// </summary>
	[Serializable]
	public class ControllerInitializationContext
	{
		/// <summary>
		/// Gets the environment
		/// </summary>
		public IEnvironment Environment
		{
			get { return m_Environment; }
		}

		/// <summary>
		/// Controller initialization context
		/// </summary>
		public ControllerInitializationContext( IEnvironment env )
		{
			Arguments.CheckNotNull( env, "env" );
			m_Environment = env;
		}

		#region Private Members

		private readonly IEnvironment m_Environment;

		#endregion
	}
}
