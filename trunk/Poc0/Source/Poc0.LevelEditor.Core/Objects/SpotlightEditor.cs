using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.Rendering.Lights;
using Rb.World;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// Edits a spotlight
	/// </summary>
	public class SpotlightEditor : PlaceableObjectEditor
	{
		/// <summary>
		/// Spotlight direction
		/// </summary>
		public Vector3 Direction
		{
			get { return m_Direction; }
			set
			{
				value.Normalise( );
				bool changed = ( m_Direction != value );
				m_Direction = value;
				if ( changed )
				{
					OnObjectChanged( );
				}
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

		private float	m_InnerRadius;
		private float	m_OuterRadius;
		private Vector3 m_Direction;
	}
}
