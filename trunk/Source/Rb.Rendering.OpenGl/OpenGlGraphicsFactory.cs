using System;
using System.Reflection;
using System.Threading;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implementation of RenderFactory
	/// </summary>
	public class OpenGlGraphicsFactory : LibraryBuilder, IGraphicsFactory
	{
		public OpenGlGraphicsFactory( GraphicsInitialization init )
		{
			AutoAssemblyScan = true;

			//	TODO: AP: Remove effect and platform assembly hardcoding
			m_EffectsAssembly = init.EffectsAssembly;
			m_PlatformAssembly = init.PlatformAssembly;

		//	Assembly.Load( init.EffectsAssembly );
			Assembly.Load( init.PlatformAssembly );
		}

		/// <summary>
		/// Gets the builder object for custom types
		/// </summary>
		public LibraryBuilder CustomTypes
		{
			get { return this; }
		}

		/// <summary>
		/// Gets the name of the API supported by this render factory
		/// </summary>
		public string ApiName
		{
			get { return "OpenGl"; }
		}

		/// <summary>
		/// Creates a new IDisplaySetup object
		/// </summary>
		public IDisplaySetup CreateDisplaySetup( )
		{
			return ( IDisplaySetup )Activator.CreateInstance( DisplaySetupType );
		}

	    /// <summary>
		/// Creates a new IRenderState object
		/// </summary>
		public IRenderState CreateRenderState( )
		{
			return new OpenGlRenderState( );
		}

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public IMaterial CreateMaterial( )
		{
			return new OpenGlMaterial( );
		}

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		public ITexture2d CreateTexture2d( )
		{
			return new OpenGlTexture2d( );
		}

		/// <summary>
		/// Creates a 3d texture
		/// </summary>
		public ITexture3d CreateTexture3d( )
		{
			return new OpenGlTexture3d( );
		}

		/// <summary>
		/// Creates a cube map texture
		/// </summary>
		public ICubeMapTexture CreateCubeMapTexture( )
		{
			return new OpenGlCubeMapTexture( );
		}


		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public ITexture2dSampler CreateTexture2dSampler( )
		{
			return new OpenGlTexture2dSampler( );
		}

		/// <summary>
		/// Creates a cube map sampler
		/// </summary>
		public ICubeMapTextureSampler CreateCubeMapTextureSampler( )
		{
			return new OpenGlCubeMapTextureSampler( );
		}

		/// <summary>
		/// Creates a new RenderFont object
		/// </summary>
		public IFont CreateFont( FontData fontData )
		{
			return new OpenGlFont( fontData );

		}

		/// <summary>
		/// Creates a new RenderTarget object
		/// </summary>
		public IRenderTarget CreateRenderTarget( )
		{
			return new OpenGlRenderTarget( );
		}

		/// <summary>
		/// Creates a new IVertexBuffer object
		/// </summary>
		public IVertexBuffer CreateVertexBuffer( )
		{
			return new OpenGlVertexBuffer( );
		}

		/// <summary>
		/// Creates an index buffer
		/// </summary>
		public IIndexBuffer CreateIndexBuffer( )
		{
			return new OpenGlIndexBuffer( );
		}

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		public IRenderer CreateRenderer( )
		{
			OpenGlRenderer renderer = new OpenGlRenderer( Thread.CurrentThread.ManagedThreadId );
			renderer.Setup( );
			return renderer;
		}
		
		/// <summary>
		/// Creates a new ShaderParameterBindings object
		/// </summary>
		public IEffectDataSources CreateEffectDataSources( )
		{
			return ( IEffectDataSources )Activator.CreateInstance( EffectDataSourcesType );
		}

		/// <summary>
		/// Creates a new Draw object
		/// </summary>
		public IDraw CreateDraw( )
		{
			return new OpenGlDraw( );
		}

		#region Private Members

		private readonly string m_EffectsAssembly;
		private readonly string m_PlatformAssembly;

		private Type m_EffectDataSourcesType;
		private Type m_DisplaySetupType;

		private Type EffectDataSourcesType
		{
			get
			{
				if ( m_EffectDataSourcesType == null )
				{
					m_EffectDataSourcesType = GetAssemblyType<IEffectDataSources>( m_EffectsAssembly );
				}
				return m_EffectDataSourcesType;
			}
		}

		private Type DisplaySetupType
		{
			get
			{
				if ( m_DisplaySetupType == null )
				{
					m_DisplaySetupType = GetAssemblyType<IDisplaySetup>( m_PlatformAssembly );
				}
				return m_DisplaySetupType;
			}
		}

		private static Type GetAssemblyType<T>( string assemblyName )
		{
			Assembly asm = AppDomain.CurrentDomain.Load( assemblyName );
			Type result = AppDomainUtils.FindTypeImplementingInterface( asm.GetTypes( ), typeof( T ) );
			if ( result == null )
			{
				throw new ArgumentException( string.Format( "There was no type in assembly \"{0}\" that implemented IEffectDataSources", asm ) );
			}
			return result;
		}

		#endregion

		#region LibraryBuilder Members

		/// <summary>
		/// Returns true if the specified type has the RenderingLibraryTypeAttribute
		/// </summary>
		protected override bool IsLibraryType( Type type )
		{
			return type.GetCustomAttributes( typeof( RenderingLibraryTypeAttribute ), true ).Length == 1;
		}

		#endregion
	}
}