using Rb.Core.Maths;

namespace Poc1.Universe
{
	public class UniTransform
	{
		public UniPoint3 Position
		{
			get { return m_Translation; }
			set { m_Translation = value; }
		}

		public Vector3 XAxis
		{
			get { return m_Axis[ 0 ]; }
		}

		public Vector3 YAxis
		{
			get { return m_Axis[ 1 ]; }
		}

		public Vector3 ZAxis
		{
			get { return m_Axis[ 2 ]; }
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