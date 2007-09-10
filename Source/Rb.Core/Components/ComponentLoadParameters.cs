using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
    /// <summary>
    /// Resource loading parameters required by component resource loaders
    /// </summary>
    public class ComponentLoadParameters : Assets.LoadParameters
    {
        #region Public constructors
        
        /// <summary>
        /// Uses Builder singleton to create components, no target, internal object set
        /// </summary>
        public ComponentLoadParameters( )
        {
            m_Builder   = new Builder( );
            m_Objects   = new Dictionary< Guid, object >( );
        }

        /// <summary>
        /// Uses specified target, Builder singleton to create components, internal object set
        /// </summary>
        /// <param name="target">Target object</param>
        public ComponentLoadParameters( object target ) :
            base( target )
        {
            m_Builder   = Rb.Core.Components.Builder.Instance;
            m_Objects   = new Dictionary< Guid, object >( );
        }

        /// <summary>
        /// Uses specified builder to create components, no target, internal object set
        /// </summary>
        /// <param name="builder">Component creator</param>
        public ComponentLoadParameters( IBuilder builder )
        {
            m_Builder   = builder;
            m_Objects   = new Dictionary< Guid, object >( );
        }

        /// <summary>
        /// Uses Builder singleton to create components, no target, specified object set
        /// </summary>
        /// <param name="objects">Object set</param>
        public ComponentLoadParameters( IDictionary< Guid, object > objects )
        {
            m_Builder   = Rb.Core.Components.Builder.Instance;
            m_Objects   = objects;
        }
        
        /// <summary>
        /// Uses Builder singleton to create components, specified target, specified object set
        /// </summary>
        /// <param name="objects">Object set</param>
        /// <param name="target">Target object</param>
        public ComponentLoadParameters( IDictionary< Guid, object > objects, object target ) :
            base( target )
        {
            m_Builder   = Rb.Core.Components.Builder.Instance;
            m_Objects   = objects;
        }

        /// <summary>
        /// Uses specified IBuilder to create components, specified target, specified object set
        /// </summary>
        /// <param name="objects">Object set</param>
        /// <param name="builder">Component creator</param>
        public ComponentLoadParameters( IDictionary< Guid, object > objects, IBuilder builder )
        {
            m_Builder   = builder;
            m_Objects   = objects;
        }

        /// <summary>
        /// No target, specified builder to create components, internal object set
        /// </summary>
        /// <param name="builder">Component creator</param>
        /// <param name="target">Target object</param>
        public ComponentLoadParameters( IBuilder builder, object target ) :
            base( target )
        {
            m_Builder   = builder;
            m_Objects   = new Dictionary< Guid, object >( );
        }

        /// <summary>
        /// Uses specified IBuilder to create components, specified target, specified object set
        /// </summary>
        /// <param name="objects">Object set</param>
        /// <param name="builder">Component creator</param>
        /// <param name="target">Target object</param>
        public ComponentLoadParameters( IDictionary< Guid, object > objects, IBuilder builder, object target ) :
            base( target )
        {
            m_Builder   = builder;
            m_Objects   = objects;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Access to the builder used to create components
        /// </summary>
        public IBuilder Builder
        {
            get { return m_Builder; }
            set { m_Builder = value; }
        }

        /// <summary>
        /// Access to the object set
        /// </summary>
        public IDictionary< Guid, object > Objects
        {
            get { return m_Objects; }
            set { m_Objects = value; }
        }

        #endregion

		#region Cloning

		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns>Deep copy clone</returns>
		public override object Clone( )
		{
			ComponentLoadParameters clone = new ComponentLoadParameters( );
			DeepCopy( clone );
			return clone;
		}

		/// <summary>
		/// Copies members from this LoadParameters to parameters
		/// </summary>
		protected void DeepCopy( ComponentLoadParameters parameters )
		{
			parameters.m_Builder = m_Builder;
			parameters.m_Objects = m_Objects;
			base.DeepCopy( parameters );
		}

		#endregion

		#region Private stuff

		private IBuilder m_Builder;
        private IDictionary< Guid, object > m_Objects;

        #endregion
    }
}
