using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Range static utilities
	/// </summary>
	public static class Range
	{
		/// <summary>
		/// Gets a midpoint in a range
		/// </summary>
		public static float Mid( Range<float> range, float t )
		{
			return range.Minimum + ( range.Maximum - range.Minimum ) * t;
		}

		/// <summary>
		/// Gets a midpoint in a range
		/// </summary>
		public static double Mid( Range<double> range, double t )
		{
			return range.Minimum + ( range.Maximum - range.Minimum ) * t;
		}

		/// <summary>
		/// Gets the midpoint in a range, where the range type is an INumeric
		/// </summary>
		public static T Mid<T>( Range<T> range, double t ) where T : INumeric<T>, IComparable<T>
		{
			return range.Minimum.Add( range.Maximum.Subtract( range.Minimum ).Multiply( t ) );
		}
	}

	/// <summary>
	/// Stores a pair of values defining a range
	/// </summary>
	/// <typeparam name="T">Range type</typeparam>
	public struct Range<T> where T : IComparable<T>
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="min">Range minimum</param>
		/// <param name="max">Range maximum</param>
		/// <exception cref="ArgumentException">Thrown if min is larger than max</exception>
		public Range( T min, T max )
		{
			if ( min.CompareTo( max ) > 0 )
			{
				throw new ArgumentException( string.Format( "Range minimum {0} cannot be greater than range maximum {1}", min, max ), "min" );
			}
			m_Minimum = min;
			m_Maximum = max;
		}

		/// <summary>
		/// Clamps a value to this range
		/// </summary>
		public T Clamp( T value )
		{
			return value.CompareTo( m_Minimum ) < 0 ? m_Minimum : ( value.CompareTo( m_Maximum ) > 0 ? m_Maximum : value );
		}

		/// <summary>
		/// Returns true if value is within this range
		/// </summary>
		public bool IsInRange( T value )
		{
			return value.CompareTo( m_Minimum ) >= 0 && value.CompareTo( m_Maximum ) <= 0;
		}
		
		/// <summary>
		/// Range set
		/// </summary>
		/// <param name="min">Range minimum</param>
		/// <param name="max">Range maximum</param>
		/// <exception cref="ArgumentException">Thrown if min is larger than max</exception>
		public void Set( T min, T max )
		{
			if ( min.CompareTo( max ) > 0 )
			{
				throw new ArgumentException( string.Format( "Range minimum {0} cannot be greater than range maximum {1}", min, max ), "min" );
			}
			m_Minimum = min;
			m_Maximum = max;
		}

		/// <summary>
		/// Gets/sets the minimum value of the range
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if new minimum is greater than current maximum</exception>
		public T Minimum
		{
			get { return m_Minimum; }
			set
			{
				if ( value.CompareTo( m_Maximum ) > 0 )
				{
					throw new ArgumentException( string.Format( "Range minimum {0} cannot be greater than range maximum {1}", value, m_Maximum ), "value" );
				}
				m_Minimum = value;
			}
		}

		/// <summary>
		/// Gets/sets the maximum value of the range
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if new maximum is less than current minimum</exception>
		public T Maximum
		{
			get { return m_Maximum; }
			set
			{
				if ( value.CompareTo( m_Minimum ) > 0 )
				{
					throw new ArgumentException( string.Format( "Range maximum {0} cannot be less than range maximum {1}", value, m_Minimum ), "value" );
				} 
				m_Maximum = value;
			}
		}

		/// <summary>
		/// Converts this range to a string
		/// </summary>
		public override string ToString( )
		{
			return string.Format( "[{0}..{1}]", m_Minimum, m_Maximum );
		}

		/// <summary>
		/// Returns true if obj is a non-null Range{T} equal to this
		/// </summary>
		public override bool Equals( object obj )
		{
			return ( obj != null ) && ( obj is Range<T> ) && ( this == ( Range<T> )obj );
		}

		/// <summary>
		/// Returns the hash code of this object
		/// </summary>
		public override int GetHashCode( )
		{
			return Minimum.GetHashCode( ) + Maximum.GetHashCode( );
		}

		#region Overloaded operators

		/// <summary>
		/// Returns true if lhs != rhs
		/// </summary>
		public static bool operator != ( Range<T> lhs, Range<T> rhs )
		{
			return ( !lhs.Minimum.Equals( rhs.Minimum ) ) || ( !lhs.Maximum.Equals( rhs.Maximum ) );
		}

		/// <summary>
		/// Returns true if lhs == rhs
		/// </summary>
		public static bool operator == ( Range<T> lhs, Range<T> rhs )
		{
			return ( lhs.Minimum.Equals( rhs.Minimum ) ) && ( lhs.Maximum.Equals( rhs.Maximum ) );
		}

		#endregion

		#region Private Members

		private T m_Minimum;
		private T m_Maximum;

		#endregion
	}
}
