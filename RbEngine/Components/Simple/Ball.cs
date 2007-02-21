using System;
using RbEngine.Maths;
using RbEngine.Rendering;

namespace RbEngine.Components.Simple
{
	/// <summary>
	/// A ball. Just for testing really (which is why it implements IRender (using the ShapeRenderer singleton and bodgy render states),
	/// instead of deferring to a composite)
	/// </summary>
	public class Ball : Component, Maths.IRay3Intersector, IXmlLoader, Scene.ISceneRenderable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Ball( )
		{
			m_LineState = 
				RenderFactory.Inst.NewRenderState( )
					.SetPolygonRenderingMode( PolygonRenderMode.kLines )
					.DisableCap( RenderStateFlag.kCullFrontFaces )
					.DisableCap( RenderStateFlag.kCullBackFaces )
					.SetDepthOffset( -1.0f );

			m_FilledState =
				RenderFactory.Inst.NewRenderState( )
					.SetColour( System.Drawing.Color.YellowGreen )
					.EnableLighting( )
					.SetShadeMode( PolygonShadeMode.Flat );
		}

		#region IXmlLoader Members

		/// <summary>
		/// Handles <position x="[float]" y="[float]" z="[float]"/> and <radius value="[float]"/> elements
		/// </summary>
		/// <param name="reader">Xml reader</param>
		public void ParseElement( System.Xml.XmlReader reader )
		{
			if ( reader.Name == "position" )
			{
				m_Sphere.Centre.X = float.Parse( reader.GetAttribute( "x" ) );
				m_Sphere.Centre.Y = float.Parse( reader.GetAttribute( "y" ) );
				m_Sphere.Centre.Z = float.Parse( reader.GetAttribute( "z" ) );
			}
			else if ( reader.Name == "radius" )
			{
				m_Sphere.Radius = float.Parse( reader.GetAttribute( "value" ) );
			}
		}

		#endregion

		#region	IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool				TestIntersection( Ray3 ray )
		{
			//	TODO: This is lazy
			return Maths.Intersection.GetIntersection( ray, m_Sphere ) != null;
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Ray3Intersection	GetIntersection( Ray3 ray )
		{
			return Maths.Intersection.GetIntersection( ray, m_Sphere );
		}

		#endregion

		#region ISceneRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( float delta )
		{
			Renderer.Inst.PushRenderState( m_LineState );
			ShapeRenderer.Inst.RenderSphere( m_Sphere.Centre, m_Sphere.Radius );
			m_FilledState.Apply( );
			ShapeRenderer.Inst.RenderSphere( m_Sphere.Centre, m_Sphere.Radius );
			Renderer.Inst.PopRenderState( );
		}

		#endregion

		private Maths.Sphere3	m_Sphere	= new Sphere3( new Point3( ), 10.0f );
		private RenderState		m_LineState;
		private RenderState		m_FilledState;
	}
}
