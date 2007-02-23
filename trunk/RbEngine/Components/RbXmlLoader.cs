using System;
using System.Xml;
using System.Collections;

namespace RbEngine.Components
{

	public class RbXmlBase
	{
		public virtual void	Resolve( Object parentObject )
		{
		}
	}

	public class RbXmlObject
	{
		public RbXmlObject( XmlElement element )
		{
			m_Element = element;
		}

		public void Add( RbXmlObject obj )
		{
			if ( m_ObjectAsParent == null )
			{
				throw new RbXmlException( m_Element, "Can't add child objects: Object type (\"{0}\") does not implement IParentObject", m_Object.GetType( ).Name );
			}
			m_ObjectAsParent.AddChild( obj );
		}

		public void Add( RbXmlBase obj )
		{
			if ( m_Resolve == null )
			{
				m_Resolve = new ArrayList( );
			}
			m_Resolve.Add( obj );
		}

		public void Add( XmlElement element )
		{
			if ( m_Elements == null )
			{
				m_Elements = new ArrayList( );
			}
			m_Elements.Add( element );
		}

		public void Resolve( Object parentObject )
		{
			//	Resolve all references and so forth
			IXmlLoader objectXmlLoader = m_Object as IXmlLoader;
			if ( objectXmlLoader != null )
			{
				objectXmlLoader.ParseGeneratingElement( m_Element );

				if ( m_Elements != null )
				{
					foreach ( XmlElement element in m_Elements )
					{
						objectXmlLoader.ParseElement( ( XmlElement )element );
					}
				}
			}
			else if ( m_Elements != null )
			{
				throw new RbXmlException( m_Element, "Can't handle unknown elements: Object's type (\"{0}\") does not implement IXmlLoader", m_Object.GetType( ).Name );
			}

			
		}

		private XmlElement		m_Element;
		private Object			m_Object;
		private IParentObject	m_ObjectAsParent;
		private ArrayList		m_Resolve;
		private ArrayList		m_Elements;
	}

	public class RbXmlProperty : RbXmlBase
	{
	}

	public class RbXmlReference : RbXmlBase
	{
		public override void Resolve( Object parentObject )
		{
		}
	}

	/// <summary>
	/// Loads an XML object definition file
	/// </summary>
	public class RbXmlLoader : Resources.ResourceLoader
	{
		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> Returns Engine.Main </returns>
		public override Object Load( System.IO.Stream input, string inputSource )
		{
			XmlDocument doc = new RbXmlDocument( );
			try
			{
				doc.Load( input );

				foreach ( XmlNode curNode in doc.ChildNodes )
				{
					if ( curNode is XmlElement )
					{
						if ( curNode.Name != "rb" )
						{
							throw new RbXmlException( curNode, "Expected root element to be named \"rb\", not \"{0}\"", curNode.Name );
						}
						else
						{
						}
					}
				}
			}
			catch ( System.Xml.XmlException e )
			{
				string msg = String.Format( "Invalid component XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, e.LineNumber, e.LinePosition, e.Message );
				throw new System.ApplicationException( msg, e );
			}


			/*
			XmlTextReader reader = new System.Xml.XmlTextReader( input );

			try
			{
				while ( reader.Read( ) )
				{
					switch ( reader.NodeType )
					{
						case XmlNodeType.Element		:
						{
							//	Root element must be called "rb"
							if ( String.Compare( reader.Name, "rb", true ) != 0 )
							{
								throw new System.ApplicationException( String.Format( "Expected root element to be named \"rb\", not \"{0}\"", reader.Name ) );
							}

							return LoadRb( reader );
						}
					}
				}
			}
			catch ( System.Xml.XmlException e )
			{
				string msg = String.Format( "Invalid component XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, e.LineNumber, e.LinePosition, e.Message );
				throw new System.ApplicationException( msg, e );
			}
			catch ( System.Exception e )
			{
				string msg = String.Format( "Failed to read component XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, reader.LineNumber, reader.LinePosition, e.Message );
				throw new System.ApplicationException( msg, e );
			}
			*/

			return null;
		}

		/// <summary>
		/// Loads the root "rb" element, which can consist of any number of "modelSet" elements, and one "object" element
		/// </summary>
		/// <param name="reader">XML reader, pointing at the "rb" element</param>
		/// <returns>Returns the root object, if one was defined</returns>
		private Object LoadRb( XmlReader reader )
		{
			Object result = null;
			while ( reader.Read( ) )
			{
				if ( reader.NodeType == XmlNodeType.Element )
				{
					if ( reader.Name == "modelSet" )
					{
						LoadModelSet( reader, ModelSet.Main );
					}
					else if ( reader.Name == "object" )
					{
						if ( result != null )
						{
							throw new System.ApplicationException( "Only allowed one root object within the <rb></rb> elements" );
						}
						result = LoadObject( reader );
					}
					else
					{
						throw new System.ApplicationException( "Only \"object\" and \"modelSet\" elements are allowed as direct children of <rb></rb> elements" );
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Loads an XML element
		/// </summary>
		/// <param name="reader"> XML reader, pointing at an element </param>
		/// <param name="parentObject"> The parent object </param>
		/// <exception> Throws an exception if the element did not define a valid object </exception>
		private Object LoadElement( XmlReader reader, Object parentObject )
		{
			string boundPropertyName = reader.GetAttribute( "property" );
			Object childObject = null;
			IXmlLoader parentXmlLoader = parentObject as IXmlLoader;
			switch ( reader.Name )
			{
				case "object"	: childObject = LoadObject( reader );		break;
				case "instance"	: childObject = LoadInstance( reader );		break;
				case "resource"	: childObject = LoadResource( reader );		break;
				case "reference": childObject = LoadReference( reader );	break;
				default			:
					if ( parentXmlLoader == null )
					{
						throw new ApplicationException( String.Format( "Unable to handle element \"{0}\" - parent object type (\"{1}\") did not implement IXmlLoader", reader.Name, parentObject.GetType( ).Name ) );
					}
					parentXmlLoader.ParseElement( reader );
					break;
			}
			
			if ( childObject != null )
			{
				if ( boundPropertyName != null )
				{
					System.Reflection.PropertyInfo boundProperty = parentObject.GetType( ).GetProperty( boundPropertyName );
					if ( boundProperty == null )
					{
						throw new ApplicationException( String.Format( "Object of type \"{0}\" did not have a property \"{1}\" to bind to", parentObject.GetType( ).Name, boundPropertyName ) );
					}
					boundProperty.SetValue( parentObject, childObject, null );
				}
				else
				{
					if ( !( parentObject is Components.IParentObject ) )
					{
						throw new ApplicationException( String.Format( "Can't add objects to object of type \"{0}\" - did not implement the IParentObject interface", parentObject.GetType( ).Name ) );
					}
					( ( Components.IParentObject )parentObject ).AddChild( childObject );
				}
			}

			return childObject;
		}

		/// <summary>
		/// Loads an object from the 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Object LoadObject( XmlReader reader )
		{
			//	Create the new object
			//	TODO: Add assembly attribute?
			string newObjectTypeName = reader.GetAttribute( "type" );
			Object obj = AppDomainUtils.CreateInstance( newObjectTypeName );

			if ( obj == null )
			{
				throw new System.ApplicationException( String.Format( "Could not create instance of object \"{0}\"", newObjectTypeName ) );
			}

			if ( obj is IXmlLoader )
			{
				( ( IXmlLoader )obj ).ParseGeneratingElement( reader );
			}

			//	Set the name of the object
			string newObjectName = reader.GetAttribute( "name" );
			if ( newObjectName != null )
			{
				if ( !( obj is Components.INamedObject ) )
				{
					throw new System.ApplicationException( String.Format( "Could not name object of type \"{0}\" - type does not implement the INamedObject interface", newObjectTypeName ) );
				}
				( ( Components.INamedObject )obj ).Name = newObjectName;
			}

			//	Nothing more to do here...
			if ( reader.IsEmptyElement )
			{
				return obj;
			}

			//	Iterate through the child elements, adding child objects
			//	obj must implement the IParentObject interface
			while ( reader.Read( ) )
			{
				switch ( reader.NodeType )
				{
					case XmlNodeType.EndElement :
					{
						return obj;
					}
					case XmlNodeType.Element :
					{
						LoadElement( reader, obj );
						break;
					}
				}
			}

			return obj;
		}

		private Object LoadInstance( XmlReader reader )
		{
			string path = reader.GetAttribute( "path" );
			Object instancerObject = ModelSet.Find( path );

			if ( instancerObject == null )
			{
				throw new ApplicationException( String.Format( "Unable to find object \"{0}\" in model set", path ) );
			}
			IInstanceable instancer = instancerObject as IInstanceable;
			if ( instancer == null )
			{
				throw new ApplicationException( String.Format( "Modelset object of type \"{0}\" (path \"{1}\") cannot be used as an instancer because it does not implement IInstanceable", instancerObject.GetType( ).Name, path ) );
			}
			Object instance = instancer.CreateInstance( );

			while ( reader.Read( ) )
			{
				switch ( reader.NodeType )
				{
					case XmlNodeType.EndElement :
					{
						return null;
					}
					case XmlNodeType.Element :
					{
						LoadElement( reader, instance );
						break;
					}
				}
			}

			return instance;
		}

		private ModelSet LoadModelSet( XmlReader reader, ModelSet curModelSet )
		{
			//	Find an existing modelset, or create a new model set
			string modelSetName = reader.GetAttribute( "name" );
			ModelSet newModelSet = ModelSet.FindModelSet( modelSetName, false );

			if ( newModelSet == null )
			{
				newModelSet = new ModelSet( modelSetName );
				curModelSet.AddChild( newModelSet );
			}

			while ( reader.Read( ) )
			{
				switch ( reader.NodeType )
				{
					case XmlNodeType.EndElement :
					{
						return newModelSet;
					}
					case XmlNodeType.Element :
					{
						if ( String.Compare( reader.Name, "modelSet" ) == 0 )
						{
							LoadModelSet( reader, newModelSet );
						}
						else
						{
							LoadElement( reader, curModelSet );
						}
						break;
					}
				}
			}
			return newModelSet;
		}

		private Object LoadResource( XmlReader reader )
		{
			//	Create the new resource
			string resourcePath = reader.GetAttribute( "path" );

			Object newResource = Resources.ResourceManager.Inst.Load( resourcePath );

			//	Set the name of the resource
			string newResourceName = reader.GetAttribute( "name" );
			if ( newResourceName != null )
			{
				if ( !( newResource is Components.INamedObject ) )
				{
					string resourceStr = String.Format( "\"{0}\"(\"{1}\"", resourcePath, newResource.GetType( ).Name );
					throw new System.ApplicationException( String.Format( "Could not name resource {0} - type does not implement the INamedObject interface", resourceStr ) );
				}
				( ( Components.INamedObject )newResource ).Name = newResourceName;
			}

			//	Nothing more to do here...
			if ( reader.IsEmptyElement )
			{
				return newResource;
			}

			//	Iterate through the child elements, adding child objects
			//	obj must implement the IParentObject interface
			while ( reader.Read( ) )
			{
				switch ( reader.NodeType )
				{
					case XmlNodeType.EndElement :
					{
						return newResource;
					}
					case XmlNodeType.Element :
					{
						LoadElement( reader, newResource );
						break;
					}
				}
			}

			return newResource;
		}

		private Object	LoadReference( XmlReader reader )
		{
			string		name = reader.GetAttribute( "name" );
			string[]	path = name.Split( '.' );

			throw new System.ApplicationException( String.Format( "<reference> tag to \"{0}\": References aren't supported yet, sorry", name ) );
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

	}
}
