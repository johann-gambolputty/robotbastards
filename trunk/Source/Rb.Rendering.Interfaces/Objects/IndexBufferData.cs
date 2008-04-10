
using System.Collections.Generic;

namespace Rb.Rendering.Interfaces.Objects
{

	/// <summary>
	/// Index buffer creation data
	/// </summary>
	public class IndexBufferData
	{
		/// <summary>
		/// Creates an IndexBufferData object from an index array
		/// </summary>
		public static IndexBufferData FromIndexArray( int[] array )
		{
			return new IndexBufferData( new IndexBufferFormat( IndexBufferIndexSize.Int32 ), array );
		}

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
