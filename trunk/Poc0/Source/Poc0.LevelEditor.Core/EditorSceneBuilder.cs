using System;
using Poc0.LevelEditor.Core.Geometry;
using Poc0.LevelEditor.Core.Rendering;
using Rb.Core.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.World;
using Rb.World.Services;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Responsible for creating the scene used by the level editor
	/// </summary>
	public static class EditorSceneBuilder
	{
		/// <summary>
		/// Creates a new scene
		/// </summary>
		public static Scene CreateScene( )
		{
			Scene scene = new Scene( );
			
			//	Add raycast service to scene
			IRayCastService rayCaster = new RayCastService( );
			rayCaster.AddIntersector( RayCastLayers.Grid, new Plane3( new Vector3( 0, 1, 0 ), 0 ) );
			scene.AddService( rayCaster );

			//	Add material set service to scene
			ISource materialSetSource = new Location( "Editor/DefaultMaterialSet.components.xml" );
			MaterialSet materials = MaterialSet.Load( materialSetSource, false );

			scene.AddService( materials );

			//	TODO: AP: Fix Z order rendering cheat
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< GroundPlaneGrid >( ) );
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< LevelGeometry >( ) );

			return scene;
		}
	}
}
