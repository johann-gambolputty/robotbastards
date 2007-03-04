using System;
using System.Xml;
using System.Collections;

namespace RbEngine.Components
{
	/*
	 * Design for factory support in XML:
	 * 
	 * 1:
	 *		<object factory="RenderFactory" call="NewRenderState"/>
	 * 
	 * OK, but that implies that all factories have "Inst" singletons. Can factories be temporary inputs?
	 * 
	 * NOTE: It would be good to tie this into the instancing architecture
	 * 
	 *		<object instanceOf="RenderState"/>
	 */

	/// <summary>
	/// Loads an XML object definition file
	/// </summary>
	public class XmlLoader : Resources.ResourceStreamLoader
	{

		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> Returns Engine.Main </returns>
		public override Object Load( System.IO.Stream input, string inputSource )
		{
			XmlDocument doc = new RbXmlDocument( inputSource );
			try
			{
				doc.Load( input );

				foreach ( XmlNode curNode in doc.ChildNodes )
				{
					if ( curNode.NodeType == XmlNodeType.Element )
					{
						if ( curNode.Name != "rb" )
						{
							throw new RbXmlException( curNode, "Expected root element to be named \"rb\", not \"{0}\"", curNode.Name );
						}
						else
						{
							Object result = LoadRb( ( XmlElement )curNode );
							return result;
						}
					}
				}
			}
			catch ( System.Xml.XmlException e )
			{
				string msg = String.Format( "Invalid component XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, e.LineNumber, e.LinePosition, e.Message );
				throw new System.ApplicationException( msg, e );
			}

			return null;
		}

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public override bool CanLoadStream( string path, System.IO.Stream input )
		{
			//	TODO: Bit of a bodge here to associate component files with the "xml" extension
			return path.EndsWith( ".components" ) || path.EndsWith( ".xml" );
		}

		/// <summary>
		/// Loads the root "rb" element, which can consist of any number of "modelSet" elements, and one "object" element
		/// </summary>
		/// <param name="element">XML "rb" element</param>
		/// <returns>Returns the root object, if one was defined</returns>
		private Object LoadRb( XmlElement element )
		{
			ObjectLoader	rootObject		= null;

			foreach ( XmlNode node in element.ChildNodes )
			{
				if ( node.NodeType == XmlNodeType.Element )
				{
					if ( node.Name == "modelSet" )
					{
						ModelSetLoader modelSet = new ModelSetLoader( ( XmlElement )node );
						modelSet.Resolve( null );
					}
					else if ( node.Name == "object" )
					{
						if ( rootObject != null )
						{
							throw new RbXmlException( node, "Only allowed one root object within the <rb></rb> elements" );
						}
						rootObject = new ObjectLoader( ( XmlElement )node );
					}
					else
					{
						throw new RbXmlException( node, "Did not handle \"{0}\" element: Only \"object\" and \"modelSet\" elements are allowed as direct children of <rb></rb> elements", node.Name );
					}
				}
			}

			if ( rootObject != null )
			{
				rootObject.Resolve( null );
				return rootObject.LoadedObject;
			}

			return null;
		}

		/// <summary>
		/// Base class for loading objects from an XML definition
		/// </summary>
		private class BaseLoader
		{
			/// <summary>
			/// Constructor. Sets the generating element
			/// </summary>
			protected BaseLoader( XmlElement element )
			{
				m_Element			= element;
				m_BoundPropertyName	= element.GetAttribute( "property" );
			}

			#region	Public loading and resolution

			/// <summary>
			/// Gets the object generated by the base
			/// </summary>
			public object			LoadedObject
			{
				set
				{
					m_LoadedObject = value;
				}
				get
				{
					return m_LoadedObject;
				}
			}

			/// <summary>
			/// Resolves the contents of this object
			/// </summary>
			public virtual void		Resolve( Object parentObject )
			{
				//	Parse the generating element
				IXmlLoader objectXmlLoader = LoadedObject as IXmlLoader;
				if ( objectXmlLoader != null )
				{
					objectXmlLoader.ParseGeneratingElement( Element );
				}

				//	Parse pre-link elements
				ParseElements( objectXmlLoader, m_PreLinkElements );

				//	Resolve pre-link children
				ResolveChildObjects( m_PreLinkChildren );

				//	Link the loaded object to its parent
				LinkObjectToParent( parentObject );

				//	Add post-link children
				ResolveChildObjects( m_PostLinkChildren );

				//	Parse post-link elements
				ParseElements( objectXmlLoader, m_PostLinkElements );
			}

			#endregion

			#region	Protected properties

			/// <summary>
			/// Order
			/// </summary>
			protected enum Order
			{
				Default,
				PreLink,
				PostLink,
			}

			/// <summary>
			/// XML element
			/// </summary>
			protected XmlElement	Element
			{
				get
				{
					return m_Element;
				}
			}

			/// <summary>
			/// Bound property name
			/// </summary>
			protected string		BoundPropertyName
			{
				get
				{
					return m_BoundPropertyName;
				}
			}

			/// <summary>
			/// The default order in which child objects are added
			/// </summary>
			protected virtual Order	DefaultChildOrder
			{
				get
				{
					return Order.PostLink;
				}
			}

			#endregion

			#region	Private resolution

			/// <summary>
			/// Gets the loaded object to parse a bunch of XmlElement objects
			/// </summary>
			private void			ParseElements( IXmlLoader objectXmlLoader, ArrayList elements )
			{
				//	Parse unhandled elements
				if ( elements != null )
				{
					if ( objectXmlLoader == null )
					{
						throw new RbXmlException( Element, "Could not handle elements: Object type \"{0}\" does not support IXmlLoader", LoadedObject.GetType( ).Name );
					}
					foreach ( XmlElement curElement in elements )
					{
						if ( !objectXmlLoader.ParseElement( curElement ) )
						{
							throw new RbXmlException( curElement, "Element was not handled by parent object of type \"{0}\"", LoadedObject.GetType( ).Name );
						}
					}
				}
			}

			/// <summary>
			/// Adds the loaded object(s) to the specified list
			/// </summary>
			private void			BindObjectToPropertyList( IList property )
			{
				if ( LoadedObject is IEnumerable )
				{
					foreach ( Object loadedChildObject in ( IEnumerable )LoadedObject )
					{
						property.Add( loadedChildObject );
					}
				}
				else
				{
					property.Add( LoadedObject );
				}
			}

			/// <summary>
			/// Sets the specified property to the loaded object(s)
			/// </summary>
			private void			BindObjectToProperty( Object parentObject, System.Reflection.PropertyInfo property )
			{
				ICollection loadedCollection = LoadedObject as ICollection;
				if ( loadedCollection != null )
				{
					IEnumerator enumerator = loadedCollection.GetEnumerator( );
					if ( enumerator.MoveNext( ) )
					{
						property.SetValue( parentObject, enumerator.Current, null );
					}
					if ( enumerator.MoveNext( ) )
					{
						throw new RbXmlException( Element, "Could not bind list with more than one item to property \"{1}\"", property.Name );
					}
				}
				else
				{
					property.SetValue( parentObject, LoadedObject, null );
				}

			}

			/// <summary>
			/// Binds an object to a named parent property
			/// </summary>
			private void			BindObjectToParentProperty( Object parentObject, string propertyName )
			{
				//	Can't bind a property if the parent is null
				if ( parentObject == null )
				{
					throw new RbXmlException( Element, "Can't bind root object of type \"{0}\" to property \"{1}\"",  LoadedObject.GetType( ).Name, propertyName );
				}

				//	Find information about the bound property
				System.Reflection.PropertyInfo boundProperty = parentObject.GetType( ).GetProperty( propertyName );
				if ( boundProperty == null )
				{
					throw new RbXmlException( Element, "Object of type \"{0}\" did not have a property \"{1}\" to bind object of type \"{2}\" to", parentObject.GetType( ).Name, propertyName, LoadedObject.GetType( ).Name );
				}

				//	If the bound property is a list, then add the loaded object(s) to it
				Object boundPropertyValue = boundProperty.CanRead ? boundProperty.GetValue( parentObject, null ) : null;
				if ( boundPropertyValue is IList )
				{
					BindObjectToPropertyList( ( IList )boundPropertyValue );
				}
				else
				{
					BindObjectToProperty( parentObject, boundProperty );
				}

				//	Let the object know it's been added to its parent (usually, IParentObject.AddChild() should do this, but because
				//	LoadedObject is being bound directly to a property, then let's do the parent object's work for it)
			//	if ( LoadedObject is IChildObject )
			//	{
			//		( ( IChildObject )LoadedObject ).AddedToParent( parentObject );
			//	}
				//	NOTE: Disabled in favour of an extra attribute "alwaysAddAsChild", handled in LinkObjectToParent()
			}

			/// <summary>
			/// Adds the loaded object to its parent
			/// </summary>
			private void			LinkObjectToParent( Object parentObject )
			{
				bool addToParent = ( parentObject != null );
				if ( BoundPropertyName != string.Empty )
				{
					BindObjectToParentProperty( parentObject, BoundPropertyName );
					addToParent = ( string.Compare( Element.GetAttribute( "alwaysAddAsChild" ), "true", true ) == 0 );
				}
				
				if ( addToParent )
				{
					IParentObject parentObjectInterface = parentObject as IParentObject;
					if ( parentObjectInterface == null )
					{
						throw new RbXmlException( Element, "Could not add child object of type \"{0}\": Parent object type \"{1}\" does not support IParentObject", parentObject.GetType( ).Name, LoadedObject.GetType( ).Name );
					}

					parentObjectInterface.AddChild( LoadedObject );
				}
			}
			
			/// <summary>
			/// Resolves child objects to the loaded object
			/// </summary>
			private void			ResolveChildObjects( ArrayList children )
			{
				if ( children != null )
				{
					foreach ( BaseLoader xmlBase in children )
					{
						xmlBase.Resolve( LoadedObject );
					}
				}
			}

			#endregion

			#region	Protected building

			/// <summary>
			/// Adds a child object loader to the specified child list
			/// </summary>e="order"></param>
			protected void			AddChildLoader( BaseLoader loader, Order order )
			{
				if ( order == Order.Default )
				{
					order = DefaultChildOrder;
				}
				if ( order == Order.PreLink )
				{
					AddObjectToList( ref m_PreLinkChildren, loader );
				}
				else
				{
					AddObjectToList( ref m_PostLinkChildren, loader );
				}
			}

			/// <summary>
			/// Loads child nodes from the generating element
			/// </summary>
			protected void			LoadChildNodes( XmlElement element, Order order )
			{
				//	Add child nodes
				foreach ( XmlNode curNode in element.ChildNodes )
				{
					if ( curNode.NodeType == XmlNodeType.Element )
					{
						if ( curNode.Name == "postLink" )
						{
							LoadChildNodes( ( XmlElement )curNode, Order.PostLink );
						}
						else if ( curNode.Name == "preLink" )
						{
							LoadChildNodes( ( XmlElement )curNode, Order.PreLink );
						}
						else
						{
							LoadElement( ( XmlElement )curNode, order );
						}
					}
				}
			}

			/// <summary>
			/// Adds an element, using default ordering
			/// </summary>
			protected void			LoadElement( XmlElement element, Order order )
			{
				BaseLoader loader = CreateLoaderFromElement( element );
				if ( loader != null )
				{
					AddChildLoader( loader, order );
				}
				else if ( ( order == Order.Default ) || ( order == Order.PreLink ) )
				{
					AddObjectToList( ref m_PreLinkElements, element );
				}
				else
				{
					AddObjectToList( ref m_PostLinkElements, element );
				}
			}

			#endregion

			#region	Private building

			/// <summary>
			/// Adds an object to a given list. If the list is null, it's created
			/// </summary>
			private void				AddObjectToList( ref ArrayList objectList, Object objectToAdd )
			{
				if ( objectList == null )
				{
					objectList = new ArrayList( );
				}
				objectList.Add( objectToAdd );
			}

			/// <summary>
			/// Creates an RbXmlBase instance from an XML element
			/// </summary>
			private static BaseLoader	CreateLoaderFromElement( XmlElement element )
			{
				BaseLoader loader = null;
				switch ( element.Name )
				{
					case "object"		: loader = new ObjectLoader( element );	break;
					case "reference"	: loader = new ReferenceLoader( element ); break;
					case "resource"		: loader = new ResourceStreamLoader( element );	break;
					case "instance"		: loader = new InstanceLoader( element ); break;
					case "string"		: loader = new ValueLoader( element, element.GetAttribute( "value" ) );	break;
					case "int"			: loader = new ValueLoader( element, int.Parse( element.GetAttribute( "value" ) ) );	break;
					case "float"		: loader = new ValueLoader( element, float.Parse( element.GetAttribute( "value" ) ) );	break;
					case "colour"		: loader = new ValueLoader( element, System.Drawing.Color.FromName( element.GetAttribute( "value" ) ) );	break;
					case "vector3"		:
					{
						float x = float.Parse( element.GetAttribute( "x" ) );
						float y = float.Parse( element.GetAttribute( "y" ) );
						float z = float.Parse( element.GetAttribute( "z" ) );
						loader = new ValueLoader( element, new Maths.Vector3( x, y, z ) );
						break;
					}
					case "point3"		:
					{
						float x = float.Parse( element.GetAttribute( "x" ) );
						float y = float.Parse( element.GetAttribute( "y" ) );
						float z = float.Parse( element.GetAttribute( "z" ) );
						loader = new ValueLoader( element, new Maths.Point3( x, y, z ) );
						break;
					}
				//	case "reference"	: loader =  new ReferenceLoader( element ); 
					default				: return null;
				}

				return loader;
			}

			#endregion

			#region	Private stuff

			private XmlElement		m_Element;
			private string			m_BoundPropertyName;
			private Object			m_LoadedObject;
			private ArrayList		m_PreLinkChildren;
			private ArrayList		m_PreLinkElements;
			private ArrayList		m_PostLinkChildren;
			private ArrayList		m_PostLinkElements;

			#endregion
		}

		/// <summary>
		/// Extends BaseLoader to load ModelSet objects
		/// </summary>
		private class ModelSetLoader : BaseLoader
		{
			/// <summary>
			/// Constructor. Creates the model set
			/// </summary>
			public ModelSetLoader( XmlElement element ) :
				base( element )
			{
				//	Find an existing modelset, or creates a new model set
				m_ModelSetName = element.GetAttribute( "name" );

				foreach ( XmlNode curNode in element.ChildNodes )
				{
					if ( curNode.NodeType == XmlNodeType.Element )
					{
						if ( curNode.Name == "modelSet" )
						{
							AddChildLoader( new ModelSetLoader( ( XmlElement )curNode ), Order.Default );
						}
						else
						{
							LoadElement( ( XmlElement )curNode, Order.Default );
						}
					}
				}
			}
			
			/// <summary>
			/// Model sets get added to their parents, before their children get added to them
			/// </summary>
			protected override Order	DefaultChildOrder
			{
				get
				{
					return Order.PreLink;
				}
			}

			/// <summary>
			/// Resolves the model set and all the objects stored in it
			/// </summary>
			/// <param name="parentObject">Parent object</param>
			public override void	Resolve( Object parentObject )
			{
				//	Root model set - may exist already
				ModelSet modelSet = null;
				if ( parentObject == null )
				{
					modelSet = ModelSet.FindModelSet( m_ModelSetName, false );
				}
				else
				{
					modelSet = ( ( ModelSet )parentObject ).FindChildModelSet( m_ModelSetName );
				}

				//	If the model set did not already exist, create a new one
				if ( modelSet == null )
				{
					modelSet = new ModelSet( m_ModelSetName );
					if ( parentObject == null )
					{
						parentObject = ModelSet.Main;
					}
				}

				LoadedObject = modelSet;

				base.Resolve( parentObject );
			}

			string m_ModelSetName;
		}

		/// <summary>
		/// Extends BaseLoader to load a reference to an existing object
		/// </summary>
		private class ReferenceLoader : BaseLoader
		{
			/// <summary>
			/// Constructor. Creates the object
			/// </summary>
			/// <param name="element"></param>
			public ReferenceLoader( XmlElement element ) :
				base( element )
			{
			}

			/// <summary>
			/// Resolves the reference
			/// </summary>
			public override void Resolve( Object parentObject )
			{
				string modelPath = Element.GetAttribute( "modelPath" );
				if ( modelPath != string.Empty )
				{
					try
					{
						LoadedObject = ModelSet.Find( modelPath );
					}
					catch ( Exception exception )
					{
						throw new RbXmlException( exception, Element, "Failed to resolve reference to model \"{0}\"", modelPath );
					}
				}
				else
				{
					throw new RbXmlException( Element, "Only \"modelPath\" references are supported at the moment, sorry" );
				}

				base.Resolve( parentObject );
			}

		}

		/// <summary>
		/// Extends BaseLoader to load a given object type
		/// </summary>
		private class ObjectLoader : BaseLoader
		{
			/// <summary>
			/// Constructor. Creates the object
			/// </summary>
			public ObjectLoader( XmlElement element ) :
				base( element )
			{
				//	Create the new object
				//	TODO: Add assembly attribute?
				string newObjectTypeName = element.GetAttribute( "type" );
				LoadedObject = AppDomainUtils.CreateInstance( newObjectTypeName );

				if ( LoadedObject == null )
				{
					throw new RbXmlException( element, "Could not create instance of object \"{0}\"", newObjectTypeName );
				}

				//	Set the name of the object
				string newObjectName = element.GetAttribute( "name" );
				if ( newObjectName != string.Empty )
				{
					if ( !( LoadedObject is Components.INamedObject ) )
					{
						throw new RbXmlException( element, "Could not name object of type \"{0}\" - type does not implement the INamedObject interface", newObjectTypeName );
					}
					( ( Components.INamedObject )LoadedObject ).Name = newObjectName;
				}

				LoadChildNodes( element, Order.Default );
			}
		}

		/// <summary>
		/// Loads a particular value
		/// </summary>
		private class ValueLoader : BaseLoader
		{
			/// <summary>
			/// Constructor
			/// </summary>
			public ValueLoader( XmlElement element, Object val ) :
				base( element )
			{
				LoadedObject = val;
			}
		}

		/// <summary>
		/// Extends BaseLoader to create instances
		/// </summary>
		private class InstanceLoader : BaseLoader
		{
			/// <summary>
			/// Constructor. Instances the reference
			/// </summary>
			/// <param name="element"></param>
			public InstanceLoader( XmlElement element ) :
				base( element )
			{
			}

			/// <summary>
			/// Creates and resolves the instance
			/// </summary>
			/// <param name="parentObject"></param>
			public override void Resolve( Object parentObject )
			{
				//	Find the object to instance
				string modelPath = Element.GetAttribute( "modelPath" );
				if ( modelPath != string.Empty )
				{
					try
					{
						LoadedObject = ModelSet.Find( modelPath );
					}
					catch ( Exception exception )
					{
						throw new RbXmlException( exception, Element, "Failed to resolve reference to model \"{0}\"", modelPath );
					}
				}
				else
				{
					throw new RbXmlException( Element, "Only \"modelPath\" instances are supported at the moment, sorry" );
				}

				//	Instance the object
				IInstanceBuilder instanceBuilder = LoadedObject as IInstanceBuilder;
				if ( instanceBuilder == null )
				{
					//	No instance builder available - just keep a reference, but write a warning also
					string	source	= ( ( RbXmlDocument )Element.OwnerDocument ).InputSource;
					int		line	= ( ( IXmlLineInfo )Element ).LineNumber;
					int		column	= ( ( IXmlLineInfo )Element ).LinePosition;
					Output.WriteLineCall( Output.ResourceWarning, "<instance> tag specified an object (\"{0}\") that did not implement IInstanceBuilder - storing as reference\n{1}({2},{3}) : [{4}]Warning occurred here", modelPath, source, line, column, Output.ResourceWarning.DisplayName );
				}
				else
				{
					LoadedObject = instanceBuilder.CreateInstance( );
				}

				base.Resolve( parentObject );
			}

		}

		/// <summary>
		/// Extends BaseLoader to load a given resource type
		/// </summary>
		private class ResourceStreamLoader : BaseLoader
		{
			/// <summary>
			/// Constructor. Loads the resource
			/// </summary>
			public ResourceStreamLoader( XmlElement element ) :
				base( element )
			{
				//	Create the new resource
				string resourcePath = element.GetAttribute( "path" );
				if ( resourcePath == string.Empty )
				{
					throw new RbXmlException( element, "<resource> element did not have a \"path\" attribute" );
				}

				//	TODO: Need to rescue bad resource paths (requires change to resource manager)
				object loadResult = Resources.ResourceManager.Inst.Load( resourcePath );
				LoadedObject = loadResult;

				//	Set the name of the resource
				string newResourceName = element.GetAttribute( "name" );
				if ( newResourceName != string.Empty )
				{
					if ( !( loadResult is Components.INamedObject ) )
					{
						string resourceStr = String.Format( "\"{0}\"(\"{1}\"", resourcePath, loadResult.GetType( ).Name );
						throw new RbXmlException( element, "Could not name resource {0} - type does not implement the INamedObject interface", resourceStr );
					}
					( ( Components.INamedObject )loadResult ).Name = newResourceName;
				}

				LoadChildNodes( element, Order.Default );
			}
		}
	}
}
