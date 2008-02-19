using System;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a player start position
	/// </summary>
	[Serializable]
	public class PlayerStartEditor : ObjectEditor, IPlaceableObjectEditor
	{
		/// <summary>
		/// Builds the associated player start object
		/// </summary>
		public override void Build( Scene scene )
		{
			PlayerStart obj = new PlayerStart( );
			obj.Position = Position;
			obj.Id = Id;

			scene.Objects.Add( obj );
		}

		/// <summary>
		/// Gets/sets the position of this object
		/// </summary>
		public Point3 Position
		{
			get { return m_PositionModifier.Position; }
			set
			{
				if ( m_PositionModifier.Position != value )
				{
					m_PositionModifier.Position = value;
					OnObjectChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the angle that the player starts at
		/// </summary>
		public float Angle
		{
			get { return m_AngleModifier.Angle; }
			set
			{
				if ( m_AngleModifier.Angle != value )
				{
					m_AngleModifier.Angle = value;
					OnObjectChanged( );
				}
			}
		}

		#region IPlaceableObjectEditor Members

		/// <summary>
		/// Places this object at a line intersection
		/// </summary>
		public void Place( ILineIntersection intersection )
		{
			Point3 pt = ( ( Line3Intersection )intersection ).IntersectionPosition;

			m_PositionModifier = new PositionModifier( this, pt );
			m_PositionModifier.Changed +=
				delegate
				{
					m_AngleModifier.Centre = m_PositionModifier.Position;
					OnObjectChanged( );
				};

			m_AngleModifier = new AngleModifier( this, pt, 0 );
			m_AngleModifier.Changed += delegate { OnObjectChanged( ); };

			AddModifier( m_AngleModifier );
			AddModifier( m_PositionModifier );
		}

		#endregion

		#region Private members

		private PositionModifier m_PositionModifier;
		private AngleModifier m_AngleModifier;

		#endregion
	}
}
