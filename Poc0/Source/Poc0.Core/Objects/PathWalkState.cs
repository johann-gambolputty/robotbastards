using System;
using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Manages the state of walking along a path
	/// </summary>
	public class PathWalkState
	{
		/// <summary>
		/// Sets up the path walk state
		/// </summary>
		/// <param name="path">Path to walk</param>
		/// <param name="pt">Position of the object about to walk the path</param>
		/// <exception cref="ArgumentNullException">Thrown if path is null</exception>
		public PathWalkState( IPath path, Point3 pt )
		{
			if ( path == null )
			{
				throw new ArgumentNullException( "path" );
			}

			m_Path = path;
			m_T = path.GetClosestPoint( pt );
			path.GetFrame( m_T, out m_Point, out m_Direction );
		}

		/// <summary>
		/// Gets the path being walked
		/// </summary>
		public IPath Path
		{
			get { return m_Path; }
		}

		/// <summary>
		/// Gets/sets the reverse flag
		/// </summary>
		/// <remarks>
		/// If true, then the path is walked backwards
		/// </remarks>
		public bool Reverse
		{
			get { return m_Reverse; }
			set { m_Reverse = value; }
		}

		/// <summary>
		/// Current position on the path
		/// </summary>
		public Point3 Point
		{
			get { return m_Point; }
		}
	
		/// <summary>
		/// Current direction on the path
		/// </summary>
		public Vector3 Direction
		{
			get { return m_Direction; }
		}

		/// <summary>
		/// Updates the walk state
		/// </summary>
		/// <param name="distance"></param>
		public void Update( float distance )
		{
			m_T = m_Path.Move( m_T, distance, m_Reverse );
			m_Path.GetFrame( m_T, out m_Point, out m_Direction );
		}

		#region Private members

		private float			m_T;
		private Point3			m_Point;
		private Vector3			m_Direction;
		private bool			m_Reverse;
		private readonly IPath	m_Path;

		#endregion
	}
}
