
using System.Windows.Forms;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc1.Bob.Controls
{
	public class WorkspaceWindowInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="group">Window group</param>
		/// <param name="menuName">Window name, decorated with an ampersand to designate the menu shortcut key </param>
		/// <param name="create">Window creation function</param>
		public WorkspaceWindowInfo( string group, string menuName, FunctionDelegates.Function<Control> create ) :
			this( group, menuName, create, DockState.Float )
		{
		}

		/// <summary>
		/// Setup constructor with non-default default dock state (if that makes sense...)
		/// </summary>
		/// <param name="group">Window group</param>
		/// <param name="menuName">Window name, decorated with an ampersand to designate the menu shortcut key </param>
		/// <param name="create">Window creation function</param>
		/// <param name="defaultDockState">Default dock state</param>
		public WorkspaceWindowInfo( string group, string menuName, FunctionDelegates.Function<Control> create, DockState defaultDockState )
		{
			m_Group = group;
			m_Name = menuName;
			m_Create = create;
			m_DefaultDockState = defaultDockState;
		}

		/// <summary>
		/// Gets the name of the window's group
		/// </summary>
		public string Group
		{
			get { return m_Group; }
		}

		/// <summary>
		/// Gets the name of the window, decorated with an ampersand to designate the menu shortcut key 
		/// </summary>
		public string MenuName
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the name of the window
		/// </summary>
		public string Name
		{
			get { return m_Name.Replace( "&", "" ); }
		}

		/// <summary>
		/// Gets the window dock content
		/// </summary>
		public DockContent Content
		{
			get
			{
				if ( m_Content == null )
				{
					m_Content = CreateDockContent( Name, Control );
				}
				return m_Content;
			}
		}

		/// <summary>
		/// Gets the window control
		/// </summary>
		public Control Control
		{
			get
			{
				if ( m_Control == null )
				{
					m_Control = m_Create( );
				}
				return m_Control;
			}
		}

		/// <summary>
		/// Returns the default dock state of the control
		/// </summary>
		public DockState DefaultDockState
		{
			get { return m_DefaultDockState; }
		}

		#region Private Members

		private readonly string m_Group;
		private readonly string m_Name;
		private readonly FunctionDelegates.Function<Control> m_Create;
		private readonly DockState m_DefaultDockState;

		private DockContent m_Content;
		private Control m_Control;

		/// <summary>
		/// Provides custom persistence strings to DockContent
		/// </summary>
		private class WindowDockContent : DockContent
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="persistString">String to write when content is persists</param>
			public WindowDockContent( string persistString )
			{
				m_PersistString = persistString;
			}

			/// <summary>
			/// Returns the persistence string passed to the constructor
			/// </summary>
			protected override string GetPersistString( )
			{
				return m_PersistString;
			}

			#region Private Members

			private readonly string m_PersistString;

			#endregion
		}

		/// <summary>
		/// Creates dock content for a control
		/// </summary>
		private static DockContent CreateDockContent( string title, Control control )
		{
			DockContent content = new WindowDockContent( control.GetType( ).Name );

			content.Text = title;
			content.Controls.Add( control );
			content.AutoScroll = true;

			control.Dock = DockStyle.Fill;

			return content;
		}

		#endregion
	}

}
