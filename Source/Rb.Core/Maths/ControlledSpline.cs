
namespace Rb.Core.Maths
{
	/// <summary>
	/// Base class for splines defined by a series of control points (<see cref="SplineControlPoints"/>)
	/// </summary>
	public abstract class ControlledSpline : Curve
	{
		#region	Construction and setup

		/// <summary>
		/// Constructor
		/// </summary>
		public ControlledSpline( )
		{
			m_ControlPoints = new SplineControlPoints( this );
		}
		
		#endregion

		#region	Spline alteration

		/// <summary>
		/// Called when the spline is changed
		/// </summary>
		/// <remarks>
		/// Resets the time range to [0,C], where C is the number of control points
		/// </remarks>
		public override void OnChanged( )
		{
			SetRange( 0, ControlPoints.Count );
			base.OnChanged( );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Returns the control points defining this spline
		/// </summary>
		public SplineControlPoints ControlPoints
		{
			get
			{
				return m_ControlPoints;
			}
		}

		#endregion

		#region	Fields

		private readonly SplineControlPoints m_ControlPoints;

		#endregion
	}
}
