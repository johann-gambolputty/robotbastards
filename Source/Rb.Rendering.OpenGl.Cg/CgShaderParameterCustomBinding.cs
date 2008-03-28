using System;
using Rb.Core.Maths;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// CG shader parameter binding
	/// </summary>
	public class CgShaderParameterCustomBinding : ShaderParameterCustomBinding
	{
		private Object		m_Value;
		private ValueType	m_Type;
		private int			m_ArraySize;

		/// <summary>
		/// Sets the CG context and array size for this binding
		/// </summary>
		public CgShaderParameterCustomBinding( string name, ValueType type, int arraySize ) :
				base( name )
		{
			m_Type		= type;
			m_ArraySize	= arraySize;
			if ( arraySize > 0 )
			{
				switch ( type )
				{
					case ValueType.Int32	:	m_Value = new int[ arraySize ];			break;
					case ValueType.Float32	:	m_Value = new float[ arraySize ];		break;
					case ValueType.Vector2	:	m_Value = new Vector2[ arraySize ];		break;
					case ValueType.Vector3	:	m_Value = new Vector3[ arraySize ];		break;
					case ValueType.Matrix	:	m_Value = new Matrix44[ arraySize ];	break;
				}
			}
			else
			{
				switch ( type )
				{
					case ValueType.Int32	:	m_Value = new int( );		break;
					case ValueType.Float32	:	m_Value = new float( );		break;
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
			IntPtr param = ( ( CgEffectParameter )parameter ).Parameter;

			if ( m_ArraySize > 0 )
			{
				switch ( m_Type )
				{
					case ValueType.Int32	:
					{
						int[] array = ( int[] )m_Value;
						CgEffectParameter.cgSetParameterValueic( param, array );
					//	for ( int arrayIndex = 0; arrayIndex < arraySize; ++arrayIndex )
					//	{
					//		IntPtr elementParam = TaoCg.cgGetArrayParameter( param, arrayIndex );
					//		TaoCg.cgSetParameter1i( elementParam, array[ arrayIndex ] );
					//	}
						break;
					}
					case ValueType.Float32	:
					{
						float[] array = ( float[] )m_Value;
						CgEffectParameter.cgSetParameterValuefc( param, array );
					//	for ( int arrayIndex = 0; arrayIndex < arraySize; ++arrayIndex )
					//	{
					//		IntPtr elementParam = TaoCg.cgGetArrayParameter( param, arrayIndex );
					//		TaoCg.cgSetParameter1f( elementParam, array[ arrayIndex ] );
					//	}
						break;
					}
					case ValueType.Vector2	:
					{
						Vector2[] array = ( Vector2[] )m_Value;
						for ( int arrayIndex = 0; arrayIndex < m_ArraySize; ++arrayIndex )
						{
							IntPtr elementParam = TaoCg.cgGetArrayParameter( param, arrayIndex );
							TaoCg.cgSetParameter2f( elementParam, array[ arrayIndex ].X, array[ arrayIndex ].Y );
						}
						break;
					}
					case ValueType.Vector3	:
					{
						Vector3[] array = ( Vector3[] )m_Value;
						for ( int arrayIndex = 0; arrayIndex < m_ArraySize; ++arrayIndex )
						{
							IntPtr elementParam = TaoCg.cgGetArrayParameter( param, arrayIndex );
							TaoCg.cgSetParameter3f( elementParam, array[ arrayIndex ].X, array[ arrayIndex ].Y, array[ arrayIndex ].Z );
						}
						break;
					}
					case ValueType.Matrix	:
					{
						Matrix44[] array = ( Matrix44[] )m_Value;
						for ( int arrayIndex = 0; arrayIndex < m_ArraySize; ++arrayIndex )
						{
							IntPtr elementParam = TaoCg.cgGetArrayParameter( param, arrayIndex );

                            if ( array[ arrayIndex ] != null )
                            {
							    CgEffectParameter.cgSetMatrixParameterfc( elementParam, array[ arrayIndex ].Elements );
                            }
                            else
                            {
							    CgEffectParameter.cgSetMatrixParameterfc( elementParam, Matrix44.Identity.Elements );
                            }
						}
						break;
					}
				}
			}
			else
			{
				switch ( m_Type )
				{
					case ValueType.Int32		:
					{
						TaoCg.cgSetParameter1i( param, ( int )m_Value );
						break;
					}
					case ValueType.Float32	:
					{
						TaoCg.cgSetParameter1f( param, ( float )m_Value );
						break;
					}
					case ValueType.Vector2	:
					{
						Vector2 val = ( Vector2 )m_Value;
						TaoCg.cgSetParameter2f( param, val.X, val.Y );
						break;
					}
					case ValueType.Vector3	:
					{
						Vector3 val = ( Vector3 )m_Value;
						TaoCg.cgSetParameter3f( param, val.X, val.Y, val.Z );
						break;
					}
					case ValueType.Matrix	:
					{
						Matrix44 val = ( Matrix44 )m_Value;
						CgEffectParameter.cgSetMatrixParameterfc( param, val.Elements );
						break;
					}
				}
			}

		}


		#endregion

		#region	Single value setters

		/// <summary>
		/// Checks a type
		/// </summary>
		private void CheckType( ValueType expected )
		{
			GraphicsLog.Assert( m_Type == expected, "Set() failed - binding is of type \"{0}\", value is of type \"{1}\"", m_Type.ToString( ), expected.ToString( ) );
		}

		/// <summary>
		/// Sets the value of this binding to an integer
		/// </summary>
		public override void Set( int val )
		{
			CheckType( ValueType.Int32 );
			m_Value = val;
		}

		/// <summary>
		/// Sets the value of this binding to a floating point value
		/// </summary>
		public override void Set( float val )
		{
			CheckType( ValueType.Float32 );
			m_Value = val;
		}

		/// <summary>
		/// Sets the value of this binding to a 2d vector
		/// </summary>
		public override void Set( Vector2 val )
		{
			CheckType( ValueType.Vector2 );
			m_Value = val;
		}

		/// <summary>
		/// Sets the value of this binding to a 3d point
		/// </summary>
		public override void Set( Point3 val )
		{
			CheckType( ValueType.Vector3 );
			m_Value = new Vector3( val.X, val.Y, val.Z );
		}

		/// <summary>
		/// Sets the value of this binding to a 3d vector
		/// </summary>
		public override void Set( Vector3 val )
		{
			CheckType( ValueType.Vector3 );
			m_Value = val;
		}

		/// <summary>
		/// Sets the value of this binding to a matrix
		/// </summary>
		public override void Set( Matrix44 val )
		{
			CheckType( ValueType.Matrix );
			m_Value = val;
		}

		#endregion

		#region	Array value setters

		/// <summary>
		/// Sets the value at the specified index to an integer
		/// </summary>
		public override void SetAt( int index, int val )
		{
			CheckType( ValueType.Int32 );
			( ( int[] )m_Value )[ index ] = val;
		}
		
		/// <summary>
		/// Sets the value at the specified index to a floating point value
		/// </summary>
		public override void SetAt( int index, float val )
		{
			CheckType( ValueType.Float32 );
			( ( float[] )m_Value )[ index ] = val;
		}
		
		/// <summary>
		/// Sets the value at the specified index to a 2d vector
		/// </summary>
		public override void SetAt( int index, Vector2 val )
		{
			CheckType( ValueType.Vector2 );
			( ( Vector2[] )m_Value )[ index ] = val;
		}
		
		/// <summary>
		/// Sets the value at the specified index to a 3d point
		/// </summary>
		public override void SetAt( int index, Point3 val )
		{
			CheckType( ValueType.Vector3 );
			( ( Vector3[] )m_Value )[ index ] = new Vector3( val.X, val.Y, val.Z );
		}
		
		/// <summary>
		/// Sets the value at the specified index to a 3d vector
		/// </summary>
		public override void SetAt( int index, Vector3 val )
		{
			CheckType( ValueType.Vector3 );
			( ( Vector3[] )m_Value )[ index ] = val;
		}

        /// <summary>
        /// Sets the value at the specified index to a matrix
        /// </summary>
        public override void SetAt( int index, Matrix44 val )
        {
            CheckType( ValueType.Matrix );
            ( ( Matrix44[] )m_Value )[ index ] = val;   
        }

		#endregion
	}
}
