
using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Implements PiecewiseLinearFunction1d. Calculates straight line segments between the function control points
	/// </summary>
	[Serializable]
	public class LineFunction1d : PiecewiseLinearFunction1d
	{
		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		public override float GetValue( float t )
		{
			if ( NumControlPoints == 0 )
			{
				return 0;
			}
			if ( t <= this[ 0 ].X )
			{
				return this[ 0 ].Y;
			}
			for ( int i = 0; i < NumControlPoints - 1; ++i )
			{
				if ( t <= this[ i + 1 ].X )
				{
					float minLocalT = this[ i ].X;
					float maxLocalT = this[ i + 1 ].Y;

					float localT = ( t - minLocalT ) / ( maxLocalT - minLocalT );
					return ( this[ i ].Y * ( 1 - localT ) + this[ i + 1 ].Y * localT );
				}
			}
			return this[ NumControlPoints - 1 ].Y;
		}
	}
}
