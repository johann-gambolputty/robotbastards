
using System;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.NiceControls
{
	/// <summary>
	/// Base class describing a function
	/// </summary>
	public abstract class FunctionDescriptor
	{
		/// <summary>
		/// Sets up the descriptor
		/// </summary>
		/// <param name="name">Name of the function</param>
		public FunctionDescriptor( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// Gets the name of this function
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Returns the name of the function
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		/// <summary>
		/// Returns true if the specified function is supported
		/// </summary>
		public abstract bool SupportsFunction( IFunction1d function );

		/// <summary>
		/// Creates a control for this function
		/// </summary>
		public virtual Control CreateControl( IFunction1d function )
		{
			return null;
		}

		#region Private Members

		private readonly string m_Name;

		#endregion
	}
}
