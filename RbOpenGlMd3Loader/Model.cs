using System;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// MD3 model parts
	/// </summary>
	public enum ModelPart
	{
		Lower,
		Upper,
		Head,
		NumParts
	}

	/// <summary>
	/// MD3 model
	/// </summary>
	public class Model : RbEngine.Components.INamedObject, RbEngine.Rendering.IRender
	{
		#region	Meshes

		/// <summary>
		/// Sets the mesh for a specified body part
		/// </summary>
		public void SetPartMesh( ModelPart part, Mesh mesh )
		{
			m_PartMeshes[ ( int )part ] = mesh;
		}

		/// <summary>
		/// Gets the mesh for a specified body part
		/// </summary>
		public Mesh GetPartMesh( ModelPart part )
		{
			return m_PartMeshes[ ( int )part ];
		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// Name changed event
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// Model name
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

		#region	Render effect

		/// <summary>
		/// Applies the specified render effect to all part meshes
		/// </summary>
		public RbEngine.Rendering.RenderEffect	Effect
		{
			set
			{
				for ( int partIndex = 0; partIndex < m_PartMeshes.Length; ++partIndex )
				{
					m_PartMeshes[ partIndex ].Effect = value;
				}
			}
			//	TODO: XmlLoader property bodge
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string		SelectedTechniqueName
		{
			set
			{
				for ( int partIndex = 0; partIndex < m_PartMeshes.Length; ++partIndex )
				{
					m_PartMeshes[ partIndex ].SelectedTechniqueName = value;
				}
			}
			//	TODO: XmlLoader property bodge
			get
			{
				return null;
			}
		}

		#endregion
		
		#region IRender Members
		#endregion

		private Mesh[]	m_PartMeshes = new Mesh[ ( int )ModelPart.NumParts ];
		private string	m_Name;

		#region IRender Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( )
		{
			m_PartMeshes[ 0 ].Render( );
		}

		#endregion
	}
}
