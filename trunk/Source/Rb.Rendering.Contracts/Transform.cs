
namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Transform types
	/// </summary>
	public enum Transform : int
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
		/// Blend weight 0 transform
		/// </summary>
		Blend0,

		/// <summary>
		/// Blend weight 1 transform
		/// </summary>
		Blend1,

		/// <summary>
		/// Blend weight 2 transform
		/// </summary>
		Blend2,

		/// <summary>
		/// Blend weight 3 transform
		/// </summary>
		Blend3,


		/// <summary>
		/// Total number of transforms
		/// </summary>
		Count
	}

	/// <summary>
	/// <see cref="Transform"/> helper class
	/// </summary>
	public static class Transforms
	{
		//	TODO: AP: Move to Rb.Rendering

		/// <summary>
		/// Gets the transform associated with a given texture unit
		/// </summary>
		public static Transform Texture( int unit )
		{
			return ( Transform.Texture0 + unit );
		}

		/// <summary>
		/// Gets the transform associated with a given blend weight
		/// </summary>
		public static Transform Blend( int weight )
		{
			return ( Transform.Blend0 + weight );
		}
	}
}
