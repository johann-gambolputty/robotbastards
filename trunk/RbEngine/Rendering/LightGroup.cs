using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// A bunch of lights
	/// </summary>
	public class LightGroup : IAppliance
	{
		#region	Light list management

		/// <summary>
		/// Gets the list of lights making up this group
		/// </summary>
		public ArrayList	Lights
		{
			get
			{
				return m_Lights;
			}
		}

		/// <summary>
		/// Adds a light to this group
		/// </summary>
		/// <param name="light">Light to add</param>
		public void			Add( Light light )
		{
			m_Lights.Add( light );
		}

		/// <summary>
		/// Removes a light from this group
		/// </summary>
		/// <param name="light">Light to remove</param>
		public void			Remove( Light light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Returns the number of lights stored in the group
		/// </summary>
		public int			NumLights
		{
			get
			{
				return m_Lights.Count;
			}
		}

		/// <summary>
		/// Indexer. Returns the light at the specified index
		/// </summary>
		public Light		this[ int index ]
		{
			get
			{
				return ( Light )m_Lights[ index ];
			}
		}

		#endregion

		#region IAppliance Members

		/// <summary>
		/// Adds all the lights in this group to the renderer
		/// </summary>
		public void Begin( )
		{
			Renderer renderer = Renderer.Inst;

			for ( int lightIndex = 0; lightIndex < NumLights; ++lightIndex )
			{
				renderer.AddLight( ( Light )m_Lights[ lightIndex ] );
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
		
		private ArrayList	m_Lights = new ArrayList( );

	}
}
