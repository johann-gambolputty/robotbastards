
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// 3D pick box
	/// </summary>
	public class PickInfoBox3 : IPickInfo
	{
		/// <summary>
		/// Builds this box
		/// </summary>
		/// <param name="topLeftBack">Minimum coordinate corner</param>
		/// <param name="bottomRightFront">Maximum coordinate corner</param>
		public PickInfoBox3( Point3 topLeftBack, Point3 bottomRightFront )
		{
		}
	}
}
