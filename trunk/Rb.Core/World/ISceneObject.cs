
namespace Rb.Core.World
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
        /// Sets the scene that this object was created for
        /// </summary>
        /// <param name="scene">Scene</param>
        public void SetSceneContext( Scene scene );
    }
}