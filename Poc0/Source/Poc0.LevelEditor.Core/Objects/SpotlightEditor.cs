using System;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.Rendering.Lights;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a spotlight
	/// </summary>
	[Serializable]
	public class SpotlightEditor : ObjectEditor, IPlaceableObjectEditor
	{
		/// <summary>
		/// Gets/sets the position of this object
		/// </summary>
		public Point3 Position
		{
			get { return m_PositionModifier.Position; }
			set { m_PositionModifier.Position = value; }
		}

		/// <summary>
		/// Spotlight direction
		/// </summary>
		public float Facing
		{
			get { return m_FacingModifier.Angle; }
			set { m_FacingModifier.Angle = value; }
		}

		/// <summary>
		/// Gets/sets the declination of the spotlight vector. Valid over the range (-1,1), where -1 is straight down, and 1 is straight up
		/// </summary>
		public float Declination
		{
			get { return m_FacingModifier.Declination; }
			set
			{
				m_FacingModifier.Declination = Utils.Clamp( value, -1, 1 );
			}
		}

		/// <summary>
		/// Spotlight inner radius
		/// </summary>
		public float InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		/// <summary>
		/// Spotlight outer radius
		/// </summary>
		public float OuterRadius
		{
			get { return m_OuterRadius; }
			set { m_OuterRadius = value; }
		}

		/// <summary>
		/// Sets the arc of the spotlight
		/// </summary>
		public float ArcDegrees
		{
			get { return m_ArcDegrees; }
			set
			{
				m_ArcDegrees = value;
				OnObjectChanged( );
			}
		}

		/// <summary>
		/// Builds the associated object
		/// </summary>
		public override void Build( Scene scene )
		{
			PointLightSocket obj = new PointLightSocket( );

			Vector3 direction = m_FacingModifier.CreateDirectionVector( );

			SpotLight light = new SpotLight( Position, direction );
			light.InnerRadius = InnerRadius;
			light.OuterRadius = OuterRadius;
			light.ArcDegrees = ArcDegrees;

			obj.Light = light;

			scene.Objects.Add( obj );
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
						m_FacingModifier.Centre = Position;
						OnObjectChanged( );
					};

			m_FacingModifier = new AngleModifier( this, pt, 0 );
			m_FacingModifier.Changed += delegate { OnObjectChanged( ); };

			AddModifier( m_FacingModifier );
			AddModifier( m_PositionModifier );
		}

		#endregion

		#region Private members

		private PositionModifier	m_PositionModifier;
		private AngleModifier		m_FacingModifier;
		private float				m_InnerRadius	= 30;
		private float				m_OuterRadius	= 40;
		private float				m_ArcDegrees	= 90;

		#endregion

	}
}
