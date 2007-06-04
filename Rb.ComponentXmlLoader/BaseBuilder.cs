using System;
using System.Collections.Generic;
using System.Text;

using Rb.Core.Components;

namespace Rb.ComponentXmlLoader
{

    /// <summary>
    /// Base builder
    /// </summary>
    internal class BaseBuilder
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="parameters">Load parameters</param>
        /// <param name="element">Element that created this builder</param>
        public BaseBuilder( ComponentLoadParameters parameters, System.Xml.XmlElement element )
        {
            m_Parameters = parameters;
            m_Element = element;
        }

        /// <summary>
        /// Resolves this builder
        /// </summary>
        public virtual void Resolve( )
        {
            
        }

        /// <summary>
        /// Sets and gets the object created by this builder
        /// </summary>
        public object BuildObject
        {
            get
            {
                return m_Object;
            }
            set
            {
                m_Object = value;
            }
        }

        #region Protected stuff

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

        protected void HandleElement( System.Xml.XmlElement element )
        {
            foreach ( System.Xml.XmlNode childNode in element.ChildNodes )
            {
                if (element.Name == "preLink")
                {
                    step = LinkStep.PreLink;
                }
                else if (element.Name == "postLink")
                {
                    step = LinkStep.PostLink;
                }
                else
                {
                }
            }
        }

        protected void HandleElement( System.Xml.XmlElement element, LinkStep step )
        {
            else
            {
            }
        }


        #endregion


        #region Private stuff

        /// <summary>
        /// Creates a builder from an element, then adds it to the appropriate link list
        /// </summary>
        private BaseBuilder CreateAndLinkBuilder( System.Xml.XmlElement element, LinkStep step )
        {
            BaseBuilder builder = CreateBuilderFromElement( element );
            GetBuilders( step ).Add( builder );
            return builder;
        }

        /// <summary>
        /// Gets the list of builders for a given link step
        /// </summary>
        private List< BaseBuilder > GetBuilders( LinkStep step )
        {
            if ( step == LinkStep.Default )
            {
                step = DefaultLinkStep;
            }
            return ( step == LinkStep.PreLink ? m_PreLinkBuilders : m_PostLinkBuilders );
        }

        /// <summary>
        /// Creates a BaseBuilder-derived object from a name
        /// </summary>
        /// <param name="element">XML element</param>
        /// <returns>Returns the new builder</returns>
        private BaseBuilder CreateBuilderFromElement( System.Xml.XmlElement element )
        {
            switch ( element.Name )
            {
                case "object"       :   return new ObjectBuilder( Parameters, element );
                case "resource"     :   return new ReferenceBuilder( Parameters, element );
                case "instance"     :
                case "ref"          :
                case "string"       :
                case "guid"         :
                case "bool"         :
                case "char"         :
                case "byte"         :
                case "sbyte"        :
                case "short"        :
                case "ushort"       :
                case "int"          :
                case "uint"         :
                case "long"         :
                case "ulong"        :
                case "float"        :
                case "double"       :
                case "point2"       :
                case "point3"       :
                case "vector2"      :
                case "vector3"      :
                case "quaternion"   :
                    return null;
            }

            return null;
        }


        #endregion

        private ComponentLoadParameters m_Parameters;
        private object                  m_Object;
        private System.Xml.XmlElement   m_Element;
        private List< BaseBuilder >     m_PreLinkBuilders   = new List< BaseBuilder >( );
        private List< BaseBuilder >     m_PostLinkBuilders  = new List< BaseBuilder >( );

    }
}
