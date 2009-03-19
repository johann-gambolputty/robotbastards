using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.Core.Components
{
	/// <summary>
	/// A template for building instances of a type
	/// </summary>
	[Serializable]
	[Obsolete]
	public class ObjectTemplate : IComposite, ICloneable, ISupportsDynamicProperties, INamed, IInstanceBuilder
	{
		#region Public properties

		/// <summary>
		/// The template type
		/// </summary>
		public Type Type
		{
			get { return m_Type; }
			set
			{
				m_Type = value;
				m_CanAddChildren =	( m_Type.GetInterface( typeof( IComposite ).Name ) != null ) ||
									( m_Type.GetInterface( typeof( IList ).Name ) != null );
			}
		}

		/// <summary>
		/// Gets child templates stored in this template
		/// </summary>
		public IEnumerable< ObjectTemplate > ChildTemplates
		{
			get { return m_ChildTemplates; }
		}

		/// <summary>
		///	Property accessor
		/// </summary>
		public object this[ string propertyName ]
		{
			get { return m_Properties[ propertyName ]; }
			set { m_Properties[ propertyName ] = value; }
		}

		#endregion

		#region Public construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="type">The type of object wrapped by this template</param>
		/// <param name="name">Template name</param>
		public ObjectTemplate( Type type, string name )
		{
			Type = type;
			m_Name = name;

			string category = type.Name.Substring( type.Name.LastIndexOf( '.' ) + 1 );

			PropertyInfo[] srcProperties = type.GetProperties( );
			foreach ( PropertyInfo srcProperty in srcProperties )
			{
				if ( !m_Properties.Contains( srcProperty.Name ) )
				{
					continue;
				}
				m_Properties.Add( new Property( srcProperty, category ) );
			}
		}

		/// <summary>
		/// Cloning (deep copy) constructor
		/// </summary>
		/// <param name="srcTemplate">Source template</param>
		public ObjectTemplate( ObjectTemplate srcTemplate )
		{
			Type = srcTemplate.m_Type;
			m_Name = srcTemplate.m_Name;

			//	Copy properties
			foreach ( Property property in srcTemplate.m_Properties )
			{
				m_Properties.Add( new Property( property ) );
			}

			//	Copy children
			CopyChildTemplates( srcTemplate.ChildTemplates );
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Gets the name of this template
		/// </summary>
		/// <returns>Template name</returns>
		public override string ToString( )
		{
			return Name;
		}

		/// <summary>
		/// Writes this template to XML
		/// </summary>
		/// <param name="doc">XML document</param>
		/// <returns>Returns a node representing this template</returns>
		public XmlNode WriteToXml( XmlDocument doc )
		{
			XmlNode node = doc.CreateElement( "object" );

			node.Attributes.Append( XmlHelpers.CreateAttribute( doc, "type", m_Type.FullName ) );
			node.Attributes.Append( XmlHelpers.CreateAttribute( doc, "assembly", m_Type.Assembly.GetName( ).Name ) );

			foreach ( Property property in m_Properties )
			{
				object value = property.Value;
				if ( value != null )
				{
					XmlNode propertyNode = WriteObjectToXml( doc, value );
					propertyNode.Attributes.Append( XmlHelpers.CreateAttribute( doc, "property", property.Name ) );
					node.AppendChild( propertyNode );
				}
			}

			foreach ( ObjectTemplate template in ChildTemplates )
			{
				node.AppendChild( template.WriteToXml( doc ) );
			}

			return node;
		}

		private static XmlNode WriteObjectToXml( XmlDocument doc, object obj )
		{
			if ( obj is ObjectTemplate )
			{
				return ( ( ObjectTemplate )obj ).WriteToXml( doc );
			}

			XmlNode node = doc.CreateElement( "object" );
			node.Attributes.Append( XmlHelpers.CreateAttribute( doc, "type", obj.GetType( ).FullName ) );
			node.Attributes.Append( XmlHelpers.CreateAttribute( doc, "assembly", obj.GetType( ).Assembly.GetName( ).Name ) );

			return node;
		}

		#endregion

		#region Child templates

		/// <summary>
		/// Finds a template whose type is derived from (or implements) type T
		/// </summary>
		public ObjectTemplate FindTemplateOfType< T >( )
		{
			if ( ( m_Type == typeof( T ) ) ||
				 ( m_Type.IsSubclassOf( typeof( T ) ) ) ||
				 ( m_Type.GetInterface( typeof( T ).Name ) != null ) )
			{
				return this;
			}
			foreach ( ObjectTemplate childTemplate in ChildTemplates )
			{
				ObjectTemplate result = childTemplate.FindTemplateOfType< T >( );
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
		public void AddChildTemplate( ObjectTemplate template )
		{
			//	Maintain a separate list of child templates
			if ( !m_CanAddChildren )
			{
				throw new InvalidOperationException( string.Format( "Can't add child templates, because type \"{0}\" can't accept child objects", m_Type ) );
			}
			m_ChildTemplates.Add( template );
		}

		#endregion

		#region Property bag creation

		/// <summary>
		/// Creates a property bag for this template, and all child templates
		/// </summary>
		/// <param name="addChildTemplates">If true, adds child template properties to the bag</param>
		/// <returns>Returns the new property bag</returns>
		public PropertyBag CreatePropertyBag( bool addChildTemplates )
		{
			PropertyBag bag = new PropertyBag( );

			bag.GetValue += GetValueHandler;
			bag.SetValue += SetValueHandler;

			AddToPropertyBag( bag, addChildTemplates );

			return bag;
		}

		#endregion

		#region Private property bag stuff

		[Serializable]
		private class Property : PropertySpec, IDynamicProperty
		{
			public Property( PropertyInfo property, string category ) :
				base( property.Name, property.PropertyType, category )
			{
				m_Property = property;
			}

			public Property( Property src ) :
				base( src.Name, src.TypeName, src.Category, src.Description, src.DefaultValue, src.EditorTypeName, src.ConverterTypeName )
			{
				m_Property = src.m_Property;
				m_Value = src.m_Value;
			}

			public bool CanAddToPropertyBag
			{
				get
				{
					return ( m_Property.CanRead ) && ( m_Property.GetIndexParameters( ).Length == 0 );
				}
			}

			private object m_Value;
			private readonly PropertyInfo m_Property;

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
		/// <param name="addChildTemplates">If true, adds child template properties to the bag</param>
		private void AddToPropertyBag( PropertyBag bag, bool addChildTemplates )
		{
			foreach ( Property property in m_Properties )
			{
				if ( property.CanAddToPropertyBag )
				{
					bag.Properties.Add( property );
				}
			}

			if ( addChildTemplates )
			{
				foreach ( ObjectTemplate childTemplate in ChildTemplates )
				{
					childTemplate.AddToPropertyBag( bag, addChildTemplates );
				}
			}
		}

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Returns a clone of this template
		/// </summary>
		/// <returns>Template clone</returns>
		public object Clone( )
		{
			return new ObjectTemplate( this );
		}

		#endregion

		#region ISupportsDynamicProperties Members

		/// <summary>
		/// Gets the properties associated with this template
		/// </summary>
		public IDynamicProperties Properties
		{
			get { return m_Properties; }
		}

		#endregion

		#region INamed Members

		/// <summary>
		/// Template name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region Private Members

		private readonly List<ObjectTemplate> m_ChildTemplates = new List<ObjectTemplate>( );
		private string m_Name;
		private Type m_Type;
		private bool m_CanAddChildren;
		private readonly DynamicProperties m_Properties = new DynamicProperties( );
		
		/// <summary>
		/// Copies child objects from children
		/// </summary>
		private void CopyChildTemplates( IEnumerable children )
		{
			foreach ( ObjectTemplate childObj in children )
			{
				Add( childObj.Clone( ) );
			}
		}

		#endregion

		#region IInstanceBuilder Members

		/// <summary>
		/// Creates an instance of the stored object, and any child objects
		/// </summary>
		/// <param name="builder">Builder to use to create the type</param>
		/// <returns>Returns the template instance</returns>
		public object CreateInstance( IBuilder builder )
		{
			object result = builder.CreateInstance( m_Type );

			foreach ( IDynamicProperty property in m_Properties )
			{
				object value = property.Value;
				if ( value != null )
				{
					if ( value is ObjectTemplate )
					{
						value = ( ( ObjectTemplate )value ).CreateInstance( builder );
					}

					m_Type.GetProperty( property.Name ).SetValue( result, value, null );
				}
			}

			if ( m_ChildTemplates.Count == 0 )
			{
				return result;
			}

			if ( result is IComposite )
			{
				IComposite resultParent = ( IComposite )result;
				foreach ( ObjectTemplate childBuilder in m_ChildTemplates )
				{
					resultParent.Add( childBuilder.CreateInstance( builder ) );
				}
			}
			else if ( result is IList )
			{
				IList resultList = ( IList )result;
				foreach ( ObjectTemplate childBuilder in m_ChildTemplates )
				{
					resultList.Add( childBuilder.CreateInstance( builder ) );
				}
			}

			return result;
		}

		#endregion

		#region IComposite Members

		/// <summary>
		/// Returns the child templates
		/// </summary>
		public IList Components
		{
			get { return m_ChildTemplates.AsReadOnly( ); }
		}

		/// <summary>
		/// Adds a child component
		/// </summary>
		public void Add( object obj )
		{
			AddChildTemplate( ( ObjectTemplate )obj );
			if ( ComponentAdded != null )
			{
				ComponentAdded( this, obj );
			}
		}

		/// <summary>
		/// Removes a child component
		/// </summary>
		public void Remove( object obj )
		{
			m_ChildTemplates.Remove( ( ObjectTemplate )obj );
			if ( ComponentRemoved != null )
			{
				ComponentRemoved( this, obj );
			}
		}

		/// <summary>
		/// Removes all child components
		/// </summary>
		public void Clear( )
		{
			while ( m_ChildTemplates.Count > 0 )
			{
				Remove( m_ChildTemplates[ 0 ] );
			}
		}

		/// <summary>
		/// Event, invoked after a child template is added to this template
		/// </summary>
		public event OnComponentAddedDelegate ComponentAdded;

		/// <summary>
		/// Event, invoked after a child template is removed from this template
		/// </summary>
		public event OnComponentRemovedDelegate ComponentRemoved;

		#endregion
	}
}
