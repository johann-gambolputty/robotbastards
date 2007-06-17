using System;
using Rb.Core.Components;
using Rb.Rendering;

namespace Rb.Rendering.OpenGl.Md3Loader
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
	public class Model : INamed, IRenderable, IInstanceBuilder
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

		#region	Animations

		/// <summary>
		/// Access to the animation set associated with this model
		/// </summary>
		public AnimationSet Animations
		{
			set
			{
				m_Animations = value;
			}
			get
			{
				return m_Animations;
			}
		}

		#endregion

		#region	IInstanceBuilder Members

		/// <summary>
		/// Creates an instance of this object
		/// </summary>
		public Object CreateInstance( )
		{
			return new ModelInstance( this );
		}

		/// <summary>
		/// Creates an instance of this object, with the specified object list as construction parameters
		/// </summary>
		public Object CreateInstance( object[] constructorParams )
		{
			throw new ApplicationException( "Can't instance Rb.Rendering.OpenGl.Md3Loader.Model with parameters" );
		}

		#endregion

        #region INamed Members

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
			}
		}

		#endregion

		#region	Render effect

		/// <summary>
		/// Applies the specified render effect to all part meshes
		/// </summary>
		public Rb.Rendering.RenderEffect	Effect
		{
			set
			{
				for ( int partIndex = 0; partIndex < m_PartMeshes.Length; ++partIndex )
				{
					m_PartMeshes[ partIndex ].Effect = value;
				}
			}
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string		AppliedTechniqueName
		{
			set
			{
				for ( int partIndex = 0; partIndex < m_PartMeshes.Length; ++partIndex )
				{
					m_PartMeshes[ partIndex ].AppliedTechniqueName = value;
				}
			}
		}

		#endregion

		/// <summary>
		/// Renders the model with a particular set of animation layers
		/// </summary>
		public void Render( AnimationLayer[] layers )
		{
			m_PartMeshes[ 0 ].Render( layers );
		}

		#region IRenderable Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( IRenderContext context )
		{
			m_PartMeshes[ 0 ].Render( );
		}

		#endregion

		private AnimationSet	m_Animations;
		private Mesh[]			m_PartMeshes = new Mesh[ ( int )ModelPart.NumParts ];
		private string			m_Name;

	}
}