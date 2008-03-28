using Rb.Log;

namespace Rb.Rendering
{
	/// <summary>
	/// Static log class for graphics
	/// </summary>
	/// <example>
	/// GraphicsLog.Error( "No tea" );
	/// </example>
	public class GraphicsLog : StaticTag< GraphicsLog >
	{
		public override string TagName
		{
			get { return "Graphics"; }
		}
	}
}
