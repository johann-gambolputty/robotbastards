using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A template for building instances of a type
	/// </summary>
	public class ObjectPattern : Node, ICloneable, ISupportsDynamicProperties
	{
		/// <summary>
		/// The template type
		/// </summary>
		public Type Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		/// <summary>
		/// Gets child patterns stored in this pattern
		/// </summary>
		public IEnumerable< ObjectPattern > ChildPatterns
		{
			get
			{
				foreach ( object obj in Children )
				{
					ObjectPattern pattern = obj as ObjectPattern;
					if ( pattern != null )
					{
						yield return pattern;
					}
				}
			}
		}

		/// <summary>
		/// Template name
		/// </summary>
		public string TemplateName
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		///	Property accessor
		/// </summary>
		public object this[ string propertyName ]
		{
			get { return m_Properties[ propertyName ]; }
			set { m_Properties[ propertyName ] = value; }
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="type">The type of object wrapped by this template</param>
		/// <param name="name">Template name</param>
		public ObjectPattern( Type type, string name )
		{
			Type = type;
			TemplateName = name;

			string category = type.Name.Substring( type.Name.LastIndexOf( '.' ) + 1 );

			PropertyInfo[] srcProperties = type.GetProperties( );
			foreach ( PropertyInfo srcProperty in srcProperties )
			{
				m_Properties.Add( new Property( srcProperty, category ) );
			}
		}

		/// <summary>
		/// Cloning (deep copy) constructor
		/// </summary>
		/// <param name="srcTemplate">Source template</param>
		public ObjectPattern( ObjectPattern srcTemplate )
		{
			m_Type = srcTemplate.m_Type;
			m_Name = srcTemplate.m_Name;

			//	Copy properties
			foreach ( Property property in srcTemplate.m_Properties )
			{
				m_Properties.Add( new Property( property ) );
			}

			//	Copy children
			CopyChildObjects( srcTemplate.Children );
		}

		private static XmlAttribute NewXmlAttribute( XmlDocument doc, string name, string value )
		{
			XmlAttribute attr = doc.CreateAttribute( name );
			attr.Value = value;
			return attr;
		}

		public XmlNode WriteToXml( XmlDocument doc )
		{
			XmlNode node = doc.CreateElement( "object" );

			node.Attributes.Append( NewXmlAttribute( doc, "type", m_Type.Name ) );
			node.Attributes.Append( NewXmlAttribute( doc, "assembly", m_Type.Assembly.GetName( ).Name ) );

			foreach ( Property property in m_Properties )
			{
				if ( m_Type.GetProperty( property.Name ) != null )
				{
				}
			}

			foreach ( ObjectPattern pattern in ChildPatterns )
			{
				node.AppendChild( pattern.WriteToXml( doc ) );
			}

			return node;
		}
		
		/// <summary>
		/// Finds a template whose type supports a given interface
		/// </summary>
		public ObjectPattern FindPatternImplementingInterface< InterfaceType >( )
		{
			if ( m_Type.GetInterface( typeof( InterfaceType ).Name ) != null )
			{
				return this;
			}
			foreach ( ObjectPattern childPattern in ChildPatterns )
			{
				ObjectPattern result = childPattern.FindPatternImplementingInterface< InterfaceType >( );
				if ( result != null )
				{
					return result;
				}
			}
			return null;
		}

		/// <summary>
		/// Adds a child template
		/// </summary>
		/// <param name="template">Child template</param>
		public void AddChildTemplate( ObjectPattern template )
		{
			AddChild( template );
		}

		/// <summary>
		/// Creates a property bag for this template, and all child templates
		/// </summary>
		/// <returns>Returns the new property bag</returns>
		public PropertyBag CreatePropertyBag( )
		{
			PropertyBag bag = new PropertyBag( );

			bag.GetValue += GetValueHandler;
			bag.SetValue += SetValueHandler;

			AddToPropertyBag( bag );

			return bag;
		}

		private class Property : PropertySpec, IDynamicProperty
		{
			public Property( PropertyInfo property, string category ) :
				base( property.Name, property.PropertyType, category )
			{
			}

			public Property( Property src ) :
				base( src.Name, src.TypeName, src.Category, src.Description, src.DefaultValue, src.EditorTypeName, src.ConverterTypeName )
			{
				m_Value = src.m_Value;
			}

			private object m_Value;

			#region IDynamicProperty Members

			/// <summary>
			/// Accessor to the property value
			/// </summary>
			public object Value
			{
				get { return m_Value; }
				set { m_Value = value; }
			}

			#endregion
		}

		private static void GetValueHandler( object sender, PropertySpecEventArgs args )
		{
			args.Value = ( ( Property )args.Property ).Value;
		}

		private static void SetValueHandler( object sender, PropertySpecEventArgs args )
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

			foreach ( object childObj in Children )
			{
				ObjectPattern childTemplate = childObj as ObjectPattern;
				if ( childTemplate != null )
				{
					childTemplate.AddToPropertyBag( bag );
				}
			}
		}

		/// <summary>
		/// Copies child objects from children
		/// </summary>
		private void CopyChildObjects( IEnumerable children )
		{
			foreach ( object childObj in children )
			{
				AddChild( ( ( ICloneable )childObj ).Clone( ) );
			}
		}
		
		#region ICloneable Members

		/// <summary>
		/// Returns a clone of this template
		/// </summary>
		/// <returns>Template clone</returns>
		public object Clone( )
		{
			return new ObjectPattern( this );
		}

		#endregion

		private string m_Name;
		private Type m_Type;
		private readonly DynamicProperties m_Properties = new DynamicProperties( );

		#region ISupportsDynamicProperties Members

		/// <summary>
		/// Gets the properties associated with this template
		/// </summary>
		public IDynamicProperties Properties
		{
			get { return m_Properties; }
		}

		#endregion
	}
}
