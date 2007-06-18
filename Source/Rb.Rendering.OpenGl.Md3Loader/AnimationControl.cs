using System.Collections.Generic;
using Rb.Animation;
using Rb.Core.Components;

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
		public void AddLayer( AnimationLayer layer )
		{
			m_Layers.Add( layer );
		}

		/// <summary>
		/// Gets an animation layer
		/// </summary>
		public IAnimationLayer GetLayer( string name )
		{
			for ( int layerIndex = 0; layerIndex < m_Layers.Count; ++layerIndex )
			{
				if ( ( ( INamed )m_Layers[ layerIndex ] ).Name == name )
				{
					return m_Layers[ layerIndex ];
				}
			}
			return null;
		}

		private List< AnimationLayer > m_Layers = new List< AnimationLayer >( );
	}
}
