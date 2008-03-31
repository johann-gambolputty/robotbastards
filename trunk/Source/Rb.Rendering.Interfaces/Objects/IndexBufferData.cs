
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Index buffer creation data
	/// </summary>
	public class IndexBufferData
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="indices">Index buffer contents</param>
		public IndexBufferData( int[] indices )
		{
			m_Indices = indices;
		}

		/// <summary>
		/// Gets the index buffer contents
		/// </summary>
		public int[] Indices
		{
			get { return m_Indices; }
		}

		private readonly int[] m_Indices;
	}
}
