using System;
using Rb.Core.Components;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// A convex region representing a piece of floor in a <see cref="WallNode"/> leaf
	/// </summary>
	[Serializable]
	public class Floor : Node
	{
		/// <summary>
		/// Floor setup
		/// </summary>
		/// <param name="points">Floor polygon points</param>
		/// <param name="height">Floor height</param>
		public Floor( Point2[] points, float height )
		{
			m_Points = points;
			m_Height = height;
		}

		/// <summary>
		/// Points making up the floor polygon
		/// </summary>
		public Point2[] Points
		{
			get { return m_Points; }
		}

		/// <summary>
		/// Height of the floor
		/// </summary>
		public float Height
		{
			get { return m_Height; }
		}

		private readonly Point2[] m_Points;
		private readonly float m_Height;
	}
}
