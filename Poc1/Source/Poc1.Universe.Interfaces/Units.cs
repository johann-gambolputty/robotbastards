using System;
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Units
	/// </summary>
	public static class Units
	{
		/// <summary>
		/// Unit conversion factors and helper functions
		/// </summary>
		public static class Convert
		{
			public const double MulUniToMetres = 1.0 / 100.0;
			public const double MulUniToRender = 1.0 / 1000.0;
			public const double MulUniToAstroRender = 1.0 / 100000.0;

			public const double MulMetresToUni = 1.0 / MulUniToMetres;
			public const double MulMetresToRender = MulUniToRender / MulUniToMetres;
			public const double MulMetresToAstroRender = MulUniToAstroRender / MulUniToMetres;

			public const double MulRenderToUni = 1.0 / MulUniToRender;
			public const double MulRenderToMetres = MulUniToMetres / MulUniToRender;
			public const double MulRenderToAstroRender = MulUniToAstroRender / MulUniToRender;

			public const double MulAstroRenderToUni = 1.0 / MulUniToAstroRender;
			public const double MulAstroRenderToMetres = MulUniToMetres / MulUniToAstroRender;
			public const double MulAstroRenderToRender = MulUniToRender / MulUniToAstroRender;

			#region Uni unit conversions

			public static double UniToMetres( long value )
			{
				return value * MulUniToMetres;
			}

			public static double UniToRender( long value )
			{
				return value * MulUniToRender;
			}

			public static double UniToAstroRender( long value )
			{
				return value * MulUniToAstroRender;
			}

			#endregion

			#region Metre conversions

			public static long MetresToUni( double value )
			{
				return ( long )( value * MulMetresToUni );
			}

			public static double MetresToRender( double value )
			{
				return value * MulMetresToRender;
			}

			public static double MetresToAstroRender( double value )
			{
				return value * MulMetresToAstroRender;
			}

			#endregion
		}

		/// <summary>
		/// Universe units. Measures distances within solar systems using fixed point. 1 uni unit is 1 centimetre
		/// </summary>
		public struct UniUnits
		{
			//	1 uni unit = 1cm
			//
			//	1 metre = 1 metre = 100 uni units
			//	1 render = 10 metres = 1000 uni units
			//	1 astro render = 1000 metres = 100000 uni units
			//

			/// <summary>
			/// Setup constructor
			/// </summary>
			public UniUnits( long value )
			{
				m_Value = value;
			}

			/// <summary>
			/// Gets the value of this object
			/// </summary>
			public static implicit operator long ( UniUnits val )
			{
				return val.m_Value;
			}

			#region Conversions

			/// <summary>
			/// Get the value of this unit measure in uni units
			/// </summary>
			public UniUnits ToUniUnits
			{
				get { return this; }
			}

			/// <summary>
			/// Gets the value of this unit measure to astro render units
			/// </summary>
			public AstroRenderUnits ToAstroRenderUnits
			{
				get { return new AstroRenderUnits( m_Value * Convert.MulUniToAstroRender ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to render units
			/// </summary>
			public RenderUnits ToRenderUnits
			{
				get { return new RenderUnits( ( float )( m_Value * Convert.MulUniToRender ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to metres
			/// </summary>
			public Metres ToMetres
			{
				get { return new Metres( m_Value * Convert.MulUniToMetres ); }
			}

			#endregion

			#region Operators

			/// <summary>
			/// Uni units addition
			/// </summary>
			public static UniUnits operator + ( UniUnits lhs, UniUnits rhs )
			{
				return new UniUnits( lhs.m_Value + rhs.m_Value );
			}

			/// <summary>
			/// Uni units subtraction
			/// </summary>
			public static UniUnits operator - ( UniUnits lhs, UniUnits rhs )
			{
				return new UniUnits( lhs.m_Value - rhs.m_Value );
			}

			#endregion

			#region Private Members

			private long m_Value;

			#endregion
		}

		/// <summary>
		/// Rendering units. Measures distances relative to the viewer, using single precision floating point
		/// </summary>
		public struct RenderUnits
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public RenderUnits( float value )
			{
				m_Value = value;
			}

			#region Conversions

			/// <summary>
			/// Get the value of this unit measure in uni units
			/// </summary>
			public UniUnits ToUniUnits
			{
				get { return new UniUnits( ( long )( m_Value * Convert.MulRenderToUni ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to astro render units
			/// </summary>
			public AstroRenderUnits ToAstroRenderUnits
			{
				get { return new AstroRenderUnits( m_Value * Convert.MulRenderToAstroRender ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to render units
			/// </summary>
			public RenderUnits ToRenderUnits
			{
				get { return this; }
			}

			/// <summary>
			/// Gets the value of this unit measure to metres
			/// </summary>
			public Metres ToMetres
			{
				get { return new Metres( m_Value * Convert.MulRenderToMetres ); }
			}

			#endregion

			/// <summary>
			/// Gets the render unit value 
			/// </summary>
			public float Value
			{
				get { return m_Value; }
			}

			/// <summary>
			/// Implicit conversion to float
			/// </summary>
			public static implicit operator float( RenderUnits renderUnits )
			{
				return renderUnits.Value;
			}

			/// <summary>
			/// Makes a point local to an origin point, measured in render units
			/// </summary>
			public static Point3 MakeRelativePoint( UniPoint3 origin, UniPoint3 pt )
			{
				double x = Convert.UniToRender( pt.X - origin.X );
				double y = Convert.UniToRender( pt.Y - origin.Y );
				double z = Convert.UniToRender( pt.Z - origin.Z );
				return new Point3( ( float )x, ( float )y, ( float )z );	
			}

			#region Private Members

			private float m_Value; 

			#endregion
		}

		/// <summary>
		/// Astro render units.
		/// </summary>
		public struct AstroRenderUnits
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public AstroRenderUnits( double value )
			{
				m_Value = value;
			}

			#region Conversions

			/// <summary>
			/// Get the value of this unit measure in uni units
			/// </summary>
			public UniUnits ToUniUnits
			{
				get { return new UniUnits( ( long )( m_Value * Convert.MulAstroRenderToUni ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to astro render units
			/// </summary>
			public AstroRenderUnits ToAstroRenderUnits
			{
				get { return this; }
			}

			/// <summary>
			/// Gets the value of this unit measure to render units
			/// </summary>
			public RenderUnits ToRenderUnits
			{
				get { return new RenderUnits( ( float )( m_Value * Convert.MulAstroRenderToRender ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to metres
			/// </summary>
			public Metres ToMetres
			{
				get { return new Metres( m_Value * Convert.MulAstroRenderToMetres ); }
			}

			#endregion

			/// <summary>
			/// Gets the render unit value 
			/// </summary>
			public double Value
			{
				get { return m_Value; }
			}

			/// <summary>
			/// Implicit conversion to float
			/// </summary>
			public static implicit operator double( AstroRenderUnits renderUnits )
			{
				return renderUnits.Value;
			}

			/// <summary>
			/// Makes a point local to an origin point, measured in render units
			/// </summary>
			public static Point3 MakeRelativePoint( UniPoint3 origin, UniPoint3 pt )
			{
				double x = Convert.UniToAstroRender( pt.X - origin.X );
				double y = Convert.UniToAstroRender( pt.Y - origin.Y );
				double z = Convert.UniToAstroRender( pt.Z - origin.Z );
				return new Point3( ( float )x, ( float )y, ( float )z );
			}

			#region Private Members

			private double m_Value; 

			#endregion	
		}
		
		/// <summary>
		/// Metres. Measures distances within solar systems using double precision floating point
		/// </summary>
		public struct Metres : IComparable<Metres>, INumeric<Metres>
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public Metres( double value )
			{
				m_Value = value;
			}

			#region Conversions

			/// <summary>
			/// Get the value of this unit measure in uni units
			/// </summary>
			public UniUnits ToUniUnits
			{
				get { return new UniUnits( ( long )( m_Value * Convert.MulMetresToUni ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to astro render units
			/// </summary>
			public AstroRenderUnits ToAstroRenderUnits
			{
				get { return new AstroRenderUnits( m_Value * Convert.MulMetresToAstroRender ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to render units
			/// </summary>
			public RenderUnits ToRenderUnits
			{
				get { return new RenderUnits( ( float )( m_Value * Convert.MulMetresToRender ) ); }
			}

			/// <summary>
			/// Gets the value of this unit measure to metres
			/// </summary>
			public Metres ToMetres
			{
				get { return this; }
			}

			#endregion


			/// <summary>
			/// Gets the metres value
			/// </summary>
			public double Value
			{
				get { return m_Value; }
			}

			/// <summary>
			/// Implicit conversion to float double
			/// </summary>
			public static implicit operator double( Metres metres )
			{
				return metres.Value;
			}

			#region Operators

			/// <summary>
			/// Addition of two metre values
			/// </summary>
			public static Metres operator + ( Metres lhs, Metres rhs )
			{
				return new Metres( lhs.Value + rhs.Value );
			}

			/// <summary>
			/// Multiplication of a metre value by a scalar
			/// </summary>
			public static Metres operator * ( Metres lhs, double rhs )
			{
				return new Metres( lhs.Value * rhs );
			}

			#endregion

			#region Private Members
			
			private double m_Value; 

			#endregion

			#region IComparable<Metres> Members

			/// <summary>
			/// Compares this value to another. Returns -1 if this is less than other, 0
			/// if this is equal to other, or 1 if this is greater than other
			/// </summary>
			public int CompareTo( Metres other )
			{
				return m_Value < other.m_Value ? -1 : ( m_Value > other.m_Value ? 1 : 0 );
			}

			#endregion

			#region INumeric<Metres> Members

			/// <summary>
			/// Adds this value to another and returns the result
			/// </summary>
			public Metres Add( Metres addend )
			{
				return new Metres( m_Value + addend.m_Value );
			}

			/// <summary>
			/// Subtracts another value from this and returns the result
			/// </summary>
			public Metres Subtract( Metres subtrahend )
			{
				return new Metres( m_Value - subtrahend.m_Value );
			}

			/// <summary>
			/// Divides this value by another and returns the result
			/// </summary>
			public Metres Divide( double divisor )
			{
				return new Metres( m_Value / divisor );
			}

			/// <summary>
			/// Multiplies this value by another and returns the result
			/// </summary>
			public Metres Multiply( double multiplier )
			{
				return new Metres( m_Value * multiplier );
			}

			#endregion
		}


	}
}
