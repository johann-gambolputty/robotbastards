using System;
using System.Collections;
using Rb.Animation;

namespace Rb.Rendering.OpenGl.Md3Loader
{
	/// <summary>
	/// Controls animations in an MD3 mesh
	/// </summary>
	public class AnimationControl : IAnimationControl
	{
		/// <summary>
		/// Adds an animation layer to the control
		/// </summary>
		public void				AddLayer( AnimationLayer layer )
		{
			m_Layers.Add( layer );
		}

		/// <summary>
		/// Gets an animation layer
		/// </summary>
		public IAnimationLayer	GetLayer( string name )
		{
			for ( int layerIndex = 0; layerIndex < m_Layers.Count; ++layerIndex )
			{
				if ( ( ( Rb.Components.INamedObject )m_Layers[ layerIndex ] ).Name == name )
				{
					return ( IAnimationLayer )m_Layers[ layerIndex ];
				}
			}
			return null;
		}

		private ArrayList		m_Layers = new ArrayList( );
	}
}
