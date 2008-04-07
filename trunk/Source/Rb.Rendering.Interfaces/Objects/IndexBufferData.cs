
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
		/// <param name="format">Index buffer format</param>
		/// <param name="indices">Index buffer contents</param>
		public IndexBufferData( IndexBufferFormat format, int[] indices )
		{
			m_Format = format;
			m_Indices = indices;
		}

		/// <summary>
		/// Gets the format of the index buffer data
		/// </summary>
		public IndexBufferFormat Format
		{
			get { return m_Format; }
		}

		/// <summary>
		/// Gets the index buffer contents
		/// </summary>
		public int[] Indices
		{
			get { return m_Indices; }
		}

		private readonly IndexBufferFormat m_Format;
		private readonly int[] m_Indices;
	}
}
