using System;
using System.Collections;
using System.ComponentModel;
using Rb.Core.Components;
using Rb.Rendering.Interfaces.Objects;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Base class for standard implementations of <see cref="IObjectEditor"/>
	/// </summary>
	[Serializable]
	public abstract class ObjectEditor : IObjectEditor, IRenderable, ISelectable, ISceneObject, IDelete, IUnique
	{
		#region IObjectEditor members

		/// <summary>
		/// Event, invoked when this object is changed
		/// </summary>
		public event EventHandler ObjectChanged
		{
			add { m_ObjectChanged += value; }
			remove { m_ObjectChanged -= value; }
		}

		/// <summary>
		/// Builds scene objects
		/// </summary>
		/// <param name="scene">Scene to add objects to</param>
		public abstract void Build( Scene scene );

		#endregion

		#region ISelectable Members

		/// <summary>
		/// Gets/sets the selected flag
		/// </summary>
		[Browsable( false )]
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		/// <summary>
		/// Gets/sets the highlighted flag
		/// </summary>
		[Browsable( false )]
		public bool Highlighted
		{
			get { return m_Highlighted; }
			set { m_Highlighted = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders all child selectable objects
		/// </summary>
		/// <param name="context">Rendering context</param>
		public virtual void Render( IRenderContext context )
		{
			foreach ( object modifier in m_Modifiers )
			{
				IRenderable renderModifier = modifier as IRenderable;
				if ( renderModifier != null )
				{
					renderModifier.Render( context );
				}
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		public void RemovedFromScene( Scene scene )
		{
			scene.Renderables.Remove( this );
		}

		#endregion
		
		#region IDelete Members

		/// <summary>
		/// Adds this object back to the scene
		/// </summary>
		public void UnDelete( )
		{
			EditorState.Instance.CurrentScene.Objects.Add( this );
		}

		/// <summary>
		/// Removes this object from the scene
		/// </summary>
		public void Delete( )
		{
			EditorState.Instance.CurrentScene.Objects.Remove( this );
		}

		#endregion

		#region IUnique Members

		/// <summary>
		/// Gets/sets the unique ID of this object
		/// </summary>
		[ReadOnly(true)]
		public Guid Id
		{
			get { return m_Id; }
			set { m_Id = value; }
		}

		#endregion

		#region Protected members
		
		/// <summary>
		/// Adds a modifier object
		/// </summary>
		protected void AddModifier( object modifier )
		{
			m_Modifiers.Add( modifier );
		}
		
		/// <summary>
		/// Invokes the <see cref="ObjectChanged"/> event
		/// </summary>
		protected void OnObjectChanged( )
		{
			if ( m_ObjectChanged != null )
			{
				m_ObjectChanged( this, null );
			}
		}

		#endregion

		#region Private members

		[NonSerialized]
		private bool m_Selected;

		[NonSerialized]
		private bool m_Highlighted;

		[NonSerialized]
		private EventHandler m_ObjectChanged;

		private Guid m_Id = Guid.NewGuid( );
		private readonly ArrayList m_Modifiers = new ArrayList( );

		#endregion

	}
}
