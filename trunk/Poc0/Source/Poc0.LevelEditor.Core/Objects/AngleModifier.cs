using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// For changing the position and angle of a game object
	/// </summary>
	[Serializable]
	internal class AngleModifier : IRay3Intersector, IPickable, IOrientable3, IRenderable, ISelectable, ISelectionModifier
	{
		/// <summary>
		/// Event, invoked when the angle changes
		/// </summary>
		public event EventHandler Changed;

		/// <summary>
		/// Sets up this modifier
		/// </summary>
		/// <param name="owner">Positioning interface of the game object</param>
		/// <param name="pos">Position to place the new object at</param>
		/// <param name="angle">Initial angle</param>
		public AngleModifier( object owner, Point3 pos, float angle )
		{
			m_Owner = owner;
			m_Centre = pos;
			m_Angle = angle;
			m_Spinner = CalculateSpinnerPosition( );
			
			IRayCastService rayCaster = EditorState.Instance.CurrentScene.GetService< IRayCastService >( );
			rayCaster.AddIntersector( RayCastLayers.Entity, this );
		}

		/// <summary>
		/// Creates a normalized direction vector from the angle and declination
		/// </summary>
		public Vector3 CreateDirectionVector( )
		{
			float s = Angle;
			float t = ( Declination + 1 ) * Constants.HalfPi;

			float x = Functions.Sin( s ) * Functions.Sin( t );
			float y = Functions.Cos( t );
			float z = Functions.Cos( s ) * Functions.Sin( t );
			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Gets/sets the declination of the modifier
		/// </summary>
		public float Declination
		{
			get { return m_Declination; }
			set
			{
				bool changed = m_Declination != value;
				m_Declination = value;
				if ( changed )
				{
					m_Spinner = CalculateSpinnerPosition( );
					if ( Changed != null )
					{
						Changed( this, null );
					}
				}
			}
		}

		/// <summary>
		/// Gets/sets the centre point of the modifier
		/// </summary>
		public Point3 Centre
		{
			get { return m_Centre; }
			set
			{
				m_Centre = value;
				m_Spinner = CalculateSpinnerPosition( );
			}
		}

		/// <summary>
		/// Gets/sets the angle
		/// </summsary>
		public float Angle
		{
			get { return m_Angle; }
			set
			{
				bool changed = ( m_Angle != value );
				m_Angle = value;
				if ( changed )
				{
					m_Spinner = CalculateSpinnerPosition( );
					if ( Changed != null )
					{
						Changed( this, null );
					}
				}
			}
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			Draw.IPen drawProps = ms_Unselected;
			if ( Selected )
			{
				drawProps = ms_Selected;
			}
			else if ( Highlighted )
			{
				drawProps = ms_Highlighted;
			}

			Graphics.Draw.Line( drawProps, Centre, m_Spinner );
			Graphics.Draw.Circle( drawProps, m_Spinner, Radius );
		}

		private const float Length = 3;
		private const float Radius = 0.4f;
		
		#region IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool TestIntersection( Ray3 ray )
		{
			Line3Intersection pick = Intersections3.GetRayIntersection( ray, m_Spinner, Vector3.YAxis );
			return pick == null ? false : pick.IntersectionPosition.DistanceTo( m_Spinner ) < Radius;
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Line3Intersection GetIntersection( Ray3 ray )
		{
			Line3Intersection pick = Intersections3.GetRayIntersection( ray, m_Spinner, Vector3.YAxis );
			if ( ( pick == null ) || ( pick.IntersectionPosition.DistanceTo( m_Spinner ) > Radius ) )
			{
			    return null;
			}
			pick.IntersectedObject = this;
			return pick;
		}

		#endregion

		private readonly object m_Owner;
		private float m_Angle;
		private float m_Declination;
		private Point3 m_Centre;
		private Point3 m_Spinner;
		
		private readonly static Draw.IPen ms_Selected;
		private readonly static Draw.IPen ms_Highlighted;
		private readonly static Draw.IPen ms_Unselected;

		static AngleModifier( )
		{
			ms_Selected		= Graphics.Draw.NewPen( Color.Red, 1.5f );
			ms_Highlighted	= Graphics.Draw.NewPen( Color.Orange, 1.5f );
			ms_Unselected	= Graphics.Draw.NewPen( Color.Gray, 1.5f );
		}

		private Point3 CalculateSpinnerPosition( )
		{
			Vector3 vec = CreateDirectionVector( ) * Length;
			return Centre + vec;
		}

		#region ISelectionModifier Members

		public object SelectedObject
		{
			get { return m_Owner; }
		}

		#endregion

		#region ISelectable Members

		public bool Highlighted
		{
			get
			{
				return ( ( ISelectable )m_Owner ).Highlighted;
			}
			set
			{
				( ( ISelectable )m_Owner ).Highlighted = value;
			}
		}

		public bool Selected
		{
			get
			{
				return ( ( ISelectable )m_Owner ).Selected;
			}
			set
			{
				( ( ISelectable )m_Owner ).Selected = value;
			}
		}

		#endregion

		#region IOrientable3 Members

		/// <summary>
		/// Gets/sets the angle of this object (synonymous with <see cref="Angle"/> property)
		/// </summary>
		public float Orientation
		{
			get { return Angle; }
			set { Angle = value; }
		}

		/// <summary>
		/// Turns to face a position
		/// </summary>
		/// <param name="inputPos">Position to face</param>
		public void Face( Point3 inputPos )
		{
			Vector3 dir = inputPos - Centre;
			Angle = Functions.Atan2( dir.X, dir.Z );
		}

		#endregion

		#region IPickable Members

		/// <summary>
		/// Creates the action appropriate for this object
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns a move action</returns>
		public IPickAction CreatePickAction( ILineIntersection pick )
		{
			//	TODO: AP: Correct move action parameters
			return new FaceAction( new RayCastOptions( RayCastLayers.StaticGeometry ) );
		}

		/// <summary>
		/// Returns true if the specified action is a move action
		/// </summary>
		/// <param name="action">Action to check</param>
		/// <returns>true if action is a <see cref="FaceAction"/></returns>
		public bool SupportsPickAction( IPickAction action )
		{
			return action is FaceAction;
		}

		#endregion
	}
}
