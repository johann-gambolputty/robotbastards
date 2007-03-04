using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for Shader.
	/// </summary>
	public class Shader : Components.INamedObject
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

		/// <summary>
		/// Binds a parameter from this shader to a render state
		/// </summary>
		/// <param name="parameter"> Parameter to bind </param>
		/// <param name="binding"> Render state variable to bind to </param>
		/// <remarks>
		/// This need only be called once, to set up the binding. Every time that the shader to which this parameter belongs is applied (IApplicable::Apply())
		/// the parameter is updated to match the value of bound variable.
		/// If the parameter binding is set to ShaderParameterBinding.NoBinding, the parameter is unbound, and will no longer get updated.
		/// </remarks>
		public virtual void				BindParameter( ShaderParameter parameter, ShaderParameterBinding binding )
		{
		}


		#region INamedObject Members

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
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		/// <summary>
		/// Shader name changed event
		/// </summary>
		public event Components.NameChangedDelegate NameChanged;

		#endregion

		#region	Private stuff

		private string	m_Name = "";

		#endregion
	}
}
