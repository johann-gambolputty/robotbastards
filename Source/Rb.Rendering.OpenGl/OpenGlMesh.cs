using Rb.Core.Components;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Simple OpenGL mesh
	/// </summary>
	public class OpenGlMesh : IRenderable, INamed
	{
		#region	Group setup

		/// <summary>
		/// Creates the mesh index buffer
		/// </summary>
		public void	CreateGroups( int size )
		{
			m_Groups = new OpenGlIndexedGroup[ size ];
		}

		/// <summary>
		/// Sets up a group's index buffer
		/// </summary>
		public void	SetGroup( int index, OpenGlIndexedGroup group )
		{
			m_Groups[ index ] = group;
		}

		#endregion

		#region	Vertex buffer setup

		/// <summary>
		/// Creates
		/// </summary>
		/// <param name="size"></param>
		public void			CreateVertexBuffers( int size )
		{
			m_VertexBuffers = new OpenGlVertexArray[ size ];
		}

		/// <summary>
		/// Sets a vertex buffer
		/// </summary>
		public void			SetVertexBuffer( int index, OpenGlVertexArray vertexBuffer )
		{
			m_VertexBuffers[ index ] = vertexBuffer;
		}

		#endregion

		#region	Texture setup

		/// <summary>
		/// Sets up a texture
		/// </summary>
		public void AddTexture( OpenGlTexture2d texture )
		{
			m_Textures[ m_NumTextures++ ].Texture = texture;
		}

		/// <summary>
		/// Gets a texture
		/// </summary>
		public OpenGlTexture2d GetTexture( int index )
		{
			return m_Textures[ index ].OpenGlTexture;
		}

		#endregion

		#region	Mesh rendering properties

		/// <summary>
		/// The mesh render effect
		/// </summary>
		public IEffect Effect
		{
			get { return m_Technique.Effect; }
			set { m_Technique.Effect = value; }
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string CurrentTechniqueName
		{
			get
			{
			    return m_Technique.Technique == null ? string.Empty : m_Technique.Technique.Name;
			}
			set
			{
                m_Technique.Select( value );
			}
		}

		#endregion
		
		#region IRenderable Members

		/// <summary>
		/// Renders this mesh
		/// </summary>
        public void Render( IRenderContext context )
		{
            context.ApplyTechnique( m_Technique.Technique, RenderMesh );
		}

		/// <summary>
		/// Renders the mesh
		/// </summary>
		private void RenderMesh( IRenderContext context )
		{
			for ( int textureIndex = 0; textureIndex < m_NumTextures; ++textureIndex )
			{
				m_Textures[ textureIndex ].Begin( );
			}

			for ( int vbIndex = 0; vbIndex < m_VertexBuffers.Length; ++vbIndex )
			{
				m_VertexBuffers[ vbIndex ].Begin( );
			}

			for ( int groupIndex = 0; groupIndex < m_Groups.Length; ++groupIndex )
			{
				m_Groups[ groupIndex ].Draw( );
			}

			for ( int vbIndex = 0; vbIndex < m_VertexBuffers.Length; ++vbIndex )
			{
				m_VertexBuffers[ vbIndex ].End( );
			}

			for ( int textureIndex = 0; textureIndex < m_NumTextures; ++textureIndex )
			{
				m_Textures[ textureIndex ].End( );
			}

		}

		#endregion

		#region INamed Members

		/// <summary>
		/// The name of this mesh
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion	

		#region	Private stuff

		private OpenGlIndexedGroup[]				m_Groups;
		private OpenGlVertexArray[]					m_VertexBuffers;
		private string								m_Name;
        private readonly TechniqueSelector          m_Technique = new TechniqueSelector( );
		private readonly OpenGlTexture2dSampler[]	m_Textures = new OpenGlTexture2dSampler[ 8 ];
		private int									m_NumTextures;

		#endregion
	}
}
