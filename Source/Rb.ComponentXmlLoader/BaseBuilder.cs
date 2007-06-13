using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

using Rb.Core.Maths;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Core;
using Rb.Log;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Base builder
    /// </summary>
    internal class BaseBuilder
    {
        #region Factory
        
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
                    case "rb"       : result = new RootBuilder( parameters, errors, reader );                          break;
                    case "object"   : result = new ObjectBuilder( parameters, errors, reader, parentBuilder );         break;
                    case "ref"      : result = new ReferenceBuilder( parameters, errors, reader, parentBuilder );      break;
                    case "resource" : result = new ResourceBuilder( parameters, errors, reader, parentBuilder );       break;
                    case "instance" : result = new InstanceBuilder( parameters, errors, reader, parentBuilder );       break;
					case "method"	: result = new MethodBuilder( parameters, errors, reader, parentBuilder );         break;
                    case "list"     : result = new ListBuilder( parameters, errors, reader, parentBuilder );           break;
                    case "type"     : result = new TypeBuilder( parameters, errors, reader, parentBuilder, "value" );  break;
                    case "string"	:
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, reader.GetAttribute( "value" ) );
                        break;
                    case "guid"     :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Guid( reader.GetAttribute( "value" ) ) );
                        break;
                    case "bool"     :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, bool.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "char"     :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, char.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "byte"     :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, byte.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "sbyte"    :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, sbyte.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "short"    :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, short.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "ushort"   :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, ushort.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "int"      :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, int.Parse( reader.GetAttribute( "value") ) );
                        break;
                    case "uint":
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, uint.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "long"     :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, long.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "ulong"    :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, ulong.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "float"    :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, float.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "double"   :
                        result = new ValueBuilder( parameters, errors, reader, parentBuilder, double.Parse( reader.GetAttribute( "value" ) ) );
                        break;
                    case "point3"   :
                        {
                            float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    float z = float.Parse( reader.GetAttribute( "y" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Point3( x, y, z ) );
						    break;
                        }
					case "vector2"	:
                        {
						    float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Vector2( x, y ) );
						    break;
                        }
					case "vector3"	:
                        {
						    float x = float.Parse( reader.GetAttribute( "x" ) );
						    float y = float.Parse( reader.GetAttribute( "y" ) );
						    float z = float.Parse( reader.GetAttribute( "y" ) );
						    result = new ValueBuilder( parameters, errors, reader, parentBuilder, new Vector3( x, y, z ) );
						    break;
                        }
                    case "point2"   :
                    case "quat"     :
                        {
                            errors.Add( reader, "Element is not yet supported", reader.Name );
                            reader.Skip( );
                            break;
                        }
                    default :
                        {
                            errors.Add( reader, "Element was not recognised", reader.Name);
                            reader.Skip( );
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
        public virtual void Resolve( )
        {
            //  Resolve pre-link objects
            ResolveChildBuilders( m_PreLinkBuilders );

            //  Link built object to its parent
            if ( ParentBuilder != null )
            {
                Link( ParentBuilder.BuildObject );
            }

            //  Resolve post-link objects
            ResolveChildBuilders( m_PostLinkBuilders );
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
                if ( ( m_Object != null ) && ( AllowIdentification ) )
                {
                    if ( m_Name != null )
                    {
                        INamed namedObject = m_Object as INamed;
                        if ( namedObject != null )
                        {
                            namedObject.Name = m_Name;
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
                builder.Errors.Add(builder, ex, "Builder threw an exception during post-creation phase");
            }
        }

        /// <summary>
        /// Calls BaseBuilder.Resolve() on the specified builder object
        /// </summary>
        public static void SafeResolve( BaseBuilder builder )
        {
            try
            {
                builder.Resolve( );
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
        private void LinkToProperty( object parent, PropertyInfo property )
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
        			LinkToList( ( IList )propertyObject );
        		}
        		else
        		{
        			LinkToSetProperty( parent, property );
        		}
        	}
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
        protected virtual void LinkToParent( IParent parent )
        {
        	parent.AddChild( BuildObject );
        }

        /// <summary>
        /// Adds the BuildObject to an IList object. Calls IChild.OnAdded() if BuildObject implements that interface
        /// </summary>
        /// <param name="parent">Parent object</param>
        protected virtual void LinkToList( IList parent )
        {
        	parent.Add( BuildObject );
        	if ( BuildObject is IChild )
        	{
        		( ( IChild )BuildObject ).AddedToParent( parent );
        	}
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
            	PropertyInfo property = parent.GetType( ).GetProperty( m_Property );
            	if ( property == null )
            	{
            		string err = string.Format( "Parent \"{0}\" of type \"{1}\" does not contain a property \"{2}\"",
            			ComponentHelpers.GetName( BuildObject ), parent.GetType( ), m_Property );
            		throw new ApplicationException( err );
            	}
            	LinkToProperty( parent, property );
            	return;
            }
            
            IParent componentParent = parent as IParent;
            if ( componentParent != null )
            {
                LinkToParent( componentParent );
                return;
            }
            
            IList listParent = parent as IList;
            if ( listParent != null )
            {
            	LinkToList( listParent );
            }
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
        /// <param name="builders">Child BaseBuilder list</param>
        private void PostCreateChildBuilders( List< BaseBuilder > builders )
        {
            foreach ( BaseBuilder builder in builders )
            {
                SafePostCreate( builder );
            }
        }

        /// <summary>
        /// Resolves a list of child BaseBuilder objects
        /// </summary>
        /// <param name="builders">Child BaseBuilder list</param>
        private void ResolveChildBuilders( List< BaseBuilder > builders )
        {
            foreach ( BaseBuilder builder in builders )
            {
                SafeResolve( builder );
            }
        }

        #endregion

        private BaseBuilder             m_ParentBuilder;
        private ErrorCollection         m_Errors;
        private int                     m_Line;
        private int                     m_Column;
        private string                  m_Name;
        private string                  m_Id;
        private string                  m_Property;
        private ComponentLoadParameters m_Parameters;
        private object                  m_Object;
        private List< BaseBuilder >     m_PreLinkBuilders   = new List< BaseBuilder >( );
        private List< BaseBuilder >     m_PostLinkBuilders  = new List< BaseBuilder >( );

    }
}
