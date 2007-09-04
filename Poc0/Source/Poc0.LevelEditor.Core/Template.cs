using System;
using System.Collections.Generic;
using System.Reflection;
using Poc0.Core;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A template for building instances of a type
	/// </summary>
	public class Template : Component, ICloneable
	{
		/// <summary>
		/// The template type
		/// </summary>
		public Type Type
		{
			get { return m_Type; }
			set
			{
				m_Type = value;
				if ( m_Type.GetInterface( typeof( IHasWorldFrame ).Name ) != null )
				{
					AddChild( new HasWorldFrame( ) );
				}
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="type">The type of object wrapped by this template</param>
		public Template( Type type )
		{
			Type = type;

			string category = type.Name.Substring( type.Name.LastIndexOf( '.' ) + 1 );

			PropertyInfo[] srcProperties = type.GetProperties( );

			List< Property > dstProperties = new List< Property >( );

			foreach ( PropertyInfo srcProperty in srcProperties )
			{
				dstProperties.Add( new Property( srcProperty, category ) );
			}

			m_Properties = dstProperties.ToArray( );
		}

		/// <summary>
		/// Cloning (deep copy) constructor
		/// </summary>
		/// <param name="srcTemplate">Source template</param>
		public Template( Template srcTemplate )
		{
			m_Type = srcTemplate.m_Type;
			m_Properties = ( Property[] )srcTemplate.m_Properties.Clone( );

			foreach ( Template srcChildTemplate in srcTemplate.ChildTemplates )
			{
				m_ChildTemplates.Add( new Template( srcChildTemplate ) );
			}
		}

		/// <summary>
		/// Adds a child template
		/// </summary>
		/// <param name="template">Child template</param>
		public void AddChildTemplate( Template template )
		{
			m_ChildTemplates.Add( template );
		}

		/// <summary>
		/// Gets the list of child templates
		/// </summary>
		public IEnumerable< Template > ChildTemplates
		{
			get { return m_ChildTemplates; }
		}

		/// <summary>
		/// Creates a property bag for this template, and all child templates
		/// </summary>
		/// <returns>Returns the new property bag</returns>
		public PropertyBag CreatePropertyBag( )
		{
			PropertyBag bag = new PropertyBag( );

			bag.GetValue += GetValue;
			bag.SetValue += SetValue;

			AddToPropertyBag( bag );

			return bag;
		}

		private class Property : PropertySpec
		{
			public Property( PropertyInfo property, string category ) :
				base( property.Name, property.PropertyType, category )
			{
			}

			public object Value
			{
				get { return m_Value; }
				set { m_Value = value; }
			}

			private object m_Value;
		}

		private static void GetValue( object sender, PropertySpecEventArgs args )
		{
			args.Value = ( ( Property )args.Property ).Value;
		}

		private static void SetValue( object sender, PropertySpecEventArgs args )
		{
			( ( Property )args.Property ).Value = args.Value;
		}

		/// <summary>
		/// Adds this template to the specified property bag
		/// </summary>
		/// <param name="bag">Property bag to add to</param>
		private void AddToPropertyBag( PropertyBag bag )
		{
			foreach ( Property property in m_Properties )
			{
				bag.Properties.Add( property );
			}

			foreach ( Template childTemplate in ChildTemplates )
			{
				childTemplate.AddToPropertyBag( bag );
			}
		}
		
		#region ICloneable Members

		/// <summary>
		/// Returns a clone of this template
		/// </summary>
		/// <returns>Template clone</returns>
		public object Clone( )
		{
			Template template = new Template( this );
			return template;
		}

		#endregion

		private Type m_Type;
		private readonly Property[] m_Properties;
		private readonly List< Template > m_ChildTemplates = new List< Template >( );
	}
}
