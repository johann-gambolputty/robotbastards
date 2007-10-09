
namespace Rb.World
{
    /// <summary>
    /// Interface for objects that like to know the scene that they are being created for
    /// </summary>
    /// <remarks>
    /// Objects implementing this interface are notified of the scene they are being created for, by the SceneBuilder builder object.
    /// </remarks>
    public interface ISceneObject
    {
        /// <summary>
        /// Called when this object is added to the specified scene
        /// </summary>
        /// <param name="scene">Scene object</param>
        void AddedToScene( Scene scene );

		/// <summary>
		/// Called when this object is removed from the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		void RemovedFromScene( Scene scene );
    }
}