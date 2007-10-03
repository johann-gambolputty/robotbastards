using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	public interface IAction
	{
		void Undo( );
		void Redo( );
	}
}
