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
						LoadElement( reader );
					}
				}
			}
		}

		/// <summary>
		/// Loads an XML element
		/// </summary>
		/// <param name="reader"> XML reader, pointing at an element </param>
		/// <returns> Returns the freshly loaded element </returns>
		/// <exception> Throws an exception if the element did not define a valid object </exception>
		private Object LoadElement( XmlReader reader )
		{
			Object newObject = null;
			switch ( reader.Name )
			{
				case "object"	: newObject = LoadObject( reader );		break;
				case "instance"	: newObject = LoadInstance( reader );	break;
				case "resource"	: newObject = LoadResource( reader );	break;
				default			:
					throw new System.ApplicationException( String.Format( "Element \"{0}\" could not be declared here", reader.Name ) );
			}

			return newObject;
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
			Object obj = GetType( ).Assembly.CreateInstance( newObjectTypeName );

			if ( obj == null )
			{
				throw new System.ApplicationException( String.Format( "Could not create instance of object \"{0}\"", newObjectTypeName ) );
			}

			//	Set the name of the object
			string newObjectName = reader.GetAttribute( "name" );
			if ( newObjectName != null )
			{
				if ( !( obj is Utilities.INamedObject ) )
				{
					throw new System.ApplicationException( String.Format( "Could not name object of type \"{0}\" - type does not implement the INamedObject interface", newObjectTypeName ) );
				}
				( ( Utilities.INamedObject )obj ).Name = newObjectName;
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
						Object childObj = LoadElement( reader );

						if ( !( obj is Utilities.IParentObject ) )
						{
							throw new System.ApplicationException( String.Format( "Can't add objects to object of type \"{0}\" - did not implement the IParentObject interface", newObjectTypeName ) );
						}

						( ( Utilities.IParentObject )obj ).AddChild( childObj );
						break;
					}
				}
			}

			return obj;
		}

		private Object LoadInstance( XmlReader reader )
		{
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
						LoadElement( reader );
						break;
					}
				}
			}
			return null;
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
							curModelSet.AddChild( LoadElement( reader ) );
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
				if ( !( newResource is Utilities.INamedObject ) )
				{
					string resourceStr = String.Format( "\"{0}\"(\"{1}\"", resourcePath, newResource.GetType( ).Name );
					throw new System.ApplicationException( String.Format( "Could not name resource {0} - type does not implement the INamedObject interface", resourceStr ) );
				}
				( ( Utilities.INamedObject )newResource ).Name = newResourceName;
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
						Object childObj = LoadElement( reader );

						if ( !( newResource is Utilities.IParentObject ) )
						{
							string resourceStr = String.Format( "\"{0}\"(\"{1}\"", resourcePath, newResource.GetType( ).Name );
							throw new System.ApplicationException( String.Format( "Can't add objects to resource {0} - did not implement the IParentObject interface", resourceStr ) );
						}

						( ( Utilities.IParentObject )newResource ).AddChild( childObj );
						break;
					}
				}
			}

			return newResource;
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
