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
			set
			{
				m_PositionModifier.Position = value;
				OnObjectChanged( );
			}
		}

		/// <summary>
		/// Spotlight direction
		/// </summary>
		public Vector3 Direction
		{
			get { return m_Direction; }
			set
			{
				m_Direction = value.MakeNormal( );
				OnObjectChanged( );
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
		/// Builds the associated object
		/// </summary>
		public override void Build( Scene scene )
		{
			PointLightSocket obj = new PointLightSocket( );

			SpotLight light = new SpotLight( Position, Direction );
			light.InnerRadius = InnerRadius;
			light.OuterRadius = OuterRadius;

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
			m_PositionModifier.Changed += delegate { OnObjectChanged( ); };
			AddModifier( m_PositionModifier );
		}

		#endregion

		#region Private members

		private PositionModifier	m_PositionModifier;
		private float				m_InnerRadius	= 30;
		private float				m_OuterRadius	= 40;
		private Vector3 			m_Direction		= new Vector3( 0, 0, 1 );

		#endregion

	}
}
