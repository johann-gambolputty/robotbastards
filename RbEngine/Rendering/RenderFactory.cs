using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Singleton factory for rendering components
	/// </summary>
	/// <remarks>
	/// On construction, the RenderFactory creates the singletons for Renderer and ShapeRenderer (using NewRenderer() and NewShapeRenderer() respectively)
	/// </remarks>
	public abstract class RenderFactory
	{
		public static void Load( string renderAssemblyName )
		{
			System.Reflection.Assembly renderAssembly = AppDomain.CurrentDomain.Load( renderAssemblyName );

			foreach ( Type curType in renderAssembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( RenderFactory ) ) )
				{
					System.Activator.CreateInstance( curType );
					return;
				}
			}

			throw new ApplicationException( String.Format( "No type in assembly \"{0}\" implemented RenderFactory" ) );
		}

		/// <summary>
		/// Render factory singleton
		/// </summary>
		public static RenderFactory			Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Creates a new RenderState object
		/// </summary>
		public abstract RenderState			NewRenderState( );

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public abstract Material			NewMaterial( );

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public abstract Texture2d			NewTexture2d( );

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public abstract TextureSampler2d	NewTextureSampler2d( );

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected abstract Renderer			NewRenderer( );

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected abstract ShapeRenderer	NewShapeRenderer( );

		/// <summary>
		/// Protected constructor. Sets up the Renderer and ShapeRenderer 
		/// </summary>
		protected RenderFactory( )
		{
			ms_Singleton = this;
			NewRenderer( );
			NewShapeRenderer( );
		}

		private static RenderFactory		ms_Singleton;
	}
}
