using System;
using System.Collections.Generic;

namespace Rb.Rendering
{
    //  TODO: AP: Removed IChild interface, so light group no longer adds itself to parent ISceneRenderable pre-render list. Need to
    //  determine alternative mechanism for applying light group pass to renderables.

	/// <summary>
	/// A bunch of lights
	/// </summary>
    public class LightGroup : IPass
	{
		#region	Light list management

		/// <summary>
		/// Gets the list of lights making up this group
		/// </summary>
		public ICollection< Light > Lights
		{
			get { return m_Lights; }
		}

		/// <summary>
		/// Adds a light to this group
		/// </summary>
		/// <param name="light">Light to add</param>
		public void Add( Light light )
		{
			m_Lights.Add( light );
		}

		/// <summary>
		/// Removes a light from this group
		/// </summary>
		/// <param name="light">Light to remove</param>
		public void Remove( Light light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Removes all lights in the group
		/// </summary>
		public void Clear( )
		{
			m_Lights.Clear( );
		}

		/// <summary>
		/// Returns the number of lights stored in the group
		/// </summary>
		public int NumLights
		{
			get { return m_Lights.Count; }
		}

		/// <summary>
		/// Indexer. Returns the light at the specified index
		/// </summary>
		public Light this[ int index ]
		{
			get
			{
				return m_Lights[ index ];
			}
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Adds all the lights in this group to the renderer
		/// </summary>
		public void Begin( )
		{
			Renderer renderer = Renderer.Inst;

			for ( int lightIndex = 0; lightIndex < NumLights; ++lightIndex )
			{
				renderer.AddLight( m_Lights[ lightIndex ] );
			}
		}

		/// <summary>
		/// Clears all the lights in this group from the renderer
		/// </summary>
		public void End( )
		{
			Renderer.Inst.ClearLights( );
		}

		#endregion

		private List< Light > m_Lights = new List< Light >( );
	}
}
