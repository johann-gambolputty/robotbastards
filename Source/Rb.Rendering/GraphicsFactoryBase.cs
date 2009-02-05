
using System;
using System.Reflection;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;

namespace Rb.Rendering
{
	/// <summary>
	/// Utility base class for implementations of <see cref="IGraphicsFactory"/>
	/// </summary>
	public abstract class GraphicsFactoryBase : LibraryBuilder
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="init">Graphics initialization</param>
		protected GraphicsFactoryBase( GraphicsInitialization init )
		{
			Arguments.CheckNotNull( init, "init" );

			GraphicsLog.Info( "Initializing graphics factory" );
			GraphicsLog.Info( "    Platform assembly: " + init.PlatformAssembly );
			GraphicsLog.Info( "    Effects assembly: " + init.EffectsAssembly );

			AutoAssemblyScan = true;

			//	TODO: AP: Remove effect and platform assembly hardcoding
			m_EffectsAssembly = init.EffectsAssembly;
			m_PlatformAssembly = init.PlatformAssembly;
		}

		/// <summary>
		/// Initializes this factory
		/// </summary>
		/// <remarks>
		/// This matches the <see cref="IGraphicsFactory.Initialize"/> signature, so it
		/// does not need to be implemented in any derived classes
		/// </remarks>
		public void Initialize( )
		{
			//	Assembly.Load( init.EffectsAssembly );
			Type initializerType = GetAssemblyType<IGraphicsPlatformInitializer>( m_PlatformAssembly );
			if ( initializerType != null )
			{
			    GraphicsLog.Info( "Found platform initializer type " + initializerType );
			    ( ( IGraphicsPlatformInitializer )Activator.CreateInstance( initializerType ) ).Init( );
			}	
		}

		#region LibraryBuilder Members

		/// <summary>
		/// Returns true if the specified type has the RenderingLibraryTypeAttribute
		/// </summary>
		protected override bool IsLibraryType( Type type )
		{
			return type.GetCustomAttributes( typeof( RenderingLibraryTypeAttribute ), true ).Length == 1;
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the effect data sources type from the effects assembly specified in the initialization
		/// parameters passed to the constructor
		/// </summary>
		protected Type EffectDataSourcesType
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

		/// <summary>
		/// Gets the display setup type from the platform assembly specified in the initialization
		/// parameters passed to the constructor
		/// </summary>
		protected Type DisplaySetupType
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


		#endregion

		#region Private Members

		private readonly string m_EffectsAssembly;
		private readonly string m_PlatformAssembly;

		private Type m_EffectDataSourcesType;
		private Type m_DisplaySetupType;

		/// <summary>
		/// Loads an assembly and retrieves a type
		/// </summary>
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
