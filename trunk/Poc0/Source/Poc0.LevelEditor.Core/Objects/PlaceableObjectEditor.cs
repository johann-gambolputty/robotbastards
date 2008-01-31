using System;
using System.ComponentModel;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Base class for standard placeable objects
	/// </summary>
	public class PlaceableObjectEditor : ObjectEditor, IPlaceableObjectEditor, IPlaceable
	{
		/// <summary>
		/// Builds the associated player start object
		/// </summary>
		public override void Build( Scene scene )
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
				if ( m_Frame.Translation == value )
				{
					return;
				}

				Point3 oldPos = m_Frame.Translation;
				m_Frame.Translation = value;

				if ( PositionChanged != null )
				{
					PositionChanged( this, oldPos, value );
				}
				OnObjectChanged( );
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
				if ( value == Angle )
				{
					return;
				}

				float angle = value;
				float sinA = ( float )Math.Sin( angle );
				float cosA = ( float )Math.Cos( angle );
				Frame.ZAxis = new Vector3( cosA, 0, sinA );
				Frame.XAxis = new Vector3( -sinA, 0, cosA );

				OnObjectChanged( );
			}
		}

		/// <summary>
		/// Current coordinate frame
		/// </summary>
		[ReadOnly( true )]
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		#endregion

		#region Private members

		private readonly Matrix44 m_Frame = new Matrix44( );

		#endregion


		#region IPlaceableObjectEditor Members

		/// <summary>
		/// Places this object at a line intersection
		/// </summary>
		/// <param name="intersection">Line intersection</param>
		public void Place( ILineIntersection intersection )
		{
			Point3 pt = ( ( Line3Intersection )intersection ).IntersectionPosition;
			AddModifier( new PositionEditor( this, pt ) );
		}

		#endregion
	}
}
