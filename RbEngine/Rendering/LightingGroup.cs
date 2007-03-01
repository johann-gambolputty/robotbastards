using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Stores a set of up to MaxLights lights that can be attached to an object
	/// </summary>
	public class LightingGroup : IApplicable
	{
		/// <summary>
		/// Sets the object that this lighting group is associated with
		/// </summary>
		public LightingGroup( Object obj )
		{
			m_Object = obj;

			( ( Scene.ISceneRenderable ) ).PreRenderList.Add( this );
		}

		/// <summary>
		/// Gets the object that this lighting group is associated with
		/// </summary>
		public Object		AssociatedObject
		{
			get
			{
				return m_Object;
			}
		}

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

		#region IApplicable Members

		/// <summary>
		/// Applies this lighting group
		/// </summary>
		public void Apply( )
		{
			Renderer renderer = Renderer.Inst;

			renderer.ClearLights( );
			for ( int lightIndex = 0; lightIndex < NumLights; ++lightIndex )
			{
				renderer.AddLight( m_Lights[ lightIndex ] );
			}
		}

		#endregion

		/// <summary>
		/// Maximum number of lights in a group
		/// </summary>
		public const int	MaxLights = 4;

		/// <summary>
		/// Stores lights that have been added to this 
		/// </summary>
		private Light[]		m_Lights	= new Light[ MaxLights ];

		/// <summary>
		/// Number of lights
		/// </summary>
		private int			m_NumLights	= 0;
		
		private Object		m_Object;

	}
}
