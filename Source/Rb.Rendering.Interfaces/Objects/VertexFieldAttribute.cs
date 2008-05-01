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
		/// <param name="semantic">The vertex field semantic tag</param>
		public VertexFieldAttribute( VertexFieldSemantic semantic )
		{
			m_Semantic = semantic;
		}

		/// <summary>
		/// Gets the associated vertex field
		/// </summary>
		public VertexFieldSemantic Semantic
		{
			get { return m_Semantic; }
		}

		#region Private Members

		private readonly VertexFieldSemantic m_Semantic; 

		#endregion
	}
}
