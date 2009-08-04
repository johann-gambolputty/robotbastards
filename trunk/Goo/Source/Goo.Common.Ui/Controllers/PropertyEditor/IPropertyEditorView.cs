
using Goo.Core.Mvc;

namespace Goo.Common.Ui.Controllers.PropertyEditor
{
	public interface IPropertyEditorView : IView
	{
		object SelectedObject
		{
			get; set;
		}

		object[] SelectedObjects
		{
			get; set;
		}
	}
}
