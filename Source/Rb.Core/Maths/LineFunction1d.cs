
namespace Rb.Core.Maths
{
	/// <summary>
	/// Implements PiecewiseLinearFunction1d. Calculates straight line segments between the function control points
	/// </summary>
	public class LineFunction1d : PiecewiseLinearFunction1d
	{
		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		public override float GetValue( float t )
		{
			if ( ControlPoints.Count == 0 )
			{
				return 0;
			}
			if ( t <= ControlPoints[ 0 ].Position )
			{
				return ControlPoints[ 0 ].Value;
			}
			for ( int i = 0; i < ControlPoints.Count - 1; ++i )
			{
				if ( t <= ControlPoints[ i + 1 ].Position )
				{
					float minLocalT = ControlPoints[ i ].Position;
					float maxLocalT = ControlPoints[ i + 1 ].Position;

					float localT = ( t - minLocalT ) / ( maxLocalT - minLocalT );
					return ( ControlPoints[ i ].Value * ( 1 - localT ) + ControlPoints[ i + 1 ].Value * localT );
				}
			}
			return ControlPoints[ ControlPoints.Count - 1 ].Value;
		}
	}
}
