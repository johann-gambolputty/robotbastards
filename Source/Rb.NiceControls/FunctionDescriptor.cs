
using System.Windows.Forms;

namespace Rb.NiceControls
{
	/// <summary>
	/// Base class describing a function
	/// </summary>
	public class FunctionDescriptor
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
		/// Creates a control for this function
		/// </summary>
		public virtual Control CreateControl( )
		{
			return null;
		}

		#region Private Members

		private readonly string m_Name;

		#endregion
	}
}
