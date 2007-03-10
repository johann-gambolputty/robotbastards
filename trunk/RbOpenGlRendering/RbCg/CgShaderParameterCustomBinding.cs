using System;
using System.Collections;
using RbEngine.Rendering;
using RbEngine.Maths;
using Tao.Cg;

namespace RbOpenGlRendering.RbCg
{
	/// <summary>
	/// CG shader parameter binding
	/// </summary>
	public class CgShaderParameterCustomBinding : ShaderParameterCustomBinding
	{
		private Object		m_Value;
		private ValueType	m_Type;

		/// <summary>
		/// Sets the CG context and array size for this binding
		/// </summary>
		public CgShaderParameterCustomBinding( ValueType type, int arraySize )
		{
			m_ValueType = type;
			if ( arraySize > 0 )
			{
				switch ( type )
				{
					case ValueType.Int		:	m_Value = new int[ arraySize ];			break;
					case ValueType.Float	:	m_Value = new float[ arraySize ];		break;
					case ValueType.Vector2	:	m_Value = new Vector2[ arraySize ];		break;
					case ValueType.Vector3	:	m_Value = new Vector3[ arraySize ];		break;
					case ValueType.Matrix	:	m_Value = new Matrix44[ arraySize ];	break;
				}
			}
			else
			{
				switch ( type )
				{
					case ValueType.Int		:	m_Value = new int( );		break;
					case ValueType.Float	:	m_Value = new float( );		break;
					case ValueType.Vector2	:	m_Value = new Vector2( );	break;
					case ValueType.Vector3	:	m_Value = new Vector3( );	break;
					case ValueType.Matrix	:	m_Value = new Matrix44( );	break;
				}
			}
		}

		#region	Shader parameter binding

		/// <summary>
		/// Adds a parameter to this binding
		/// </summary>
		public override void	Bind( ShaderParameter parameter )
		{
		}

		/// <summary>
		/// Removes a parameter from this binding
		/// </summary>
		public override void	Unbind( ShaderParameter parameter )
		{
		}

		/// <summary>
		/// Applies this binding to a given shader parameter
		/// </summary>
		/// <param name="parameter"></param>
		public override void ApplyTo( ShaderParameter parameter )
		{
			IntPtr param = ( ( CgShaderParameter )parameter ).Parameter;

			if ( arraySize > 0 )
			{
				switch ( type )
				{
					case ValueType.Int		:
					{
						int[] array = ( int[] )m_Value;
						for ( int arrayIndex = 0; arrayIndex < arraySize;++arrayIndex )
						{
							Cg.cgSetParameter1i( elementParam, array[ arrayIndex ] );
						}
						Cg.cgSetParameterValuei
					}
					case ValueType.Float	:	Cg.cgSetParameter1f( elementParam, ( float[] )( m_Value )[ arrayIndex ] );	break;
					case ValueType.Vector2	:	Cg.cgSetParameter1i( elementParam, ( int[] )( m_Value )[ arrayIndex ] );	break;
					case ValueType.Vector3	:	Cg.cgSetParameter1i( elementParam, ( int[] )( m_Value )[ arrayIndex ] );	break;
					case ValueType.Matrix	:	Cg.cgSetParameter1i( elementParam, ( int[] )( m_Value )[ arrayIndex ] );	break;
						for ( int arrayIndex = 0; arrayIndex < arraySize; ++arrayIndex )
						{
							IntPtr elementParam = Cg.cgGetArrayParameter( param, arrayIndex );
						}
				}
			}
			}
			else
			{
			}

		}


		#endregion

		#region	Single value setters

		/// <summary>
		/// Sets the value of this binding to an integer
		/// </summary>
		public override void Set( int val );

		/// <summary>
		/// Sets the value of this binding to a floating point value
		/// </summary>
		public override void Set( float val );

		/// <summary>
		/// Sets the value of this binding to a 2d vector
		/// </summary>
		public override void Set( Vector2 val );

		/// <summary>
		/// Sets the value of this binding to a 3d point
		/// </summary>
		public override void Set( Point3 val );

		/// <summary>
		/// Sets the value of this binding to a 3d vector
		/// </summary>
		public override void Set( Vector3 val );

		/// <summary>
		/// Sets the value of this binding to a matrix
		/// </summary>
		public override void Set( Matrix44 val );

		#endregion

		#region	Array value setters

		/// <summary>
		/// Sets the value at the specified index to an integer
		/// </summary>
		public override void SetAt( int index, int val );
		
		/// <summary>
		/// Sets the value at the specified index to a floating point value
		/// </summary>
		public override void SetAt( int index, float val );
		
		/// <summary>
		/// Sets the value at the specified index to a 2d vector
		/// </summary>
		public override void SetAt( int index, Vector2 val );
		
		/// <summary>
		/// Sets the value at the specified index to a 3d point
		/// </summary>
		public override void SetAt( int index, Point3 val );
		
		/// <summary>
		/// Sets the value at the specified index to a 3d vector
		/// </summary>
		public override void SetAt( int index, Vector3 val );

		#endregion
	}
}
