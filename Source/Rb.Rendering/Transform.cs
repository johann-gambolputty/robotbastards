
namespace Rb.Rendering
{
    /// <summary>
    /// Transform types
    /// </summary>
    public enum Transform
    {
        /// <summary>
        /// Transforms vertices from local space to world space
        /// </summary>
        LocalToWorld,

        /// <summary>
        /// Transforms vertices from world space to view space
        /// </summary>
        WorldToView,

        /// <summary>
        /// Transforms vertices from view space to screen space (projective transform)
        /// </summary>
        ViewToScreen,

        /// <summary>
        /// Texture unit 0 transform
        /// </summary>
        Texture0,

        /// <summary>
        /// Texture unit 1 transform
        /// </summary>
        Texture1,

        /// <summary>
        /// Texture unit 2 transform
        /// </summary>
        Texture2,

        /// <summary>
        /// Texture unit 3 transform
        /// </summary>
        Texture3,

        /// <summary>
        /// Texture unit 4 transform
        /// </summary>
        Texture4,

        /// <summary>
        /// Texture unit 5 transform
        /// </summary>
        Texture5,

        /// <summary>
        /// Texture unit 6 transform
        /// </summary>
        Texture6,

        /// <summary>
        /// Texture unit 7 transform
        /// </summary>
        Texture7,

        /// <summary>
        /// Total number of transforms
        /// </summary>
        Count
    }
}
