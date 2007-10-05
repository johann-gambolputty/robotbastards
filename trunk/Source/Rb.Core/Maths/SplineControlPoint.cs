namespace Rb.Core.Maths
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
		public Point3 Position
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
		public CurveFrame Frame
		{
			get { return m_Frame; }
		}

		/// <summary>
		/// Spline that owns this control point
		/// </summary>
		public Curve Owner
		{
			get { return m_Owner; }
		}

		/// <summary>
		/// Fraction along the spline that this control point is defined at (always a whole number)
		/// </summary>
		public float T
		{
			get { return m_Index; }
		}


		#endregion

		#region	Construction

		/// <summary>
		/// Sets up this control point
		/// </summary>
		public SplineControlPoint( Curve owner, int index )
		{
			m_Owner = owner;
			m_Index	= index;
			m_Frame	= m_Owner.EvaluateFrame( index );

			owner.OnChangedEvent += OnOwnerSplineChanged;
		}

		#endregion

		#region	Private stuff

		private readonly Curve	m_Owner;
		private readonly int	m_Index;
		private CurveFrame		m_Frame;

		/// <summary>
		/// Delegate, added to the Spline.OnChangedEvent. Re-evaluates the frame of the control point
		/// </summary>
		/// <param name="spline">Spline that changed</param>
		private void OnOwnerSplineChanged( Curve spline )
		{
			m_Frame = spline.EvaluateFrame( m_Index );
		}


		#endregion

	}
}
