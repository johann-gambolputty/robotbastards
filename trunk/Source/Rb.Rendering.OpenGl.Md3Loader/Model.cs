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
			set { m_Animations = value; }
			get { return m_Animations; }
		}

		#endregion

		#region	IInstanceBuilder Members

		/// <summary>
		/// Creates an instance of this object
		/// </summary>
		public Object CreateInstance( IBuilder builder )
		{
			return Builder.CreateInstance< ModelInstance >( builder, this );
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
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region	Render effect

		/// <summary>
		/// Applies the specified render effect to all part meshes
		/// </summary>
		public IEffect Effect
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
		public string TechniqueName
		{
			set
			{
				for ( int partIndex = 0; partIndex < m_PartMeshes.Length; ++partIndex )
				{
					m_PartMeshes[ partIndex ].TechniqueName = value;
				}
			}
		}

		#endregion

		#region Rendering

		/// <summary>
		/// Renders the model with a particular set of animation layers
		/// </summary>
		public void Render( IRenderContext context, AnimationLayer[] layers )
		{
			Mesh.FrameInfo frame = m_PartMeshes[ 0 ].GetAnimationFrame( layers );
			Graphics.Renderer.Translate( Transform.LocalToWorld, 0, ( frame.MaxBounds - frame.MinBounds ).Y / 2, 0 );
			m_PartMeshes[ 0 ].Render( context, layers );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( IRenderContext context )
		{
			m_PartMeshes[ 0 ].Render( context );
		}

		#endregion

		private AnimationSet	m_Animations;
		private readonly Mesh[]	m_PartMeshes = new Mesh[ ( int )ModelPart.NumParts ];
		private string			m_Name;
	}
}
