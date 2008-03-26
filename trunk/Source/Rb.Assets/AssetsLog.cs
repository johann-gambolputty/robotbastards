using Rb.Log;

namespace Rb.Assets
{
	/// <summary>
	/// Static log class for assets
	/// </summary>
	/// <example>
	/// AssetsLog.Error( "Asset {0} does not exist", assetName );
	/// </example>
	public class AssetsLog : StaticTag<AssetsLog>
	{
		public override string TagName
		{
			get { return "Assets"; }
		}
	}
}
