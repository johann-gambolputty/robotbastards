using System.Collections.Generic;
using Rb.Core.Assets;
using Rb.Log;
using Rb.World;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Holds a list of the materials that are available 
	/// </summary>
	/// <remarks>
	/// Available as a scene service in the editor scene
	/// </remarks>
	public class MaterialSet
	{
		//	TODO: AP: MaterialSet should be a scene service, not a singleton
		//	TODO: AP: MaterialSet can be imported, editing and exported

		/// <summary>
		/// Gets the MaterialSet in a given scene
		/// </summary>
		/// <param name="scene">Scene containing MaterialSet service</param>
		/// <param name="createIfNonExistant">If true, a MaterialSet is created and added to the scene, if there isn't one present</param>
		/// <returns>Returns the scene's MaterialSet service. Returns null if the MaterialSet does not exist, and createIfNonExistant is false</returns>
		public static MaterialSet FromScene( Scene scene, bool createIfNonExistant )
		{
			MaterialSet result = scene.GetService< MaterialSet >( );
			if ( result == null )
			{
				if ( !createIfNonExistant )
				{
					return null;
				}
				AppLog.Warning( "Creating MaterialSet for scene..." );
				result = new MaterialSet( );
				scene.AddService( result );
			}
			return result;
		}

		/// <summary>
		/// Loads materials from an asset
		/// </summary>
		/// <param name="source">Asset source</param>
		public void Load( ISource source )
		{
			LoadParameters parameters = new LoadParameters( m_Materials );
			AssetManager.Instance.Load( source, parameters );
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
		/// Gets the set of stored materials
		/// </summary>
		public IEnumerable< Material > Materials
		{
			get { return m_Materials; }
		}

		#region Private members

		private Material m_DefaultWallMaterial;
		private Material m_DefaultFloorMaterial;
		private Material m_DefaultObstacleMaterial;
		private readonly List< Material > m_Materials = new List< Material >( );

		#endregion
	}
}
