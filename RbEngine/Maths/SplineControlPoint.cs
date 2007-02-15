using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for SplineControlPoint.
	/// </summary>
	public class SplineControlPoint
	{

		#region	Public properties

		/// <summary>
		/// The position of the control point. If set, then the owner spline is changed (Spline.OnChanged())
		/// </summary>
		public Vector3		Position
		{
			get
			{
				return m_Frame.Position;
			}
			set
			{
				m_Frame.Position = value;
				m_Owner.OnChanged( );
			}
		}

		/// <summary>
		/// The Frenet frame at the control
		/// </summary>
		public SplineFrame	Frame
		{
			get
			{
				return m_Frame;
			}
		}

		/// <summary>
		/// Spline that owns this control point
		/// </summary>
		public Spline		Owner
		{
			get
			{
				return m_Owner;
			}
		}

		/// <summary>
		/// Fraction along the spline that this control point is defined at (always a whole number)
		/// </summary>
		public float		T
		{
			get
			{
				return ( float )m_Index;
			}
		}


		#endregion

		#region	Construction

		/// <summary>
		/// Sets up this control point
		/// </summary>
		public SplineControlPoint( Spline owner, int index )
		{
			m_Owner = owner;
			m_Index	= index;
			m_Frame	= m_Owner.EvaluateFrame( ( float )index );

			owner.OnChangedEvent += new Spline.ChangedDelegate( OnOwnerSplineChanged );
		}

		#endregion

		#region	Private stuff

		private Spline		m_Owner;
		private int			m_Index;
		private SplineFrame	m_Frame;

		/// <summary>
		/// Delegate, added to the Spline.OnChangedEvent. Re-evaluates the frame of the control point
		/// </summary>
		/// <param name="spline"></param>
		private void		OnOwnerSplineChanged( Spline spline )
		{
			m_Frame = spline.EvaluateFrame( ( float )m_Index );
		}


		#endregion

	}
}
