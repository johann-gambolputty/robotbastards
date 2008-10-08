using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{

	/// <summary>
	/// A static class containing implementations of the key rendering interfaces
	/// </summary>
	public static class Graphics
	{
		#region Default Pens
		
		/// <summary>
		/// Pre-defined pens
		/// </summary>
		public static class Pens
		{
			public static readonly Draw.IPen White	= Draw.NewPen( Color.White );
			public static readonly Draw.IPen Red	= Draw.NewPen( Color.Red );
			public static readonly Draw.IPen Green	= Draw.NewPen( Color.Green );
			public static readonly Draw.IPen Blue	= Draw.NewPen( Color.Blue );
			public static readonly Draw.IPen Black	= Draw.NewPen( Color.Black );
		}
		
		#endregion

		#region Default Brushes
		
		/// <summary>
		/// Pre-defined brushes
		/// </summary>
		public static class Brushes
		{
			public static readonly Draw.IBrush White	= Draw.NewBrush( Color.White );
			public static readonly Draw.IBrush Red		= Draw.NewBrush( Color.Red );
			public static readonly Draw.IBrush Green	= Draw.NewBrush( Color.Green );
			public static readonly Draw.IBrush Blue		= Draw.NewBrush( Color.Blue );
			public static readonly Draw.IBrush Black	= Draw.NewBrush( Color.Black );
		}
		#endregion

		#region Default Surfaces

		/// <summary>
		/// Pre-defined surfaces
		/// </summary>
		public static class Surfaces
		{
			public static readonly Draw.ISurface White	= Draw.NewSurface( Color.White );
			public static readonly Draw.ISurface Red	= Draw.NewSurface( Color.Red );
			public static readonly Draw.ISurface Green	= Draw.NewSurface( Color.Green );
			public static readonly Draw.ISurface Blue	= Draw.NewSurface( Color.Blue );
			public static readonly Draw.ISurface Black	= Draw.NewSurface( Color.Black );


			public static readonly Draw.ISurface TransparentRed = TransparentSurface( Color.Red );
			public static readonly Draw.ISurface TransparentBlue = TransparentSurface( Color.Blue );
			public static readonly Draw.ISurface TransparentGreen = TransparentSurface( Color.Green );

			#region Helpers

			private static Draw.ISurface TransparentSurface( Color colour )
			{
				Draw.ISurface surface = Draw.NewSurface( colour );
				surface.FaceBrush.State.Blend = true;
				surface.FaceBrush.State.SourceBlend = BlendFactor.One;
				surface.FaceBrush.State.DestinationBlend = BlendFactor.One;

				return surface;
			}
		
			#endregion
		}

		#endregion

		#region Default Fonts

		/// <summary>
		/// Default fonts
		/// </summary>
		public static class Fonts
		{
			/// <summary>
			/// Gets a simple font used for displaying debug information
			/// </summary>
			public static IFont DebugFont
			{
				get
				{
					if ( s_DebugFont == null )
					{
						s_DebugFont = Factory.CreateFont( new FontData( "Courier New", 8 ) );
					}
					return s_DebugFont;
				}
			}
			
			/// <summary>
			/// Gets a simple font used for displaying debug information
			/// </summary>
			public static IFont SmallDebugFont
			{
				get
				{
					if ( s_SmallDebugFont == null )
					{
						s_SmallDebugFont = Factory.CreateFont( new FontData( "Courier New", 8 ) );
					}
					return s_SmallDebugFont;
				}
			}

			#region Private Members

			private static IFont s_DebugFont;
			private static IFont s_SmallDebugFont;

			#endregion
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes graphics objects by loading a specified assembly, instancing the first type that implements
		/// <see cref="IGraphicsFactory"/>, and using the result to initialize the other graphics objects
		/// </summary>
		public static void Initialize( GraphicsInitialization init )
		{
			Assembly assembly;
			if ( File.Exists( init.RenderingAssembly ) )
			{
#pragma warning disable 0618
				//	TODO: AP: Find alternative...
				AppDomain.CurrentDomain.AppendPrivatePath( Path.GetDirectoryName( init.RenderingAssembly ) );
#pragma warning restore 0618
				assembly = Assembly.LoadFrom( init.RenderingAssembly );
			}
			else
			{
				assembly = AppDomain.CurrentDomain.Load( init.RenderingAssembly );
			}
			IGraphicsFactory factory = GetGraphicsFactoryFromAssembly( assembly, init );
			factory.CustomTypes.ScanAssembly( assembly );
			Initialize( factory );	
		}

		/// <summary>
		/// Creates graphics objects from the specified factory
		/// </summary>
		public static void Initialize( IGraphicsFactory factory )
		{
			s_Factory = factory;
			s_Renderer = factory.CreateRenderer( );
			s_Draw = factory.CreateDraw( );

			//	Effect data sources are created lazily, to remove the dependency
			//	on the effect assembly for applications that don't need it
		}

		/// <summary>
		/// Scans all assemblies in a given directory. If any contain the assembly identifier "GraphicsApi=X", where
		/// X is the name of the current graphics API (<see cref="IGraphicsFactory.ApiName"/>), it's loaded
		/// </summary>
		public static void LoadCustomTypeAssemblies( string directory, bool recursive )
		{
			Rb.AssemblySelector.IdentifierMap.Instance.AddAssemblyIdentifiers( directory, recursive ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories );
			Rb.AssemblySelector.IdentifierMap.Instance.LoadAll( "GraphicsApi=" + Factory.ApiName );	
		}

		#endregion

		/// <summary>
		/// Gets the current draw object
		/// </summary>
		public static IDraw Draw
		{
			get { return s_Draw; }
		}

		/// <summary>
		/// Gets the current renderer
		/// </summary>
		public static IRenderer Renderer
		{
			get { return s_Renderer; }
			set { s_Renderer = value; }
		}

		/// <summary>
		/// Gets the current effect data source set
		/// </summary>
		/// <remarks>
		/// Lazy loaded
		/// </remarks>
		public static IEffectDataSources EffectDataSources
		{
			get
			{
				if ( s_EffectDataSources == null )
				{
					s_EffectDataSources = Factory.CreateEffectDataSources( );
				}
				return s_EffectDataSources;
			}
			set { s_EffectDataSources = value; }
		}

		/// <summary>
		/// Gets the graphics factory
		/// </summary>
		public static IGraphicsFactory Factory
		{
			get { return s_Factory; }
			set { s_Factory = value; }
		}

		#region Private members

		private static IDraw s_Draw;
		private static IRenderer s_Renderer;
		private static IGraphicsFactory s_Factory;
		private static IEffectDataSources s_EffectDataSources;

		private static IGraphicsFactory GetGraphicsFactoryFromAssembly( Assembly assembly, GraphicsInitialization init )
		{
			foreach ( Type type in assembly.GetTypes( ) )
			{
				if ( type.GetInterface( typeof( IGraphicsFactory ).Name ) != null )
				{
					IGraphicsFactory factory = ( IGraphicsFactory )Activator.CreateInstance( type, init );
					return factory;
				}
			}
			
			throw new ArgumentException( string.Format( "No type in assembly \"{0}\" implemented IGraphicsFactory", assembly.FullName ) );
		}

		#endregion

	}
}
