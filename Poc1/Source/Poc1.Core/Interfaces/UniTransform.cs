using Rb.Core.Maths;

namespace Poc1.Core.Interfaces
{
	/// <summary>
	/// Universe transform
	/// </summary>
	public class UniTransform
	{
		/// <summary>
		/// Get/sets transform position
		/// </summary>
		public UniPoint3 Position
		{
			get { return m_Translation; }
			set { m_Translation = value; }
		}

		/// <summary>
		/// Gets the transform X axis
		/// </summary>
		public Vector3 XAxis
		{
			get { return m_Axis[ 0 ]; }
		}

		/// <summary>
		/// Gets the transform Y axis
		/// </summary>
		public Vector3 YAxis
		{
			get { return m_Axis[ 1 ]; }
		}

		/// <summary>
		/// Gets the transform Z axis
		/// </summary>
		public Vector3 ZAxis
		{
			get { return m_Axis[ 2 ]; }
		}

		/// <summary>
		/// Sets the coordinate frame
		/// </summary>
		public void Set( Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			m_Axis[ 0 ] = xAxis;
			m_Axis[ 1 ] = yAxis;
			m_Axis[ 2 ] = zAxis;
		}

		/// <summary>
		/// Sets the coordinate frame
		/// </summary>
		public void Set( UniPoint3 pt, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			Position = pt;
			m_Axis[ 0 ] = xAxis;
			m_Axis[ 1 ] = yAxis;
			m_Axis[ 2 ] = zAxis;
		}

		#region Private Members

		private UniPoint3	m_Translation = new UniPoint3( );
		private Vector3[]	m_Axis = new Vector3[ ]
			{
				new Vector3( 1, 0, 0 ),
				new Vector3( 0, 1, 0 ),
				new Vector3( 0, 0, 1 ),
			};

		#endregion
	}
}
