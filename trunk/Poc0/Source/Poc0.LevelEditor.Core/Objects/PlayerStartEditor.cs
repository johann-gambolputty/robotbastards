
using System;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a player start position
	/// </summary>
	public class PlayerStartEditor : IPlaceable
	{
		/// <summary>
		/// Builds the associated player start object
		/// </summary>
		public void Build( Scene scene )
		{
			PlayerStart obj = new PlayerStart( );
			obj.Position = Position;

			scene.Objects.Add( obj );
		}

		#region IPlaceable Members

		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Current position
		/// </summary>
		public Point3 Position
		{
			get { return Frame.Translation; }
			set
			{
				if ( PositionChanged == null )
				{
					Frame.Translation = value;
				}
				else
				{
					bool changed = ( m_Frame.Translation != value );

					Point3 oldPos = m_Frame.Translation;
					m_Frame.Translation = value;

					if ( changed )
					{
						PositionChanged( this, oldPos, m_Frame.Translation );
					}
				}
			}
		}

		/// <summary>
		/// Angle (rotation around y axis)
		/// </summary>
		public float Angle
		{
			get
			{
				return ( float )Math.Atan2( -Frame.ZAxis.Z, -Frame.ZAxis.X );
			}
			set
			{
				float angle = value;
				float sinA = ( float )Math.Sin( angle );
				float cosA = ( float )Math.Cos( angle );
				Frame.ZAxis = new Vector3( cosA, 0, sinA );
				Frame.XAxis = new Vector3( -sinA, 0, cosA );
			}
		}

		/// <summary>
		/// Current coordinate frame
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		#endregion


		#region Private members

		private readonly Matrix44 m_Frame = new Matrix44( );

		#endregion
	}
}
