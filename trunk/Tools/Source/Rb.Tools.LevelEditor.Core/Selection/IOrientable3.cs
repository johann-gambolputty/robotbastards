
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for editor objects that can be oriented to face a towards a given point
	/// </summary>
	public interface IOrientable3
	{

		/// <summary>
		/// Gets the current orientation of the object
		/// </summary>
		//	TODO: AP: Should return a quat
		float Orientation
		{
			get;
			set;
		}

		/// <summary>
		/// Turns this object to face a given point
		/// </summary>
		/// <param name="pt">Point to ace</param>
		void Face( Point3 pt );
	}
}
