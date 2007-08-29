using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.LevelEditor.Core.Actions
{
	public interface IAction
	{
		void Undo( );
		void Redo( );
	}
}
