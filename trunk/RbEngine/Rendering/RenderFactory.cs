using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Singleton factory for rendering components
	/// </summary>
	/// <remarks>
	/// On construction, the <c>RenderFactory</c> creates the singletons for <c>Renderer</c> and <c>ShapeRenderer</c> (using
	/// <c>NewRenderer()</c> and <c>NewShapeRenderer()</c> respectively)
	/// </remarks>
	public abstract class RenderFactory
	{

		#region	Assembly loaders

		/// <summary>
		/// Loads an assembly that implements the <c>RbEngine.Rendering</c> namespace
		/// </summary>
		/// <param name="renderAssemblyName">Assembly name</param>
		/// <exception cref="ApplicationException">Thrown if the assembly does not contain a class that implements RenderFactory</exception>
		/// <remarks>
		/// Loads the specified assembly, and searches it for a class that implements RenderFactory. If one is found, an instance is
		/// created of that class. This sets the singletons RenderFactory.Inst, Renderer.Inst and ShapeRenderer.Inst. If no class
		/// is found that implements RenderFactory, an ApplicationException is thrown.
		/// <note>
		/// This call also automatically finds any Rendering.Composites.Composite derived classes in the loaded assembly, and adds them
		/// the render factory, so that they can be created using the <see cref="NewComposite"/> methods. It also tracks any subsequent
		/// assembly loads, and looks for composites in those also. Therefore, simply calling AppDomain.Load() for an assembly that contains
		/// composites, will update the RenderFactory singleton.
		/// </note>
		/// </remarks>
		public static void Load( string renderAssemblyName )
		{
			System.Reflection.Assembly renderAssembly = AppDomain.CurrentDomain.Load( renderAssemblyName );

			foreach ( Type curType in renderAssembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( RenderFactory ) ) )
				{
					RenderFactory factory = ( RenderFactory )System.Activator.CreateInstance( curType );
					factory.AddAssemblyComposites( renderAssembly );

					AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler( OnAssemblyLoad );

					return;
				}
			}

			throw new ApplicationException( String.Format( "No type in assembly \"{0}\" implemented RenderFactory" ) );
		}

		/// <summary>
		/// Adds the Composite-derived classes in the specified assembly
		/// </summary>
		/// <param name="compositeAssembly"> Assembly to check </param>
		private void AddAssemblyComposites( System.Reflection.Assembly compositeAssembly )
		{
			foreach ( Type curType in compositeAssembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( Composites.Composite ) ) )
				{
					if ( m_CompositeNameMap.ContainsKey( curType.Name ) )
					{
						Output.WriteLineCall( Output.RenderingError, "Composite type \"{0}\" already exists in the rendering factory composite map", curType.Name );
					}
					else
					{
						Output.WriteLineCall( Output.RenderingInfo, "Adding composite type \"{0}\" to render factory", curType.Name );
						m_CompositeNameMap.Add( curType.Name, curType );

						for ( Type baseType = curType; baseType != typeof( Composites.Composite ); baseType = baseType.BaseType )
						{
							m_CompositeBaseTypeMap.Add( baseType, curType );
						}
					}
				}
			}
		}

		private static void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			RenderFactory.Inst.AddAssemblyComposites( args.LoadedAssembly );
		}

		#endregion

		#region	Singleton

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

		#endregion

		#region	Composites

		/// <summary>
		/// Returns the Type object representing a composite with the specified name
		/// </summary>
		/// <param name="typeName">Type name</param>
		/// <returns>Returns the named composite Type</returns>
		public Type						GetCompositeTypeFromName( string typeName )
		{
			return ( Type )m_CompositeNameMap[ typeName ];
		}

		/// <summary>
		/// Returns a Composite-derived class's Type object that implements a specified Composite base type
		/// </summary>
		public Type						GetCompositeTypeFromBaseType( Type baseType )
		{
			return ( Type )m_CompositeBaseTypeMap[ baseType ];
		}

		/// <summary>
		/// Creates a new composite object, from the name of a type
		/// </summary>
		/// <param name="typeName">Type name of the composite</param>
		/// <returns>Returns the new composite object. Returns null if the specified composite type is not supported</returns>
		/// <remarks>
		/// Composites are objects that combine render states, vertex buffers, textures and other rendering objects. Examples
		/// are meshes, particle systems, heightfields, and so on.
		/// </remarks>
		/// <example>
		/// <code lang="C#">
		/// Mesh newMesh = ( Mesh )RenderFactory.Inst.NewComposite( "Mesh" );
		/// </code>
		/// </example>
		/// <seealso cref="GetCompositeTypeFromName">GetCompositeTypeFromName</seealso>
		public Composites.Composite			NewComposite( string typeName )
		{
			return ( Composites.Composite )System.Activator.CreateInstance( GetCompositeTypeFromName( typeName ) );
		}


		/// <summary>
		/// Creates a new composite object, from a given base type
		/// </summary>
		/// <param name="baseType">Base type of the composite</param>
		/// <returns>Returns the new composite object. Returns null if the specified composite type is not supported</returns>
		/// <remarks>
		/// Composites are objects that combine render states, vertex buffers, textures and other rendering objects. Examples
		/// are meshes, particle systems, heightfields, and so on.
		/// </remarks>
		/// <example>
		/// <code lang="C#">
		/// Mesh newMesh = ( Mesh )RenderFactory.Inst.NewComposite( typeof( Mesh ) );
		/// </code>
		/// </example>
		/// <seealso cref="GetCompositeTypeFromBaseType">GetCompositeTypeFromBaseType</seealso>
		public Composites.Composite			NewComposite( Type baseType )
		{
			return ( Composites.Composite )System.Activator.CreateInstance( GetCompositeTypeFromBaseType( baseType ) );
		}

		#endregion

		/// <summary>
		/// Creates a new RenderState object
		/// </summary>
		[ Components.BuilderMethod ]
		public abstract RenderState					NewRenderState( );

		/// <summary>
		/// Creates a new Material object
		/// </summary>
		public abstract Material					NewMaterial( );

		/// <summary>
		/// Creates a new Texture2d object
		/// </summary>
		/// <returns></returns>
		public abstract Texture2d					NewTexture2d( );

		/// <summary>
		/// Creates a new TextureSampler2d object
		/// </summary>
		public abstract TextureSampler2d			NewTextureSampler2d( );

		/// <summary>
		/// Creates a new RenderFont object
		/// </summary>
		public abstract RenderFont					NewFont( );

		/// <summary>
		/// Creates a new RenderTarget object
		/// </summary>
		public abstract RenderTarget				NewRenderTarget( );

		/// <summary>
		/// Creates a new Renderer object
		/// </summary>
		protected abstract Renderer					NewRenderer( );

		/// <summary>
		/// Creates a new ShapeRenderer object
		/// </summary>
		protected abstract ShapeRenderer			NewShapeRenderer( );

		/// <summary>
		/// Creates a new ShaderParamterBindings object
		/// </summary>
		protected abstract ShaderParameterBindings	NewShaderParameterBindings( );

		/// <summary>
		/// Protected constructor. Sets up the Renderer and ShapeRenderer 
		/// </summary>
		protected RenderFactory( )
		{
			ms_Singleton = this;
			NewRenderer( );					//	Renderer constructor sets the Renderer singleton
			NewShapeRenderer( );			//	ShapeRenderer constructor sets the ShapeRenderer singleton
			NewShaderParameterBindings( );	//	ShaderParameterBindings constructor sets the ShaderParameterBindings singleton
		}

		private System.Collections.Hashtable	m_CompositeNameMap		= new System.Collections.Hashtable( );
		private System.Collections.Hashtable	m_CompositeBaseTypeMap	= new System.Collections.Hashtable( );
		private static RenderFactory			ms_Singleton;

	}
}
