using System;
using System.Xml;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for EngineXmlLoader.
	/// </summary>
	public class EngineXmlLoader : Resources.ResourceLoader
	{
		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> Returns Engine.Main </returns>
		public override Object Load( System.IO.Stream input, string inputSource )
		{
			XmlTextReader reader = new System.Xml.XmlTextReader( input );

			try
			{
				while ( reader.Read( ) )
				{
					switch ( reader.NodeType )
					{
						case XmlNodeType.Element		:
						{
							//	Root element must be called "engine"
							if ( String.Compare( reader.Name, "engine", true ) != 0 )
							{
								throw new System.ApplicationException( String.Format( "Expected root element to be named \"engine\", not \"{0}\"", reader.Name ) );
							}

							LoadEngine( reader );
							break;
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

			return Engine.Main;
		}

		private void LoadEngine( XmlReader reader )
		{
			while ( reader.Read( ) )
			{
				if ( reader.NodeType == XmlNodeType.Element )
				{
					if ( String.Compare( reader.Name, "modelSet" ) == 0 )
					{
						LoadModelSet( reader, ModelSet.Main );
					}
					else
					{
						LoadElement( reader, Engine.Main );
					}
				}
			}
		}

		/// <summary>
		/// Loads an XML element
		/// </summary>
		/// <param name="reader"> XML reader, pointing at an element </param>
		/// <param name="parentObject"> The parent object </param>
		/// <exception> Throws an exception if the element did not define a valid object </exception>
		private void LoadElement( XmlReader reader, Object parentObject )
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
			string name = reader.GetAttribute( "name" );

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
