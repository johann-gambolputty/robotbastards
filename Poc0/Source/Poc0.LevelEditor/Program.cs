using System;
using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Tools.LevelEditor.Core.Controls.Forms;

namespace Poc0.LevelEditor
{
	static class Program
	{
		interface TestInterface
		{
			void Method();

			bool Property
			{
				get; set;
			}

			event EventHandler Event;
		}

		class TestObject : TestInterface
		{

			#region TestInterface Members

			public void Method()
			{
				System.Diagnostics.Debug.WriteLine("Method");
			}

			public bool Property
			{
				get
				{
					System.Diagnostics.Debug.WriteLine("GetProperty");
					return false;
				}
				set
				{
					System.Diagnostics.Debug.WriteLine("SetProperty to " + value);
					if ( Event != null)
					{
						Event(null, null);
					}
				}
			}

			public event EventHandler Event;

			#endregion
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			AppDomain.CurrentDomain.Load( "MagicLibrary" );

			EditorApp.InitializeAll( );

			TestInterface testHandle = AssetHandleProxy.Create<TestInterface>();

			TestObject test = new TestObject( );
			((AssetHandle)testHandle).Asset = test;

			testHandle.Method();
			bool b = testHandle.Property;
			testHandle.Property = b;

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm( ) );
		}
	}
}