
using Goo.Core.Mvc;
using Rb.Core.Utils;

namespace Goo.Test
{
	public interface ITestView : IView
	{
		event ActionDelegates.Action Button1Clicked;
	}
}
