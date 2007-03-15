using System;
using RbEngine.Rendering;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Simple OpenGL mesh
	/// </summary>
	public class OpenGlMesh : IRender, RbEngine.Components.INamedObject
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public OpenGlMesh( )
		{
		}

		#region	Group setup

		/// <summary>
		/// Creates the mesh index buffer
		/// </summary>
		public void			CreateGroups( int size )
		{
			m_Groups = new OpenGlIndexedGroup[ size ];
		}

		/// <summary>
		/// Sets up a group's index buffer
		/// </summary>
		public void			SetGroup( int index, OpenGlIndexedGroup group )
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
			m_VertexBuffers = new OpenGlVertexBuffer[ size ];
		}

		/// <summary>
		/// Sets a vertex buffer
		/// </summary>
		public void			SetVertexBuffer( int index, OpenGlVertexBuffer vertexBuffer )
		{
			m_VertexBuffers[ index ] = vertexBuffer;
		}

		#endregion

		#region	Texture setup

		/// <summary>
		/// Sets up a texture
		/// </summary>
		public void AddTexture( Texture2d texture )
		{
			m_Textures[ m_NumTextures++ ].Texture = texture;
		}

		/// <summary>
		/// Gets a texture
		/// </summary>
		public Texture2d	GetTexture( int index )
		{
			return m_Textures[ index ].Texture;
		}

		#endregion

		#region	Mesh rendering properties

		/// <summary>
		/// The mesh render effect
		/// </summary>
		public RenderEffect	Effect
		{
			get
			{
				return m_Technique.Effect;
			}
			set
			{
				m_Technique.Effect = value;
			}
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string		AppliedTechniqueName
		{
			get
			{
				return ( m_Technique.Technique == null ) ? string.Empty : m_Technique.Technique.Name;
			}
			set
			{
				m_Technique.SelectTechnique( value );
			}
		}

		#endregion
		
		#region IRender Members

		/// <summary>
		/// Renders this mesh
		/// </summary>
		public void Render( )
		{
			m_Technique.Apply( new TechniqueRenderDelegate( RenderMesh ) );
		}

		/// <summary>
		/// Renders the mesh
		/// </summary>
		private void RenderMesh( )
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

		#region INamedObject Members

		/// <summary>
		/// Event, invoked when the named of this mesh is changed
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// The name of this mesh
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		#endregion	

		#region	Private stuff

		private OpenGlIndexedGroup[]	m_Groups;
		private OpenGlVertexBuffer[]	m_VertexBuffers;
		private string					m_Name;
		private AppliedTechnique		m_Technique = new AppliedTechnique( );
		private TextureSampler2d[]		m_Textures = new TextureSampler2d[ 8 ];
		private int						m_NumTextures;

		#endregion
	}
}
