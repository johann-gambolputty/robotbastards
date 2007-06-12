using System;
using System.Collections;

namespace RbEngine.Maths
{
	/// <summary>
	/// Stores a collection of control points that defines the shape of a ControlledSpline
	/// </summary>
	public class SplineControlPoints
	{
		#region	Construction and setup

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="owner"> Owner of the control points </param>
		public SplineControlPoints( Spline owner )
		{
			m_Owner = owner;			
		}

		/// <summary>
		/// Adds a new control point at the specified position to the spline
		/// </summary>
		public void					Add( Vector3 pt )
		{
			m_Points.Add( new SplineControlPoint( m_Owner, m_Points.Count ) );
			m_Owner.OnChanged( );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Gets/sets the control point at the specified index
		/// </summary>
		public SplineControlPoint	this[ int index ]
		{
			get
			{
				return ( SplineControlPoint )m_Points[ index ];
			}
			set
			{
				m_Points[ index ] = value;
			}
		}

		/// <summary>
		/// Returns the number of stored control points
		/// </summary>
		public int			Count
		{
			get
			{
				return m_Points.Count;
			}
		}

		#endregion

		#region	Private stuff

		private ArrayList	m_Points	= new ArrayList( );
		private Spline		m_Owner		= null;

		#endregion
	}
}
