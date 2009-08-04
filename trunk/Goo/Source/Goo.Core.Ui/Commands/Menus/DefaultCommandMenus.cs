
namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu items and groups representing the commands in <see cref="DefaultCommands"/>
	/// </summary>
	public static class DefaultCommandMenus
	{
		/// <summary>
		/// Menu descriptors for the File commands (<see cref="DefaultCommands.File"/>)
		/// </summary>
		public static class File
		{
			/// <summary>
			/// Gets the File group menu item descriptor
			/// </summary>
			public static MenuGroupInfo Group
			{
				get { return s_Group; }
			}

			/// <summary>
			/// Gets the File Open command menu item descriptor
			/// </summary>
			public static MenuItemInfo Open
			{
				get { return s_Open; }
			}

			/// <summary>
			/// Gets the File Close command menu item descriptor
			/// </summary>
			public static MenuItemInfo Close
			{
				get { return s_Close; }
			}

			/// <summary>
			/// Gets the File Exit command menu item descriptor
			/// </summary>
			public static MenuItemInfo Exit
			{
				get { return s_Exit; }
			}


			#region Private Members

			private static readonly MenuGroupInfo s_Group = new MenuGroupInfo( "&File", 0 );
			private static readonly MenuItemInfo s_Open = new MenuItemInfo( DefaultCommands.File.Open, 10 );
			private static readonly MenuItemInfo s_Close = new MenuItemInfo( DefaultCommands.File.Close, 20 );
			private static readonly MenuItemInfo s_Exit = new MenuItemInfo( DefaultCommands.File.Exit, MenuNodeInfo.OrderLast );

			#endregion
		}
	}
}
