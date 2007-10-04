using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// 3D pick information
	/// </summary>
	public interface IPickInfo3 : IPickInfo
	{
		/// <summary>
		/// Gets the pick point
		/// </summary>
		Point3 PickPoint
		{
			get;
		}
	}
}
