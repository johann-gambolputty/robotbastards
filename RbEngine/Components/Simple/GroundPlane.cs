using System;

namespace RbEngine.Components.Simple
{
	/// <summary>
	/// The ground plane (XZ plane)
	/// </summary>
	public class GroundPlane : Component, Maths.IRay3Intersector, Scene.ISceneObject
	{

		/// <summary>
		/// Constructor. Adds an implementation of Rendering.Composites.GroundPlaneArea
		/// </summary>
		public GroundPlane( )
		{
		//	m_Graphics = ( Rendering.IRender )Rendering.RenderFactory.Inst.NewComposite( typeof( Rendering.Composites.GroundPlaneArea ) );
		}

		#region IRay3Intersector Members

		/// <summary>
		/// Gets the layer(s) that this object belongs to. Returns 0 (i.e. included in all raycasts) because this is a crappy test object
		/// </summary>
		public ulong			Layer
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool TestIntersection( Maths.Ray3 ray )
		{
			return Maths.Intersection.TestRayIntersection( ray, m_Plane );
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Maths.Ray3Intersection GetIntersection( Maths.Ray3 ray )
		{
			return Maths.Intersection.GetRayIntersection( ray, m_Plane );
		}

		#endregion

		/*
		#region ISceneRenderable Members

		/// <summary>
		/// Renders the ground plane
		/// </summary>
		/// <param name="renderTime">Frame time</param>
		public void Render( long renderTime )
		{
			m_Graphics.Render( );
		}

		#endregion
		*/

	//	private Rendering.IRender	m_Graphics;
		private Maths.Plane3		m_Plane = new Maths.Plane3( new Maths.Vector3( 0, 1, 0 ), 0 );

		#region ISceneObject Members

		/// <summary>
		/// Adds this object to the scene's ray cast manager
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			Scene.IRayCaster rayCaster = ( Scene.IRayCaster )db.GetSystem( typeof( Scene.IRayCaster ) );
			if ( rayCaster != null )
			{
				rayCaster.Add( this );
			}
		}

		/// <summary>
		/// Removes this object from the scene's ray cast manager
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			Scene.IRayCaster rayCaster = ( Scene.IRayCaster )db.GetSystem( typeof( Scene.IRayCaster ) );
			if ( rayCaster != null )
			{
				rayCaster.Remove( this );
			}
		}

		#endregion
	}
}
