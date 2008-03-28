using System;
using System.Reflection;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implementation of RenderFactory
	/// </summary>
	public abstract class OpenGlGraphicsFactory : IGraphicsFactory
	{
		public OpenGlGraphicsFactory( )
		{
			string effectAssemblyName = "Rb.Rendering.OpenGl.Cg";
			string platformAssemblyName = "Rb.Rendering.OpenGl.Windows";

			GetFactoryTypes( effectAssemblyName, platformAssemblyName );
		}

		/// <summary>
		/// Gets the builder object for custom types
		/// </summary>
		public LibraryBuilder CustomTypes
		{
			get { return m_CustomTypes; }
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
			return ( IDisplaySetup )Activator.CreateInstance( m_DisplaySetupType );
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
		/// <returns></returns>
		public ITexture2d CreateTexture2d( )
		{
			return new OpenGlTexture2d( );
		}

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public ITexture2dSampler CreateTexture2dSampler( )
		{
			return new OpenGlTextureSampler2d( );
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
		public IVertexBuffer CreateVertexBuffer( VertexBufferData bufferData )
		{
			return new OpenGlVertexBuffer( bufferData );
		}

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		public IRenderer CreateRenderer( )
		{
			return new OpenGlRenderer( );
		}
		
		/// <summary>
		/// Creates a new ShaderParameterBindings object
		/// </summary>
		public IEffectDataSources CreateEffectDataSources( )
		{
			return ( IEffectDataSources )Activator.CreateInstance( m_EffectDataSourcesType );
		}

		/// <summary>
		/// Creates a new Draw object
		/// </summary>
		public IDraw CreateDraw( )
		{
			return new OpenGlDraw( );
		}

		#region Private Members

		private readonly LibraryBuilder m_CustomTypes = new LibraryBuilder( );
		private Type m_EffectDataSourcesType;
		private Type m_DisplaySetupType;

		private void GetFactoryTypes( string effectAssemblyName, string platformAssemblyName )
		{
			m_EffectDataSourcesType = GetAssemblyType<IEffectDataSources>( effectAssemblyName );
			m_DisplaySetupType = GetAssemblyType<IDisplaySetup>( platformAssemblyName );
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
	}
}