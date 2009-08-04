using Goo.Common.WinForms.Host;
using Goo.Common.WinForms.Layouts.Dialogs;
using Goo.Core.Services.Workspaces;
using Goo.Core.Ui.WinForms.Commands.Menus;
using Goo.Core.Ui.WinForms.MagicDocking.Layouts.Docking;

namespace Goo.Test
{
	class TestHost : Host
	{
		public TestHost( ) :
			base( "Test Host" )
		{
		}

		protected override void AddStartupServices( )
		{
			base.AddStartupServices( );
			IActiveWorkspaceService activeWsService = Environment.EnsureGetService<IActiveWorkspaceService>( );
			Environment.AddService( new DialogService( Form, activeWsService ) );
			Environment.AddService( new MenuService( Form, Environment ) );
			Environment.AddService( new DockingService( Form ) );
		}
	}
}
