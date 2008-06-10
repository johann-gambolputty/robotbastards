using System;
using System.Windows.Forms;
using Poc1.Particles.Interfaces;

namespace Poc1.ParticleSystemBuilder
{
	public partial class CompositeComponentControl : UserControl
	{
		public CompositeComponentControl( )
		{
			InitializeComponent( );
		}


		public T BuildComponent< T >( IParticleSystemComponent component )
			where T : IParticleSystemCompositeComponent, new ( )
		{
			if ( component == null )
			{
				T wrapper = new T( );
				CompositeComponent = wrapper;
				return wrapper;
			}
			if ( !( component is IParticleSystemCompositeComponent ) )
			{
				T wrapper = new T( );
				wrapper.Add( component );
				component = wrapper;
			}

			CompositeComponent = ( IParticleSystemCompositeComponent )component;

			return ( T )component;
		}

		public IParticleSystemCompositeComponent CompositeComponent
		{
			get { return m_CompositeComponent; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				m_CompositeComponent = value;
				foreach ( IParticleSystemComponent component in m_CompositeComponent.Components )
				{
					componentsListBox.Items.Add( component );
				}
			}
		}

		public Type[] ComponentTypes
		{
			get { return m_ComponentTypes; }
			set
			{
				m_ComponentTypes = value ?? new Type[ 0 ];

				addComponentToolStripSplitButton.DropDownItems.Clear( );
				foreach ( Type type in m_ComponentTypes )
				{
					ToolStripItem item = new ComponentTypeToolStripButton( type );
					item.Click += OnAddComponentClicked;
					addComponentToolStripSplitButton.DropDownItems.Add( item );
				}
			}
		}

		private Type[] m_ComponentTypes = new Type[ 0 ];
		private IParticleSystemCompositeComponent m_CompositeComponent;

		private class ComponentTypeToolStripButton : ToolStripButton
		{
			public ComponentTypeToolStripButton( Type type ) :
				base( type.Name )
			{
				m_Type = type;
				m_Instance = Activator.CreateInstance( m_Type );
			}

			public Type Type
			{
				get { return m_Type; }
			}

			public object Instance
			{
				get { return m_Instance; }
			}

			private readonly Type m_Type;
			private readonly object m_Instance;
		}

		private void OnAddComponentClicked( object sender, EventArgs e )
		{
			ComponentTypeToolStripButton componentSender = ( ComponentTypeToolStripButton )sender;
			foreach ( object obj in componentsListBox.Items )
			{
				if ( obj.GetType( ) == componentSender.Type )
				{
					//	List box already contains one of these...
					return;
				}
			}
			componentsListBox.Items.Add( componentSender.Instance );
			CompositeComponent.Add( ( IParticleSystemComponent )componentSender.Instance );
		}

		private void removeToolStripButton_Click( object sender, EventArgs e )
		{
			if ( componentsListBox.SelectedItem == null )
			{
				return;
			}
			componentsListBox.Items.Remove( componentsListBox.SelectedItem );
			CompositeComponent.Remove( ( IParticleSystemComponent )componentsListBox.SelectedItem );
		}

		private void componentsListBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			selectedComponentPropertyGrid.SelectedObject = componentsListBox.SelectedItem;
		}
	}
}
