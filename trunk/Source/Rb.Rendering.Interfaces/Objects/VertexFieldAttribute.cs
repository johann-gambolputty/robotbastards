using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Marks a class field as a vertex field
	/// </summary>
	public class VertexFieldAttribute : Attribute
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="field">The vertex field</param>
		public VertexFieldAttribute( VertexFieldSemantic field )
		{
			m_Field = field;
		}

		/// <summary>
		/// Gets the associated vertex field
		/// </summary>
		public VertexFieldSemantic VertexField
		{
			get { return m_Field; }
		}

		private readonly VertexFieldSemantic m_Field;
	}
}
