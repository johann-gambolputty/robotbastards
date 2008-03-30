using System;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Lights;

namespace Rb.Rendering.Lights
{
    //  TODO: AP: Removed IChild interface, so light group no longer adds itself to parent ISceneRenderable pre-render list. Need to
    //  determine alternative mechanism for applying light group pass to renderables.

	/// <summary>
	/// A bunch of lights
	/// </summary>
	[Serializable]
    public class LightGroup : IPass
	{
		#region	Light list management

		/// <summary>
		/// The lights making up the group
		/// </summary>
		public ILight[] Lights
		{
			get { return m_Lights; }
			set { m_Lights = value;  }
		}

		/// <summary>
		/// Returns the number of lights stored in the group
		/// </summary>
		public int NumLights
		{
			get { return ( m_Lights == null ) ? 0 : m_Lights.Length; }
		}

		/// <summary>
		/// Indexer. Returns the light at the specified index
		/// </summary>
		public ILight this[ int index ]
		{
			get { return m_Lights[ index ]; }
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Adds all the lights in this group to the renderer
		/// </summary>
		public void Begin( )
		{
			if ( m_Lights == null )
			{
				return;
			}

			IRenderer renderer = Graphics.Renderer;

			for ( int lightIndex = 0; lightIndex < m_Lights.Length; ++lightIndex )
			{
				renderer.AddLight( m_Lights[ lightIndex ] );
			}
		}

		/// <summary>
		/// Clears all the lights in this group from the renderer
		/// </summary>
		public void End( )
		{
			Graphics.Renderer.ClearLights( );
		}

		#endregion

		#region Private members

		private ILight[] m_Lights;

		#endregion
	}
}
