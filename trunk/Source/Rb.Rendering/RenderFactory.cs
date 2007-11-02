using System;
using System.Reflection;
using Rb.Core.Utils;
using Rb.Rendering.Textures;

namespace Rb.Rendering
{
	/// <summary>
	/// Singleton factory for rendering components
	/// </summary>
	/// <remarks>
	/// On construction, the <c>RenderFactory</c> creates the singletons for <c>Renderer</c> and <c>ShapeRenderer</c> (using
	/// <c>NewRenderer()</c> and <c>NewShapeRenderer()</c> respectively)
	/// </remarks>
	public abstract class RenderFactory : LibraryBuilder
	{
		#region	Assembly loaders

		/// <summary>
		/// Loads an assembly that implements the RenderFactory class
		/// </summary>
		/// <param name="renderAssemblyName">Assembly name</param>
		/// <exception cref="ArgumentException">Thrown if the assembly does not contain a class that implements RenderFactory</exception>
		/// <remarks>
		/// Loads the specified assembly, and searches it for a class that implements RenderFactory. If one is found, an instance is
		/// created of that class. This sets the singletons Graphics.Factory, Graphics.Renderer and ShapeGraphics.Renderer. If no class
		/// is found that implements RenderFactory, an ArgumentException is thrown.
		/// </remarks>
		public static Assembly Load( string renderAssemblyName )
		{
			Assembly renderAssembly = AppDomain.CurrentDomain.Load( renderAssemblyName );

			foreach ( Type curType in renderAssembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( RenderFactory ) ) )
				{
					RenderFactory factory = ( RenderFactory )Activator.CreateInstance( curType );

					//	Set up the graphics singleons from the factory
					Graphics.Initialise( factory );

					factory.ScanAssembly( renderAssembly );
					factory.AutoAssemblyScan = true;

					return renderAssembly;
				}
			}

			throw new ArgumentException( string.Format( "No type in assembly \"{0}\" implemented RenderFactory", renderAssemblyName ) );
		}

		/// <summary>
		/// Returns true if type has the RenderLibraryAttribute
		/// </summary>
		protected override bool IsLibraryType( Type type )
		{
			return type.GetCustomAttributes( typeof( RenderingLibraryTypeAttribute ), true ).Length == 1;
		}

		/// <summary>
		/// Gets the name of the API that this rendering factory supports (e.g. "OpenGl", "DirectX9", ...)
		/// </summary>
		/// <remarks>
		/// This must match the API names used by assembly identifier attributes
		/// </remarks>
		public abstract string ApiName
		{
			get;
		}

		#endregion

		#region Platform factory

		/// <summary>
		/// Display setup creator
		/// </summary>
		public abstract IDisplaySetup CreateDisplaySetup( );

		#endregion

		#region Factory

		/// <summary>
		/// Creates a new RenderState object
		/// </summary>
		public abstract RenderState NewRenderState( );

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public abstract Material NewMaterial( );

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public abstract ITexture2d NewTexture2d( );

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public abstract TextureSampler2d NewTextureSampler2d( );

		/// <summary>
		/// Creates a new RenderFont object
		/// </summary>
		public abstract RenderFont NewFont( );

		/// <summary>
		/// Creates a new RenderTarget object
		/// </summary>
		public abstract RenderTarget NewRenderTarget( );

		/// <summary>
		/// Creates a new IVertexBuffer object
		/// </summary>
		/// <param name="bufferData">Vertex buffer initialisation data</param>
		public abstract IVertexBuffer NewVertexBuffer( VertexBufferData bufferData );

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected internal abstract Renderer NewRenderer( );

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected internal abstract ShapeRenderer NewShapeRenderer( );

		/// <summary>
		/// Creates a new ShaderParamterBindings object
		/// </summary>
		protected internal abstract ShaderParameterBindings NewShaderParameterBindings( );

		/// <summary>
		/// Creates a new 
		/// </summary>
		/// <returns></returns>
		protected internal abstract Draw NewDraw( );

		#endregion
	}
}
