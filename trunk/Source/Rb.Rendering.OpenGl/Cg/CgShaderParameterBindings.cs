using System.Collections;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// CG shader parameter bindings
	/// </summary>
	public class CgShaderParameterBindings : ShaderParameterBindings
	{

		/// <summary>
		/// Creates a binding
		/// </summary>
		/// <returns></returns>
		public override ShaderParameterCustomBinding CreateBinding( string name, ShaderParameterCustomBinding.ValueType type )
		{
			CgShaderParameterCustomBinding newBinding = new CgShaderParameterCustomBinding( name, type, 0 );
			m_CustomBindings.Add( newBinding );

			AddNewBinding( newBinding );

			return newBinding;
		}

		/// <summary>
		/// Creates a binding to an array
		/// </summary>
		public override ShaderParameterCustomBinding CreateBinding( string name, ShaderParameterCustomBinding.ValueType type, int arraySize )
		{
			CgShaderParameterCustomBinding newBinding = new CgShaderParameterCustomBinding( name, type, arraySize );
			AddNewBinding( newBinding );

			return newBinding;
		}

		/// <summary>
		/// Gets a default binding
		/// </summary>
		public override ShaderParameterBinding GetBinding( ShaderParameterDefaultBinding binding )
		{
			return m_DefaultBindings[ ( int )binding ];
		}

		/// <summary>
		///	Finds a binding by its name
		/// </summary>
		public override ShaderParameterBinding GetBinding( string name )
		{
			for ( int defaultIndex = 0; defaultIndex < ( int )ShaderParameterDefaultBinding.NumDefaults; ++defaultIndex )
			{
				if ( string.Compare( ( ( ShaderParameterDefaultBinding )defaultIndex ).ToString( ), name ) == 0 )
				{
					return m_DefaultBindings[ defaultIndex ];
				}
			}

			foreach ( CgShaderParameterCustomBinding customBinding in m_CustomBindings )
			{
				if ( string.Compare( customBinding.Name, name, true ) == 0 )
				{
					return customBinding;
				}
			}

			return null;
		}

		/// <summary>
		/// Creates default bindings
		/// </summary>
		public CgShaderParameterBindings( )
		{
			for ( int defaultIndex = 0; defaultIndex < m_DefaultBindings.Length; ++defaultIndex )
			{
				m_DefaultBindings[ defaultIndex ] = new CgShaderParameterDefaultBinding( ( ShaderParameterDefaultBinding )defaultIndex );
			}
		}
		
		private readonly ArrayList m_CustomBindings	= new ArrayList( );
		private readonly CgShaderParameterDefaultBinding[] m_DefaultBindings = new CgShaderParameterDefaultBinding[(int)ShaderParameterDefaultBinding.NumDefaults];

	}
}
