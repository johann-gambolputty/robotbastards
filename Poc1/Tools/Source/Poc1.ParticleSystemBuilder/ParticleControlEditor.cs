using System;
using System.Windows.Forms;

namespace Poc1.ParticleSystemBuilder
{
	public partial class ParticleControlEditor : UserControl
	{
		public ParticleControlEditor( )
		{
			InitializeComponent( );
		}

		public string ControlDisplayName
		{
			get { return groupBox1.Text; }
			set { groupBox1.Text = value; }
		}

		public Type[] ControlTypes
		{
			get { return m_Types; }
			set
			{
				m_Types = value;
				typeComboBox.Items.Clear( );
				if ( m_Types == null )
				{
					return;
				}
				foreach ( Type type in m_Types )
				{
					typeComboBox.Items.Add( new TypeWrapper( type ) );
				}
				if ( m_Types.Length > 0 )
				{
					typeComboBox.SelectedIndex = 0;
				}
			}
		}

		private Type[] m_Types;
		private object m_Control;

		private class TypeWrapper
		{
			public TypeWrapper( Type t )
			{
				m_Type = t;
			}

			public override string ToString( )
			{
				return m_Type.Name;
			}

			public Type AssociatedType
			{
				get { return m_Type; }
			}

			private readonly Type m_Type;
		}

		private void typeComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			Type selectedType = ( ( TypeWrapper )typeComboBox.SelectedItem ).AssociatedType;
			m_Control = Activator.CreateInstance( selectedType );

			controlPropertyGrid.SelectedObject = m_Control;
		}
	}
}
