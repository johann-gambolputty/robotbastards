using System;
using Rb.Core.Components;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// BSP node representing a wall
	/// </summary>
	/// <remarks>
	/// For this proof of concept, there's a really simple 2D BSP structure that underlies rendering, physics
	/// and navigation. More complex proofs will have separate structures for this purpose.
	/// 
	/// It's misleading that WallNode inherits from <see cref="Rb.Core.Components.Node"/> - the base class is
	/// used to store graphics, physics and AI related interfaces. The actual BSP node connections are stored
	/// in the properties <see cref="InFront"/> and <see cref="Behind"/>
	/// </remarks>
	[Serializable]
	public class WallNode : Node
	{

		/// <summary>
		/// Internal node setup
		/// </summary>
		/// <param name="start">Wall start</param>
		/// <param name="end">Wall end</param>
		/// <param name="height">Height of the wall</param>
		public WallNode( Point2 start, Point2 end, float height )
		{
			m_Height = height;
			m_Start = start;
			m_End = end;
			m_Floor = null;
		}

		/// <summary>
		/// Floor node setup
		/// </summary>
		/// <param name="start">Wall start</param>
		/// <param name="end">Wall end</param>
		/// <param name="height">Height of the wall</param>
		/// <param name="floor">The floor region at this node</param>
		/// <remarks>
		/// Floor nodes are nodes in the BSP tree that have no further walls in front of them. The intersection
		/// of the planes of all nodes from a floor node to the root node is a convex region, represented by
		/// the Floor object.
		/// </remarks>
		public WallNode( Point2 start, Point2 end, float height, Floor floor )
		{
			m_Height = height;
			m_Start = start;
			m_End = end;
			m_Floor = floor;
		}

		/// <summary>
		/// Height of the wall
		/// </summary>
		public float Height
		{
			 get { return m_Height; }
		}

		/// <summary>
		/// Start point of the wall
		/// </summary>
		public Point2 Start
		{
			get { return m_Start; }
		}

		/// <summary>
		/// End point of the wall
		/// </summary>
		public Point2 End
		{
			get { return m_End; }
		}

		/// <summary>
		/// Subtree of nodes in front of this wall
		/// </summary>
		public WallNode InFront
		{
			get { return m_InFront; }
			set
			{
				if ( ( m_Floor != null ) && ( value != null ) )
				{
					throw new ArgumentException( "Floor nodes cannot have in-front subtrees", "value" );
				}

				m_InFront = value;
			}
		}

		/// <summary>
		/// Subtree of nodes behind this wall
		/// </summary>
		public WallNode Behind
		{
			get { return m_Behind; }
			set { m_Behind = value; }
		}

		/// <summary>
		/// The floor at this node
		/// </summary>
		public Floor Floor
		{
			get { return m_Floor; }
		}

		private WallNode m_InFront;
		private WallNode m_Behind;
		private readonly float m_Height;
		private readonly Point2 m_Start;
		private readonly Point2 m_End;
		private readonly Floor m_Floor;
	}
}
