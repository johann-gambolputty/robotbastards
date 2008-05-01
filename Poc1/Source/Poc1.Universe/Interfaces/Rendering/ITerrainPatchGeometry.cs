
using System;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Interface for a patch's geometry at a given level of detail
	/// </summary>
	public unsafe interface ITerrainPatchGeometry : IDisposable
	{
		/// <summary>
		/// Gets the level of detail of this patch (0=highest)
		/// </summary>
		int LevelOfDetail
		{
			get;
		}

		/// <summary>
		/// Gets the resolution of this patch (width or height of square patch)
		/// </summary>
		int Resolution
		{
			get;
		}

		/// <summary>
		/// Gets the index of the first vertex buffer in the planet vertex buffer, used by this patch
		/// </summary>
		int FirstVertexIndex
		{
			get;
		}

		/// <summary>
		/// Sets up the patch index buffer
		/// </summary>
		void SetIndexBuffer( PrimitiveType prim, int[] indices );

		/// <summary>
		/// Locks the vertex buffer
		/// </summary>
		/// <param name="read">If true, vertex buffer is locked for reading</param>
		/// <param name="write">If true, vertex buffer is locked for writing</param>
		/// <returns>Returns a pointer into the vertex buffer</returns>
		TerrainVertex* LockVertexBuffer( bool read, bool write );

		/// <summary>
		/// Unlocks the vertex buffer, commiting the changes made by the last <see cref="LockVertexBuffer"/>
		/// </summary>
		void UnlockVertexBuffer( );

		/// <summary>
		/// Draws the patch geometry
		/// </summary>
		void Draw( );
	}
}