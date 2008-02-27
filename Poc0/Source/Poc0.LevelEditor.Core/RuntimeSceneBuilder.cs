using Rb.Core.Utils;
using Rb.World;
using Rb.World.Services;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Responsible for creating the scene used by the runtime
	/// </summary>
	public static class RuntimeSceneBuilder
	{
		/// <summary>
		/// Creates a new scene
		/// </summary>
		public static Scene CreateScene( )
		{
			Scene scene = new Scene( );

			//	Populate runtime scene
			scene.AddService( new LightingService( ) );

			IUpdateService updater = new UpdateService( );
			updater.AddClock( new Clock( "updateClock", 30, true ) );
			updater.AddClock( new Clock( "animationClock", 60, true ) );
			scene.AddService( updater );

			return scene;
		}
	}
}
