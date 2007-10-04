using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// 2D pick information
	/// </summary>
	public interface IPickInfo2 : IPickInfo
	{
		/// <summary>
		/// Gets the pick point
		/// </summary>
		Point2 PickPoint
		{
			get;
		}
	}
}
