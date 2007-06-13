using Rb.Core.Components;

namespace Rb.Rendering
{
	/// <summary>
	/// Shader is a base class for collections of techniques
	/// </summary>
	public class Shader : INamed
	{
		/// <summary>
		/// Finds a shader parameter by name
		/// </summary>
		/// <param name="name"> Name of the parameter to search for </param>
		/// <returns> Returns the named parameter, or null if the parameter could not be found </returns>
		public virtual ShaderParameter	GetParameter( string name )
		{
			return null;
		}

		#region INamed Members

		/// <summary>
		/// Shader name
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		#endregion

		#region	Private stuff

		private string	m_Name = "";

		#endregion
	}
}
