namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Index buffer formats
	/// </summary>
	public enum IndexBufferIndexSize
	{
		Int16,
		Int32
	}

	public class IndexBufferFormat
	{
		public IndexBufferFormat( IndexBufferIndexSize size )
		{
			m_Size = size;
		}

		public IndexBufferIndexSize Size
		{
			get { return m_Size; }
		}

		public bool Static
		{
			get { return m_Static; }
			set { m_Static = value; }
		}

		private bool m_Static;
		private readonly IndexBufferIndexSize m_Size;
	}
}
