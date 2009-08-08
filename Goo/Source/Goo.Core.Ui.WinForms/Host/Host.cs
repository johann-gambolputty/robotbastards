using System.Windows.Forms;
using Goo.Core.Host;
using Goo.Core.Services.Events;
using Goo.Core.Services.Workspaces;
using Goo.Core.Ui.Layouts;
using Goo.Core.Units;
using Rb.Core.WinForms.Application;

namespace Goo.Common.WinForms.Host
{
	/// <summary>
	/// Winforms host
	/// </summary>
	public class Host : AbstractHost
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Host name (title bar text)</param>
		public Host( string name )
		{
			m_Name = name;
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
		}

		/// <summary>
		/// Gets the form used by this host
		/// </summary>
		public Form Form
		{
			get { return m_Form; }
		}

		/// <summary>
		/// Runs the host
		/// </summary>
		public override void Run( params IUnit[] startupUnits )
		{
			m_Form = CreateMainForm( );

			AddStartupServices( );
			
			//	TODO: REMOVE: Testing only
			if ( System.IO.File.Exists( "c:\\temp\\layouts.bin" ) )
			{
				using ( System.IO.FileStream fs = System.IO.File.OpenRead( "c:\\temp\\layouts.bin" ) )
				{
					Environment.GetService<ILayoutManagerService>( ).LoadLayouts( fs );
				}
			}

			//	Load all startup units
			foreach ( IUnit unit in startupUnits )
			{
				LoadUnit( unit );
			}

		//	Application.Run( Form );
			RealTimeApplication.Run( Form );
		}

		#region Protected Members

		/// <summary>
		/// Creates the main application form
		/// </summary>
		protected virtual Form CreateMainForm( )
		{
			DefaultMainForm form = new DefaultMainForm( );
			form.Text = m_Name;
			return form;
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Adds any services required at startup. Called after main form creation, but before unit loading
		/// </summary>
		protected virtual void AddStartupServices( )
		{
			//	Some default services - move to DI
			IEventService eventService = Environment.AddService( new EventService( ) );
			Environment.AddService( new ActiveWorkspaceService( eventService ) );
			Environment.AddService( new LayoutManagerService( Environment ) );
		}

		/// <summary>
		/// Specific handling for closing the host
		/// </summary>
		protected override void CloseHost( )
		{
			//	TODO: REMOVE: Testing only
			using ( System.IO.FileStream fs = new System.IO.FileStream( "c:\\temp\\layouts.bin", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write ) )
			{
				Environment.EnsureGetService<LayoutManagerService>( ).SaveLayouts( fs );
			}
			if ( Form != null )
			{
				Form.Close( );
			}
		}

		#endregion

		#region Private Members

		private Form m_Form;
		private readonly string m_Name;

		#endregion
	}
}
