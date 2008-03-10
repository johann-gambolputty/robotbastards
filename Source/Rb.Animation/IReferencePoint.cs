using Rb.Core.Maths;
using Rb.Rendering;

namespace Rb.Animation
{
	/// <summary>
	/// A reference point in the animation
	/// </summary>
	public interface IReferencePoint
	{
		/// <summary>
		/// Gets the name of this reference point
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the transform from this reference point
		/// </summary>
		Matrix44 Transform
		{
			get;
		}

		event RenderDelegate OnRender;
	}
}
