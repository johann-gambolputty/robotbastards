using System;
using System.Collections.Generic;
using Rb.Core.Assets;
using Rb.World;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Holds a list of the materials that are available 
	/// </summary>
	/// <remarks>
	/// Available as a scene service in the editor scene
	/// </remarks>
	[Serializable]
	public class MaterialSet
	{
		//	TODO: AP: MaterialSet should be a scene service, not a singleton
		//	TODO: AP: MaterialSet can be imported, editing and exported

		/// <summary>
		/// Gets the MaterialSet in a given scene
		/// </summary>
		/// <param name="scene">Scene containing MaterialSet service</param>
		/// <returns>Returns the scene's MaterialSet service</returns>
		/// <exception cref="System.ArgumentException">Thrown if scene does not contain a material set</exception>
		public static MaterialSet FromScene( Scene scene )
		{
			MaterialSet result = scene.GetService< MaterialSet >( );
			if ( result == null )
			{
				throw new ArgumentException( "No material set available in scene", "scene" );
			}
			return result;
		}

		/// <summary>
		/// Loads materials from an asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="createFallbackOnError">Generates a fallback default material set if an error occurs</param>
		public static MaterialSet Load( ISource source, bool createFallbackOnError )
		{
			LoadParameters parameters = new LoadParameters( );
			return ( MaterialSet )AssetManager.Instance.Load( source, parameters );
		}

		/// <summary>
		/// Sets the name of this material set
		/// </summary>
		public MaterialSet( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// Gets the name of this material set
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Clears the list of materials
		/// </summary>
		public void Clear( )
		{
			m_Materials.Clear( );
		}

		/// <summary>
		/// Gets/sets the default wall material
		/// </summary>
		public Material DefaultWallMaterial
		{
			get { return m_DefaultWallMaterial; }
			set { m_DefaultWallMaterial = value; }
		}

		/// <summary>
		/// Gets/sets the default floor material
		/// </summary>
		public Material DefaultFloorMaterial
		{
			get { return m_DefaultFloorMaterial; }
			set { m_DefaultFloorMaterial = value; }
		}

		/// <summary>
		/// Gets/sets the default obstacle material
		/// </summary>
		public Material DefaultObstacleMaterial
		{
			get { return m_DefaultObstacleMaterial; }
			set { m_DefaultObstacleMaterial = value; }
		}

		/// <summary>
		/// Gets/sets the set of stored materials
		/// </summary>
		public List< Material > Materials
		{
			get { return m_Materials; }
		}

		#region Private members

		private readonly string m_Name;
		private readonly List<Material> m_Materials = new List<Material>( );
		private Material m_DefaultWallMaterial;
		private Material m_DefaultFloorMaterial;
		private Material m_DefaultObstacleMaterial;

		#endregion
	}
}
