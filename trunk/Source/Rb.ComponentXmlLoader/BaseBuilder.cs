using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

using Rb.Core.Maths;
using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Base builder
    /// </summary>
    internal class BaseBuilder
    {
        #region Factory

		/// <summary>
		/// Creates a Color object from attributes stored in an xml element
		/// </summary>
		private static System.Drawing.Color MakeColour( XmlReader reader )
		{
			string value = reader.GetAttribute( "value" );
			if ( value != null )
			{
				return System.Drawing.Color.FromName( value );
			}

			string rStr = reader.GetAttribute( "r" );
			string bStr = reader.GetAttribute( "g" );
			string gStr = reader.GetAttribute( "b" );
			string aStr = reader.GetAttribute( "a" );

			if ( rStr == null || bStr == null || gStr == null )
			{
				throw new ApplicationException( string.Format( "<{0}> tags need either a value attribute, or \"r\", \"g\" and \"b\" attributes", reader.Name ) );
			}

			int r	= int.Parse( rStr );
			int g	= int.Parse( gStr );
			int b	= int.Parse( bStr );
			int a	= ( aStr == null ) ? 255 : int.Parse( aStr );

			return System.Drawing.Color.FromArgb( a, r, g, b );
		}

        /// <summary>
        /// Creates a BaseBuilder-derived object from a name
        /// </summary>
        /// <param name="parentBuilder">Parent builder</param>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader</param>
        /// <returns>Returns the new builder, or null if there as an error</returns>
        public static BaseBuilder CreateBuilderFromReader( BaseBuilder parentBuilder, ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader )
        {
            BaseBuilder result = null;
            try
            {
                switch ( reader.Name )
                {
                    case "rb"       : result = new RootBuilder( parameters, errors, reader );							break;
                    case "object"   : result = new ObjectBuilder( parameters, errors, reader, parentBuilder );			break;
                    case "ref"      : result = new ReferenceBuilder( parameters, errors, reader, parentBuilder );		break;
                    case "resource" : result = new ResourceBuilder( parameters, errors, reader, parentBuilder );		break;
                    case "instance" : result = new InstanceBuilder( parameters, errors, reader, parentBuilder );		break;
					case "method"	: result = new MethodBuilder( parameters, errors, reader, parentBuilder );			break;
                    case "list"     : result = new ListBuilder( parameters, errors, reader, parentBuilder );			break;
					case "table"	: result = new DictionaryBuilder( parameters, errors, reader, parentBuilder );		break;
                    case "type"     : result = new TypeBuilder( parameters, errors, reader, parentBuilder );			break;
					case "template"	: result = new TemplateBuilder( parameters, errors, reader, parentBuilder );		break;
					case "dictionaryEntry"	:
						result = new DictionaryEntryBuilder( parameters, errors, reader, parentBuilder );
                		break;
					case "dynProperty"	:
						object dynPropertyValue = parameters.Properties[ reader.GetAttribute( "value" ) ];
						result = new ValueBuilder( parameters, errors, reader, parentBuilder, dynPropertyValue );
                		break;
					case "colour"		:
						result = new ValueBuilder( parameters, errors, reader, parentBuilder, MakeColour( reader ) );
						break;
                    case "string"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, reader.GetAttribute( "value" ) );
                        break;
                    case "guid"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Guid( reader.GetAttribute( "value" ) ) );
                        break;
                    case "bool"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, bool.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "char"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, char.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "byte"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, byte.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "sbyte"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, sbyte.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "short"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, short.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "ushort"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, ushort.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "int"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, int.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "uint"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, uint.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "long"			:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, long.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "ulong"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, ulong.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "float"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, float.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "double"		:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, double.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "point3"		:
                        {
                            float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    float z = float.Parse( reader.GetAttribute( "z" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Point3( x, y, z ) );
						    break;
                        }
					case "bigPoint3"	:
                		{
                            long x = long.Parse( reader.GetAttribute( "x" ) );
						    long y = long.Parse( reader.GetAttribute( "y" ) );
						    long z = long.Parse( reader.GetAttribute( "z" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new BigPoint3( x, y, z ) );
						    break;	
                		}
					case "vector2"		:
                        {
						    float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Vector2( x, y ) );
						    break;
                        }
					case "vector3"		:
                        {
						    float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    float z = float.Parse( reader.GetAttribute( "z" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Vector3( x, y, z ) );
						    break;
                        }
                    case "point2"   	:
                    case "quat"     	:
                        {
                            errors.Add( reader, "Element is not yet supported", reader.Name );
                            reader.Skip( );
                            break;
                        }
                    default	:
                        {
							//	Interpret name as object type
                        	string typeName = reader.Name;
							result = new ObjectBuilder( parameters, errors, reader, parentBuilder, typeName );
                            break;
                        }
                }
            }
            catch ( Exception ex )
            {
                errors.Add( reader, ex, "Builder created from element \"{0}\" threw an exception", reader.Name );
                reader.Skip( );
            }

            if ( result != null )
            {
                string name = reader.Name;
                try
                {
                    result.ReadChildBuilders( reader );
                }
                catch ( Exception ex )
                {
                    errors.Add( reader, ex, "Exception thrown while reading children from builder \"{0}\"", name );
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="errors">Error collection</param>
        /// <param name="reader">XML reader positioned at the element that created this builder</param>
        /// <param name="parentBuilder">Parent builder</param>
        public BaseBuilder( ComponentLoadParameters parameters, ErrorCollection errors, XmlReader reader, BaseBuilder parentBuilder )
        {
            IXmlLineInfo lineInfo = reader as IXmlLineInfo;
            if ( lineInfo != null )
            {
                m_Line      = lineInfo.LineNumber;
                m_Column    = lineInfo.LinePosition;
            }

            m_ParentBuilder = parentBuilder;
            m_Errors        = errors;
            m_Parameters    = parameters;
            m_Property      = reader.GetAttribute( "property" );
            m_DynProperty   = reader.GetAttribute( "dynProperty" );
            m_Name          = reader.GetAttribute( "name" );
            m_Id            = reader.GetAttribute( "id" );
        }

        /// <summary>
        /// Creates child builder objects
        /// </summary>
        public void ReadChildBuilders( XmlReader reader )
        {
            HandleChildElements( reader, GetBuilders( LinkStep.Default ) );
        }

        /// <summary>
        /// Called before resolution and after creation
        /// </summary>
        public virtual void PostCreate( )
        {
            PostCreateChildBuilders( m_PreLinkBuilders );
            PostCreateChildBuilders( m_PostLinkBuilders );
        }

        /// <summary>
        /// Resolves this builder
        /// </summary>
        public virtual void Resolve( bool linkThisBuilder )
        {
            //  Resolve pre-link objects
            ResolveChildBuilders( m_PreLinkBuilders, true );

            //  Link built object to its parent
            if ( ( ParentBuilder != null ) && ( linkThisBuilder ) && ( BuildObject != null ) )
            {
				ParentBuilder.OnLink( BuildObject );
				Link( ParentBuilder.BuildObject );
            }

            //  Resolve post-link objects
            ResolveChildBuilders( m_PostLinkBuilders, true );
        }

        /// <summary>
        /// Sets and gets the object created by this builder
        /// </summary>
        public object BuildObject
        {
            get { return m_Object; }
            set
            {
                m_Object = value;

                //  TODO: AP: Move naming and identification to specific builders? (i.e. ObjectBuilder only)
                if ( ( m_Object != null ) && ( AllowIdentification ) )
                {
                    if ( m_Name != null )
                    {
                        INamed namedObject = m_Object as INamed;
                        if ( namedObject != null )
                        {
                            namedObject.Name = m_Name;
						}
						else
						{
							throw new ApplicationException( string.Format( "Can't set name: Object of type \"{0}\" does not implement INamed", m_Object.GetType( ) ) );
						}
                    }
                    if ( m_Id != null )
                    {
                        IUnique uniqueObject = m_Object as IUnique;
                        Guid guid = new Guid( m_Id );
                        if ( uniqueObject != null )
                        {
                            uniqueObject.Id = guid;
                        }
						else
                        {
                        	//throw new ApplicationException( string.Format( "Can't set ID: Object of type \"{0}\" does not implement INamed", m_Object.GetType( ) ) );
                        }
                        Parameters.Objects[ guid ] = m_Object;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the parent builder
        /// </summary>
        public BaseBuilder ParentBuilder
        {
            get { return m_ParentBuilder;  }
        }

        /// <summary>
        /// Returns true if this is the root builder
        /// </summary>
        public bool IsRoot
        {
            get { return m_ParentBuilder == null; }
        }

        #region Safe methods

        /// <summary>
        /// Calls BaseBuilder.PostCreate() on the specified builder object
        /// </summary>
        public static void SafePostCreate( BaseBuilder builder )
        {
            try
            {
                builder.PostCreate( );
            }
            catch ( Exception ex )
            {
                builder.Errors.Add( builder, ex, "Builder threw an exception during post-creation phase" );
            }
        }

        /// <summary>
        /// Calls BaseBuilder.Resolve() on the specified builder object
        /// </summary>
        public static void SafeResolve( BaseBuilder builder, bool linkBuilder )
        {
            try
            {
				builder.Resolve( linkBuilder );
            }
            catch ( Exception ex )
            {
                builder.Errors.Add( builder, ex, "Builder threw an exception during resolution phase" );
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The line of the element that created this builder
        /// </summary>
        public int Line
        {
            get { return m_Line; }
        }

        /// <summary>
        /// The column of the element that created this builder
        /// </summary>
        public int Column
        {
            get { return m_Column; }
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the error collection
        /// </summary>
        protected ErrorCollection Errors
        {
            get { return m_Errors; }
        }

        /// <summary>
        /// Gets the load parameters
        /// </summary>
        protected ComponentLoadParameters Parameters
        {
            get { return m_Parameters; }
        }

        /// <summary>
        /// Gets the default link step for children of this builder
        /// </summary>
        protected virtual LinkStep DefaultLinkStep
        {
            get { return LinkStep.PostLink; }
        }

        /// <summary>
        /// Returns true if the built object can be identified by name or id
        /// </summary>
        protected virtual bool AllowIdentification
        {
            get { return true; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Link step types
        /// </summary>
        protected enum LinkStep
        {
            Default,
            PreLink,
            PostLink
        }

        /// <summary>
        /// Links the BuildObject to the property of a parent
        /// </summary>
        /// <param name="parent">Parent object</param>
        /// <param name="property">Property information</param>
        protected virtual void LinkToProperty( object parent, PropertyInfo property )
        {
        	if ( !property.CanRead )
        	{
        		LinkToSetProperty( parent, property );
        	}
        	else
        	{
        		object propertyObject = property.GetValue( parent, null );
        		if ( propertyObject is IList )
        		{
        			LinkToList( ( IList )propertyObject, BuildObject );
        		}
        		else
        		{
        			LinkToSetProperty( parent, property );
        		}
        	}
        }

		/// <summary>
		/// Called prior to the specified child object getting added to this builder's object
		/// </summary>
		protected virtual void OnLink( object child )
		{
		}

        /// <summary>
        /// Sets a property of a parent to the BuildObject. Calls IChild.OnAdded() if BuildObject implements that interface
        /// </summary>
        /// <param name="parent">Parent object</param>
        /// <param name="property">Property information</param>
        protected virtual void LinkToSetProperty( object parent, PropertyInfo property )
        {
        	property.SetValue( parent, BuildObject, null );
        	if ( BuildObject is IChild )
        	{
        		( ( IChild )BuildObject ).AddedToParent( parent );
        	}
        }

        /// <summary>
        /// Adds the BuildObject to an IParent object
        /// </summary>
        /// <param name="parent">Parent object</param>
        /// <param name="child">Child object</param>
        protected static void LinkToParent( IParent parent, object child )
        {
			parent.AddChild( child );
			//	NOTE: It's the responsibility of IParent.AddChild() to call IChild.AddedToParent()
        }

        /// <summary>
        /// Adds the BuildObject to an IList object. Calls IChild.OnAdded() if BuildObject implements that interface
        /// </summary>
		/// <param name="parent">Parent object</param>
		/// <param name="child">Child object</param>
        protected static void LinkToList( IList parent, object child )
        {
        	parent.Add( child );
        	if ( child is IChild )
        	{
        		( ( IChild )child ).AddedToParent( parent );
        	}
        }

		/// <summary>
		/// Adds the BuildObject to an IDictionary parent. The BuildObject must be a DictionaryEntry. Calls <see cref="IChild.AddedToParent"/>
		/// if the DictionaryEntry Value implements IChild, 
		/// </summary>
		/// <param name="parent">Parent object</param>
		/// <param name="child">Child object</param>
		protected static void LinkToDictionary( IDictionary parent, object child )
		{
			if ( !( child is DictionaryEntry ) )
			{
				throw new ApplicationException( "Can only add DictionaryEntry objects to dictionary parents" );
			}
			DictionaryEntry entry = ( DictionaryEntry )child;
			parent.Add( entry.Key, entry.Value );

			if ( entry.Value is IChild )
			{
				( ( IChild )entry.Value ).AddedToParent( parent );
			}
		}

		/// <summary>
		/// Links an object to a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		/// <param name="child">Child object</param>
		protected static void LinkObjectToParent( object parent, object child )
		{
            IParent componentParent = parent as IParent;
            if ( componentParent != null )
            {
                LinkToParent( componentParent, child );
                return;
            }
            
            IList listParent = parent as IList;
            if ( listParent != null )
            {
            	LinkToList( listParent, child );
                return;
            }

        	IDictionary dictionaryParent = parent as IDictionary;
			if ( dictionaryParent != null )
			{
				LinkToDictionary( dictionaryParent, child );
				return;
			}

			//	TODO: AP: Reinstate throw?
			//	Removed because scene does not, and cannot, support IParent. This is because objects must be associated with IDs, and
			//	IParent does not support id/object pairs (could add IUnique objects, but not generic object/id pairs).
			//	Even if scene.Add(object) throws if the object is not IUnique, then the object is being added to the scene twice
			//	(once via the the Add() call, once via the loader object ID map (which is the scene object ID map).
			//throw new ArgumentException( string.Format( "Can't add object of type \"{0}\" to object of type \"{1}\" (parent does not implement IParent, IList or IDictionary)", child.GetType( ), parent.GetType( ) ) );
		}

        /// <summary>
        /// Links the built object to the specified parent
        /// </summary>
        /// <param name="parent">Parent object to link BuildObject to</param>
        protected void Link( object parent )
        {
        	//	Link rules
        	//
        	//	If linking to a property:
        	//		If the property can be set only
        	//			Set the property to the build object
        	//		Else
        	//			Get the property
        	//			If the property is a list
        	//				Add the build object to the list
        	//			else
        	//				Set the property to the build object
        	//	If the build object is an IChild
        	//		call IChild.OnAddedToParent()
        	//
        	//	If parent is an IParent
        	//		call IParent.Add()
        	//
        	//	If parent is an IList
        	//		call IList.Add()
        	//		if the build object is an IChild
        	//			call IChild.OnAddedToParent()
        
            if ( m_Property != null )
            {
				string[] properties = m_Property.Split( new char[] { '.' } );
				for ( int propertyIndex = 0; propertyIndex < properties.Length - 1; ++propertyIndex )
				{
					PropertyInfo parentProperty = parent.GetType( ).GetProperty( properties[ propertyIndex ] );
					if ( parentProperty == null )
					{
            			string err = string.Format( "Parent type \"{0}\" does not contain a property \"{1}\"", parent.GetType( ), m_Property );
            			throw new ApplicationException( err );
					}
					parent = parentProperty.GetValue( parent, null );
				}

				string propertyName = properties[ properties.Length - 1 ];
            	PropertyInfo property = parent.GetType( ).GetProperty( propertyName );
            	if ( property == null )
            	{
            		string err = string.Format( "Parent type \"{0}\" does not contain a property \"{1}\"", parent.GetType( ), propertyName );
            		throw new ApplicationException( err );
            	}
            	LinkToProperty( parent, property );
            	return;
            }
            if ( m_DynProperty != null )
            {
                ISupportsDynamicProperties dynPropertySupport = parent as ISupportsDynamicProperties;
                if ( dynPropertySupport == null )
                {
                    string err = string.Format( "Parent \"{0}\" of type \"{1}\" does not support dynamic properties", ComponentHelpers.GetName( BuildObject ), parent.GetType( ) );
            		throw new ApplicationException( err );
                }
                dynPropertySupport.Properties[ m_DynProperty ] = BuildObject;
                return;
            }

			//	TODO: AP: There should be some element or attribute to disable linkage?
			LinkObjectToParent( parent, BuildObject );
        }

		/// <summary>
		/// Handles an element, creating a new builder from it and adding it to builders
		/// </summary>
		/// <param name="reader">Reader, positioned at the element</param>
		/// <param name="builders">Builder list</param>
		protected virtual void HandleElement( XmlReader reader, List< BaseBuilder > builders )
		{
			//  Create a builder from the element and add it to the current builder set
			BaseBuilder builder = CreateBuilderFromReader( this, Parameters, Errors, reader );
			if ( builder != null )
			{
				builders.Add( builder );
			}
		}

        /// <summary>
        /// Handles an element's children, in a given link context
        /// </summary>
        /// <param name="reader">Reader, positioned at the parent element</param>
		/// <param name="builders">Builder list</param>
        protected void HandleChildElements( XmlReader reader, List< BaseBuilder > builders )
        {
            if ( reader.IsEmptyElement )
            {
                reader.Read( );
                return;
            }

            reader.ReadStartElement( );

            while ( reader.NodeType != XmlNodeType.EndElement )
            {
                if ( reader.NodeType != XmlNodeType.Element )
                {
                    reader.Read( );
                    continue;
                }

                if ( reader.Name == "preLink")
                {
                    //  Recurse in "preLink" element
                    HandleChildElements( reader, GetBuilders( LinkStep.PreLink ) );
                }
                else if ( reader.Name == "postLink" )
                {
                    //  Recurse in "postLink" element
                    HandleChildElements( reader, GetBuilders( LinkStep.PostLink ) );
                }
                else
                {
					HandleElement( reader, builders );
                }
            }

            reader.ReadEndElement( );
        }
        
        /// <summary>
        /// Gets the list of builders for a given link step
        /// </summary>
        protected List< BaseBuilder > GetBuilders( LinkStep step )
        {
            if ( step == LinkStep.Default )
            {
                step = DefaultLinkStep;
            }
            return ( step == LinkStep.PreLink ? m_PreLinkBuilders : m_PostLinkBuilders );
        }

        #endregion

        #region Private stuff

        /// <summary>
        /// Calls BaseBuilder.PostCreate() for a child list of BaseBuilder objects
        /// </summary>
        private static void PostCreateChildBuilders( IEnumerable< BaseBuilder > builders )
        {
            foreach ( BaseBuilder builder in builders )
            {
                SafePostCreate( builder );
            }
        }

        /// <summary>
        /// Resolves a list of child BaseBuilder objects
        /// </summary>
        private static void ResolveChildBuilders( IEnumerable< BaseBuilder > builders, bool linkBuilders )
        {
            foreach ( BaseBuilder builder in builders )
            {
                SafeResolve( builder, linkBuilders );
            }
        }

        #endregion

        private readonly BaseBuilder             	m_ParentBuilder;
        private readonly ErrorCollection         	m_Errors;
        private readonly int                     	m_Line;
        private readonly int                     	m_Column;
        private readonly string                  	m_Name;
        private readonly string                  	m_Id;
        private readonly string                  	m_Property;
        private readonly string                  	m_DynProperty;
        private readonly ComponentLoadParameters 	m_Parameters;
        private object								m_Object;
        private readonly List< BaseBuilder >     	m_PreLinkBuilders   = new List< BaseBuilder >( );
        private readonly List< BaseBuilder >     	m_PostLinkBuilders  = new List< BaseBuilder >( );

    }
}
