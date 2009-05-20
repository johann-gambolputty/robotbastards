using System;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	public class CgEffectValueDataSource<T> : IEffectValueDataSource<T>
	{
		private object m_Value;
		private readonly Action<IntPtr> m_Apply;


		public CgEffectValueDataSource( )
		{
			m_Apply = CreateApplyAction( );
		}

		#region IEffectValueDataSource<T> Members

		public T Value
		{
			get { return ( T )m_Value; }
			set { m_Value = value; }
		}

		#endregion

		#region IEffectDataSource Members

		public void Bind( IEffectParameter parameter )
		{
			parameter.DataSource = this;
		}

		public void Apply( IEffectParameter parameter )
		{
			m_Apply( ( ( CgEffectParameter )parameter ).Parameter );
		}

		#endregion

		#region Private Members

		private Action<IntPtr> CreateApplyAction( )
		{
			Action<IntPtr> apply;
			if ( typeof( T ) == typeof( int ) )
			{
				apply = delegate( IntPtr param )
							{
								TaoCg.cgSetParameter1i( param, ( int )m_Value );
							};
			}
			else if ( typeof( T ) == typeof( float ) )
			{
				apply = delegate( IntPtr param )
							{
								TaoCg.cgSetParameter1f( param, ( float )m_Value );
							};
			}
			else if ( typeof( T ) == typeof( Point2 ) )
			{
				apply = delegate( IntPtr param )
							{
								Point2 pt = ( Point2 )m_Value;
								TaoCg.cgSetParameter2f( param, pt.X, pt.Y );
							};
			}
			else if ( typeof( T ) == typeof( Point3 ) )
			{
				apply = delegate( IntPtr param )
							{
								Point3 pt = ( Point3 )m_Value;
								TaoCg.cgSetParameter3f( param, pt.X, pt.Y, pt.Z );
							};
			}
			else if ( typeof( T ) == typeof( Vector2 ) )
			{
				apply = delegate( IntPtr param )
							{
								Vector2 pt = ( Vector2 )m_Value;
								TaoCg.cgSetParameter2f( param, pt.X, pt.Y );
							};
			}
			else if ( typeof( T ) == typeof( Vector3 ) )
			{
				apply = delegate( IntPtr param )
							{
								Vector3 pt = ( Vector3 )m_Value;
								TaoCg.cgSetParameter3f( param, pt.X, pt.Y, pt.Z );
							};
			}
			else if ( typeof( T ) == typeof( int[] ) )
			{
				apply = delegate( IntPtr param )
					{
						CgEffectParameter.cgSetParameterValueic( param, ( int[] )m_Value );
					};
			}
			else if ( typeof( T ) == typeof( float[] ) )
			{
				apply = delegate( IntPtr param )
					{
						CgEffectParameter.cgSetParameterValuefc( param, ( float[] )m_Value );
					};
			}
			else if ( typeof( T ) == typeof( Matrix44 ) )
			{
				apply = delegate( IntPtr param )
					{
						CgEffectParameter.cgSetMatrixParameterfc( param, ( ( Matrix44 )m_Value ).Elements );
					};
			}
			else if ( typeof( T ) == typeof( InvariantMatrix44 ) )
			{
				apply = delegate( IntPtr param )
					{
						CgEffectParameter.cgSetMatrixParameterfc( param, ( ( InvariantMatrix44 )m_Value ).ToFloatArray( ) );
					};
			}
			else if ( typeof( T ) == typeof( Matrix44[] ) )
			{
				apply = delegate( IntPtr param )
					{
						Matrix44[] values = ( Matrix44[] )m_Value;
						for ( int i = 0; i < values.Length; ++i )
						{
							IntPtr elementParam = TaoCg.cgGetArrayParameter( param, i );
							Matrix44 matrix = values[ i ] ?? new Matrix44( );
							CgEffectParameter.cgSetMatrixParameterfc( elementParam, matrix.Elements );
						}
					};
			}
			else
			{
				throw new NotImplementedException( string.Format( "Support for type \"{0}\" in value data source has not been added yet - sorry", typeof( T ) ) );
			}
			return apply;
		}

		#endregion
	}
}
