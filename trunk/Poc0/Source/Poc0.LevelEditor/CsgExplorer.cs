using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Poc0.LevelEditor.Core.EditModes;

namespace Poc0.LevelEditor
{
	public partial class CsgExplorer : UserControl
	{
		public CsgExplorer( )
		{
			InitializeComponent( );

			EditModeContext.Instance.PostSetup += PostSetup;
		}

		private void PostSetup( EditModeContext context )
		{
			
		}
	}
}
