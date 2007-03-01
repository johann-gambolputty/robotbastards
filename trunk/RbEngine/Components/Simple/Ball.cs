using System;
using System.Collections;
using RbEngine.Maths;
using RbEngine.Rendering;

namespace RbEngine.Components.Simple
{
	/// <summary>
	/// A ball. Just for testing really (which is why it implements Scene.ISceneRenderable (using the ShapeRenderer singleton and bodgy render states),
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
					.SetPolygonRenderingMode( PolygonRenderMode.Lines )
					.DisableCap( RenderStateFlag.CullFrontFaces )
					.DisableCap( RenderStateFlag.CullBackFaces )
					.SetDepthOffset( -1.0f );

			m_FilledState =
				RenderFactory.Inst.NewRenderState( )
					.SetColour( System.Drawing.Color.YellowGreen )
					.EnableLighting( )
					.SetShadeMode( PolygonShadeMode.Flat );
		}

		#region IXmlLoader Members

		/// <summary>
		/// Parses the XML element that was responsible for creating this object
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
		}

		/// <summary>
		/// Handles <position x="[float]" y="[float]" z="[float]"/> and <radius value="[float]"/> elements
		/// </summary>
		public bool ParseElement( System.Xml.XmlElement element )
		{
			if ( element.Name == "position" )
			{
				Point3 centre = new Point3
				(
					float.Parse( element.GetAttribute( "x" ) ),
					float.Parse( element.GetAttribute( "y" ) ),
					float.Parse( element.GetAttribute( "z" ) )
				);

				m_Sphere.Centre = centre;
				return true;
			}
			else if ( element.Name == "radius" )
			{
				m_Sphere.Radius = float.Parse( element.GetAttribute( "value" ) );
				return true;
			}
			return false;
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
		/// Rendering.IApplicable objects to apply before rendering
		/// </summary>
		public ApplianceList	PreRenderList
		{
			get
			{
				return m_PreRenders;
			}
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( long renderTime )
		{
			m_PreRenders.Apply( );
			Renderer.Inst.PushRenderState( m_LineState );
			ShapeRenderer.Inst.DrawSphere( m_Sphere.Centre, m_Sphere.Radius );
			m_FilledState.Apply( );
			ShapeRenderer.Inst.DrawSphere( m_Sphere.Centre, m_Sphere.Radius );
			Renderer.Inst.PopRenderState( );
		}

		#endregion

		private ApplianceList	m_PreRenders	= new ApplianceList( );
		private Maths.Sphere3	m_Sphere		= new Sphere3( new Point3( ), 10.0f );
		private RenderState		m_LineState;
		private RenderState		m_FilledState;
	}
}
