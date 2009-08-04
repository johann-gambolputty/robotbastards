using Goo.Common.Layouts.Dialogs;
using Goo.Common.Ui.Controllers.CompositeEditor;
using Goo.Common.Ui.Controllers.PropertyEditor;
using Goo.Common.Ui.WinForms.Controllers.CompositeEditor;
using Goo.Common.Ui.WinForms.Controllers.PropertyEditor;
using Goo.Core.Commands;
using Goo.Core.Environment;
using Goo.Core.Mvc;
using Goo.Core.Ui.Commands;
using Goo.Core.Ui.Commands.Menus;
using Goo.Core.Ui.Layouts.Docking;
using Goo.Core.Units;
using Rb.Core.Components;
using Rb.Interaction.Classes;

namespace Goo.Test
{
	class TestUnit : AbstractUnit, ICommandExecutor
	{
		public TestUnit( )
		{
			m_PropertyEditorControllerFactory =
				new DelegateControllerFactory
				(
					delegate( ControllerInitializationContext context )
						{
							return new PropertyEditorController( context.Environment, new PropertyEditorView( ) );
						}
				);
			m_CompositeEditorControllerFactory =
				new DelegateControllerFactory
				(
					delegate( ControllerInitializationContext context )
						{
							return CreateComponentEditorController( context );
						}
				);
		}

		public IControllerFactory TestControllerFactory
		{
			get { return m_TestControllerFactory; }
		}


		/// <summary>
		/// Called after initialization
		/// </summary>
		/// <param name="env">Environment</param>
		protected override void PostInitialize( IEnvironment env )
		{
			IMenuService menuService = env.EnsureGetService<IMenuService>( );

			IMenuGroup fileGroup = menuService[ DefaultCommandMenus.File.Group ];
			fileGroup.AddItem( DefaultCommandMenus.File.Exit );
			fileGroup.AddItem( DefaultCommandMenus.File.Close );
			fileGroup.AddItem( DefaultCommandMenus.File.Open );

			IMenuGroup viewGroup = menuService[ new MenuGroupInfo( "view", "&View", 10 ) ];
			viewGroup.AddItem( new MenuItemInfo( s_ShowTestViewAsDialog, 0 ) );
			viewGroup.AddItem( new MenuItemInfo( s_ShowTestViewAsDocked, 10 ) );
			viewGroup.AddItem( new MenuItemInfo( s_showComponentEditorAsDocked, 20 ) );
			viewGroup.AddItem( new MenuItemInfo( s_ShowPropertyEditorAsDocked, 20 ) );
		}

		/// <summary>
		/// Gets the controller factories associated with this unit
		/// </summary>
		protected override IControllerFactory[] ControllerFactories
		{
			get
			{
				return new IControllerFactory[]
					{
						TestControllerFactory,
						m_CompositeEditorControllerFactory,
						m_PropertyEditorControllerFactory
					};
			}
		}

		/// <summary>
		/// Gets the command executors associated with this unit
		/// </summary>
		protected override ICommandExecutor[] CommandExecutors
		{
			get { return new ICommandExecutor[] { this }; }
		}

		private IController CreateComponentEditorController( ControllerInitializationContext initContext )
		{
			Composite t0 = new Composite( );
				Composite t00 = new Composite( );
					Composite t000 = new Composite( );
					Composite t001 = new Composite( );
					t00.Add( t000 );
					t00.Add( t001 );
				Composite t01 = new Composite( );
				Composite t02 = new Composite( );
				t0.Add( t00 );
				t0.Add( t01 );
				t0.Add( t02 );

			CompositeEditorController controller = new CompositeEditorController
				(
					initContext.Environment,
					new CompositeEditorView( ),
					t0,
					new DefaultComponentUiElementFactory( ),
					new ComponentDependencies( ),
					new CompositeAggregator( )
				);
			return controller;
		}

		private readonly IControllerFactory m_PropertyEditorControllerFactory;
		private readonly IControllerFactory m_CompositeEditorControllerFactory;
		private readonly IControllerFactory m_TestControllerFactory = new DelegateControllerFactory( delegate { return new TestController( new TestView( ) ); } );
		private readonly static CommandGroup s_TestCommands = new CommandGroup( "Test Commands" );
		private readonly static Command s_ShowTestViewAsDialog = s_TestCommands.NewCommand( "showTestViewDialog", "Show Test View Dialog", "Shows the test view as a dialog" );
		private readonly static Command s_ShowTestViewAsDocked = s_TestCommands.NewCommand( "showTestViewDock", "Show Test View Docked", "Shows the test view as a docked window" );
		private readonly static Command s_showComponentEditorAsDocked = s_TestCommands.NewCommand( "showComponentEditorDialog", "Show Component Editor", "Shows the component editor" );
		private readonly static Command s_ShowPropertyEditorAsDocked = s_TestCommands.NewCommand( "showPropertyEditorDialog", "Show Property Editor", "Shows the property editor" );

		#region ICommandExecutor Members

		/// <summary>
		/// Returns the visibility state of a command (whether it can be executed, and if it appears in the UI)
		/// </summary>
		public void GetCommandAvailability( IEnvironment env, Command command, ref bool available, ref bool visible )
		{
			if ( command == s_ShowTestViewAsDialog )
			{
				available = true;
				visible = true;
			}
		}

		/// <summary>
		/// Executes a command
		/// </summary>
		public CommandExecutionResult Execute( IEnvironment env, Command command, CommandParameters parameters )
		{
			if ( command == DefaultCommands.File.Exit )
			{
				env.Host.Close( );
				return CommandExecutionResult.StopExecutingCommand;
			}
			if ( command == s_ShowTestViewAsDialog )
			{
				IDialogService dialogService = env.EnsureGetService<IDialogService>( );
				dialogService.Show( DefaultDialogFrameType.Ok, "Test View In A Dialog", m_TestControllerFactory.Create( new ControllerInitializationContext( env ) ).View );
				return CommandExecutionResult.StopExecutingCommand;
			}
			if ( command == s_ShowTestViewAsDocked  )
			{
				IDockingService dockService = env.EnsureGetService<IDockingService>( );
				dockService.Show( "Test Docking Pane", m_TestControllerFactory.Create( new ControllerInitializationContext( env ) ).View, DockLocation.Fill );
				return CommandExecutionResult.StopExecutingCommand;
			}
			if ( command == s_showComponentEditorAsDocked )
			{
				IDialogService dialogService = env.EnsureGetService<IDialogService>( );
				dialogService.Show( DefaultDialogFrameType.Ok, "Component Editor", m_CompositeEditorControllerFactory.Create( new ControllerInitializationContext( env ) ).View );
				return CommandExecutionResult.StopExecutingCommand;
			}
			if ( command == s_ShowPropertyEditorAsDocked )
			{
				IDockingService dockService = env.EnsureGetService<IDockingService>( );
				dockService.Show( "Properties", m_PropertyEditorControllerFactory.Create( new ControllerInitializationContext( env ) ).View, DockLocation.Floating );
				return CommandExecutionResult.StopExecutingCommand;
			}
			return CommandExecutionResult.ContinueExecutingCommand;
		}

		#endregion
	}
}
