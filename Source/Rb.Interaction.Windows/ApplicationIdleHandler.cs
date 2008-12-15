using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
	//	TODO: AP: Class would be useful for rendering also

	/// <summary>
	/// Windows-only application idle handler
	/// </summary>
	public class ApplicationIdleHandler : InteractionUpdateTimer
	{
		/// <summary>
		/// Default constructor. Starts listening to the application idle event
		/// </summary>
		public ApplicationIdleHandler( )
		{
			Application.Idle += OnInteractionUpdate;
		}

		/// <summary>
		/// Returns true if the application is idle (no messages in the message queue)
		/// </summary>
		public static bool IsAppStillIdle
		{
			get
			{
				Message msg;
				return !PeekMessage( out msg, IntPtr.Zero, 0, 0, 0 );
			}
		}


		#region Private Members

		[StructLayout( LayoutKind.Sequential )]
		private struct Message
		{
			public IntPtr hWnd;
			public uint msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public int x;
			public int y;
		}

		[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
		[DllImport( "User32.dll", CharSet = CharSet.Auto )]
		private static extern bool PeekMessage( out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags );

		/// <summary>
		/// Handles the application idle event. Updates all input binding monitors
		/// </summary>
		private void OnInteractionUpdate( object sender, EventArgs args )
		{
			while ( IsAppStillIdle )
			{
				OnUpdate( );
			}
		}

		#endregion
	}

}
