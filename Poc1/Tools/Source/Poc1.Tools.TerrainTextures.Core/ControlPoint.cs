
namespace Poc1.Tools.TerrainTextures.Core
{
	public class ControlPoint
	{
		public ControlPoint( float position, float value )
		{
			m_Position = position;
			m_Value = value;
		}

		public float Value
		{
			get { return m_Value; }
			set { m_Value = value < 0 ? 0 : ( value > 1 ? 1 : value ); }
		}

		public float Position
		{
			get { return m_Position; }
			set { m_Position = value < 0 ? 0 : ( value > 1 ? 1 : value ); }
		}

		#region Private Members

		private float m_Position;
		private float m_Value;

		#endregion
	}
}
