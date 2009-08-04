using Goo.Core.Mvc;

namespace Goo.Test
{
	class TestController : ControllerBase
	{
		public TestController( ITestView view ) :
			base( view )
		{
		}
	}
}
