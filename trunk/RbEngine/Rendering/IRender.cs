using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Symbolic constants for describing common positions in the object render order
	/// </summary>
	/// <seealso cref="IRender.GetRenderOrder">IRender.GetRenderOrder()</seealso>
	public enum RenderOrder
	{
		First	= 0,
		Default	= 50,
		Last	= 100
	};


	/// <summary>
	/// Compares the render order of two objects that implement the IRender interface
	/// </summary>
	public class RenderOrderComparison : System.Collections.IComparer
	{
		/// <summary>
		/// Comparison method
		/// </summary>
		/// <param name="x"> First object to compare </param>
		/// <param name="y"> Second object to compare </param>
		/// <returns> Returns -1 if x has a lower render order value than y, 1 if x is higher, 0 if they are the same</returns>
		public int Compare( Object x, Object y )
		{
			int xOrder = ( ( IRender )x ).GetRenderOrder( );
			int yOrder = ( ( IRender )y ).GetRenderOrder( );

			return ( xOrder < yOrder ) ? -1 : ( xOrder == yOrder ? 0 : 1 );
		}
	}

	/// <summary>
	/// Summary description for IRender.
	/// </summary>
	public interface IRender
	{
		/// <summary>
		/// Returns the position that this object should occupy in the render order
		/// </summary>
		/// <seealso>Rendering.RenderOrder</seealso>
		int GetRenderOrder( );

		/// <summary>
		/// 
		/// </summary>
		void Render( );
	}
}
