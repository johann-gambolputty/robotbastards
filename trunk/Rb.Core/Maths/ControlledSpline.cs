using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for ControlledSpline.
	/// </summary>
	public abstract class ControlledSpline : Spline
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
		public override void				OnChanged( )
		{
			SetRange( 0, ( float )ControlPoints.Count );
			base.OnChanged( );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Returns the control points defining this spline
		/// </summary>
		public SplineControlPoints	ControlPoints
		{
			get
			{
				return m_ControlPoints;
			}
		}

		#endregion

		#region	Fields

		private SplineControlPoints	m_ControlPoints;

		#endregion
	}
}
