using System;
using System.Collections;
using RbEngine.Rendering;
using Tao.Cg;

namespace RbOpenGlRendering.RbCg
{
	/// <summary>
	/// CG shader parameter bindings
	/// </summary>
	public class CgShaderParameterBindings : ShaderParameterBindings
	{
		/// <summary>
		/// Event, gets called when CreateBinding(), CreateArrayBinding() is called
		/// </summary>
		public event NewBindingDelegate	OnNewBinding;

		/// <summary>
		/// Adds a delegate to an event that gets called when CreateBinding(), CreateArrayBinding() is called. Also, called for all existing default and custom bindings
		/// </summary>
		public override void							AddNewBindingListener( NewBindingDelegate newBinding )
		{
			for ( int defaultIndex = 0; defaultIndex < ( int )ShaderParameterDefaultBinding.NumBindings; ++defaultIndex )
			{
				newBinding( m_DefaultBindings[ defaultIndex ] );
			}

			foreach ( ShaderParameterBinding customBinding in m_CustomBindings )
			{
				newBinding( customBinding );
			}

			OnNewBinding += newBinding;
		}

		/// <summary>
		/// Creates a binding
		/// </summary>
		/// <returns></returns>
		public override ShaderParameterCustomBinding	CreateBinding( string name, ValueType type )
		{
			ShaderParameterBinding newBinding = new CgShaderParameterCustomBinding( type, 0 );
			m_CustomBindings.Add( newBinding );
			
			if ( OnNewBinding != null )
			{
				OnNewBinding( newBinding );
			}

			return newBinding;
		}

		/// <summary>
		/// Creates a binding to an array
		/// </summary>
		public override ShaderParameterCustomBinding	CreateBinding( string name, ValueType type, int arraySize )
		{
			ShaderParameterBinding newBinding = new CgShaderParameterCustomBinding( type, arraySize );
			
			if ( OnNewBinding != null )
			{
				OnNewBinding( newBinding );
			}

			return newBinding;
		}

		/// <summary>
		/// Gets a default binding
		/// </summary>
		public override ShaderParameterBinding			GetBinding( ShaderParameterDefaultBinding binding )
		{
			return m_DefaultBindings[ ( int )binding ];
		}

		/// <summary>
		///	Finds a binding by its name
		/// </summary>
		public override ShaderParameterBinding			GetBinding( string name )
		{
			foreach ( CgShaderParameterCustomBinding customBinding in m_CustomBindings )
			{
				if ( string.Compare( customBinding.Name, name, true ) == 0 )
				{
					return customBinding;
				}
			}

			return null;
		}
		
		private ArrayList							m_CustomBindings	= new ArrayList( );
		private CgShaderParameterDefaultBinding[]	m_DefaultBindings	= new CgShaderParameterDefaultBinding[ ( int )ShaderParameterDefaultBinding.NumBindings ];

	}
}
