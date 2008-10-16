using NUnit.Framework;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Tests
{
	/// <summary>
	/// Tests 2d texturing
	/// </summary>
	public class Test2dTexturesBase
	{
		[Test]
		public void TestCreateFromBitmaps( )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			//	TODO: AP: ...
		}
	}
}
