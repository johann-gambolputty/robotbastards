using System;
using System.Windows.Forms;

namespace Poc1.ParticleSystemBuilder
{
	public partial class ComponentControl : UserControl
	{
		public event Action<object> SelectedControlChanged;

		public ComponentControl( )
		{
			InitializeComponent( );
		}

		public string ControlDisplayName
		{
			get { return groupBox1.Text; }
			set { groupBox1.Text = value; }
		}

		public object ControlObject
		{
			get { return m_Control; }
			set
			{
				m_Control = value;
			}
		}

		public void Setup( Type[] controlTypes, Type defaultType )
		{
			ControlTypes = controlTypes;
			DefaultControlType = defaultType;
		}

		public Type DefaultControlType
		{
			set
			{
				m_DefaultType = value;
				if ( m_DefaultType != null )
				{
					typeComboBox.SelectedItem = FindWrapper( m_DefaultType );
				}
			}
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
				if ( m_DefaultType != null )
				{
					typeComboBox.SelectedItem = FindWrapper( m_DefaultType );
				}
			}
		}

		private Type m_DefaultType;
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

		private TypeWrapper FindWrapper( Type type )
		{
			foreach ( TypeWrapper wrapper in typeComboBox.Items )
			{
				if ( wrapper.AssociatedType == type )
				{
					return wrapper;
				}
			}
			return null;
		}

		private void typeComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( typeComboBox.SelectedItem == null )
			{
				return;
			}
			Type selectedType = ( ( TypeWrapper )typeComboBox.SelectedItem ).AssociatedType;
			m_Control = Activator.CreateInstance( selectedType );

			controlPropertyGrid.SelectedObject = m_Control;

			if ( SelectedControlChanged != null )
			{
				SelectedControlChanged( m_Control );
			}
		}
	}
}
