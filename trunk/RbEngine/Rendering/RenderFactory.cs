using System;

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
		/// </remarks>
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
		/// Loads an assembly containing composite objects
		/// </summary>
		/// <param name="assemblyName"> Name of the assembly </param>
		public static void LoadCompositeAssembly( string assemblyName )
		{

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

		/// <summary>
		/// Returns the Type object representing a composite with the specified name
		/// </summary>
		/// <param name="typeName">Type name</param>
		/// <returns>Returns the named composite Type</returns>
		public abstract Type				GetCompositeType( string typeName );

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
		/// <seealso cref="GetCompositeType">GetCompositeType</seealso>
		public Object						NewComposite( string typeName )
		{
			return NewComposite( GetCompositeType( typeName ) );
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
		public abstract Object				NewComposite( Type baseType );

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
			NewRenderer( );			//	Renderer constructor sets the Renderer singleton
			NewShapeRenderer( );	//	ShapeRenderer constructor sets the ShapeRenderer singleton
		}

		private static RenderFactory		ms_Singleton;
	}
}
