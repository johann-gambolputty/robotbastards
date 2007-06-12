using System;
using RbEngine.Rendering;

namespace RbEngine.Scene
{
	/// <summary>
	/// LightingData objects can be attached to ISceneRenderable objects, enabling those objects to be lit by the scene LightingManager
	/// </summary>
	public class LightingData : Rendering.IAppliance, Components.IChildObject
	{
		/// <summary>
		/// Clears all lights from the group
		/// </summary>
		public void			ClearLights( )
		{
			m_NumLights = 0;
		}

		/// <summary>
		/// Adds a light to the group
		/// </summary>
		public void			AddLight( Light light )
		{
			m_Lights[ m_NumLights++ ] = light;
		}

		/// <summary>
		/// Number of lights
		/// </summary>
		public int			NumLights
		{
			get
			{
				return m_NumLights;
			}
		}

		#region IAppliance Members

		/// <summary>
		/// Starts applying this lighting group
		/// </summary>
		public void Begin( )
		{
			Renderer renderer = Renderer.Inst;

			renderer.ClearLights( );
			for ( int lightIndex = 0; lightIndex < NumLights; ++lightIndex )
			{
				renderer.AddLight( m_Lights[ lightIndex ] );
			}
		}

		/// <summary>
		/// Stops applying this lighting group
		/// </summary>
		public void End( )
		{
		}

		#endregion

		/// <summary>
		/// Maximum number of lights in a group
		/// </summary>
		public const int	MaxLights	= 4;

		/// <summary>
		/// Stores lights that have been added to this 
		/// </summary>
		private Light[]		m_Lights	= new Light[ MaxLights ];

		/// <summary>
		/// Number of lights
		/// </summary>
		private int			m_NumLights	= 0;

		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			if ( parentObject is ISceneRenderable )
			{
				( ( ISceneRenderable )parentObject ).PreRenderList.Add( this );
			}
		}

		#endregion
	}
}
