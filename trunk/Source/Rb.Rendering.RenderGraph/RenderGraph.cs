using System.Collections.Generic;


namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Simple wrapper around a group of render node
	/// </summary>
	public class RenderGraph
	{

		#region Private Members

		private readonly List<IRenderNode> m_Nodes = new List<IRenderNode>( );

		#endregion
	}
}
