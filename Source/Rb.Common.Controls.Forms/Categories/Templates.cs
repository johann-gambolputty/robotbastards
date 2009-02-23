using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace SchemaTest
{
	public interface IModelTemplate : IComponent
	{
	}

	public class ModelTemplate : IModelTemplate
	{
		public ModelTemplate( string name )
		{
			m_Name = name;
		}

		public override string ToString( )
		{
			return m_Name;
		}
		
		#region IComponent Members

		/// <summary>
		/// Gets/sets the composite object that contains this component
		/// </summary>
		public IComposite Owner
		{
			get { return m_Owner; }
			set
			{
				if ( m_Owner == value )
				{
					return;
				}
				if ( m_Owner != null )
				{
					m_Owner.Remove( this );
				}
				m_Owner = value;
				if ( m_Owner != null )
				{
					m_Owner.Add( this );
				}
			}
		}

		#endregion

		private IComposite m_Owner;
		private readonly string m_Name;
	}


}
