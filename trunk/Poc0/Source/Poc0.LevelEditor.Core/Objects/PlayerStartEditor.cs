
using System;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a player start position
	/// </summary>
	public class PlayerStartEditor
	{
		/// <summary>
		/// Event, invoked when properties are changed
		/// </summary>
		public EventHandler Changed;


		/// <summary>
		/// Sets up this editor
		/// </summary>
		public PlayerStartEditor( )
		{
			Position;
		}

		/// <summary>
		/// Player start position
		/// </summary>
		public Point3 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// Player start facing
		/// </summary>
		public float Facing
		{
			get { return m_Facing; }
			set { m_Facing = value; }
		}

		/// <summary>
		/// Builds the associated player start object
		/// </summary>
		public void Build( Scene scene )
		{
			PlayerStart obj = new PlayerStart( );
			obj.Position = m_Position;

			scene.Objects.Add( obj );
		}

		#region Private members

		private Point3 m_Position;
		private float m_Facing;

		#endregion
	}
}
