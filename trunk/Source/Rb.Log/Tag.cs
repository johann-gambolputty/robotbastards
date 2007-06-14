using System.Collections.Generic;
using System.Diagnostics;

namespace Rb.Log
{
	/// <summary>
	/// Tags store child tags, and log sources
	/// </summary>
	public class Tag
	{
		#region Public construction

		/// <summary>
		/// Sets up this tag. The Root tag is the parent
		/// </summary>
		/// <param name="name">Tag name</param>
		public Tag( string name )
		{
			Construct( Root, name );
		}

		/// <summary>
		/// Sets up this tag
		/// </summary>
		/// <param name="parent">The parent tag</param>
		/// <param name="name">Tag name</param>
		public Tag ( Tag parent, string name )
		{
			Construct( parent, name );
		}

		#endregion

        #region Child tags

        /// <summary>
        /// Finds a child tag with a given name
        /// </summary>
        public Tag FindChildTag( string name )
        {
            foreach ( Tag childTag in m_ChildTags )
            {
                if ( childTag.Name == name )
                {
                    return childTag;
                }
            }
            return null;
        }

        #endregion

        #region Debug source output helpers

        /// <summary>
		/// Writes to the DebugSource for the Severity.DebugVerbose level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public void DebugVerbose( string msg, params object[] args )
		{
			GetDebugSource( Severity.Verbose ).Write( 2, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugInfo level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public void DebugInfo( string msg, params object[ ] args )
		{
			GetDebugSource( Severity.Info ).Write( 2, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugWarning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public void DebugWarning( string msg, params object[ ] args )
		{
			GetDebugSource( Severity.Warning ).Write( 2, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugError level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public void DebugError( string msg, params object[ ] args )
		{
			GetDebugSource( Severity.Error ).Write( 2, msg, args );
		}

		#endregion

		#region Source output helpers

		/// <summary>
		/// Writes to the Source for the Severity.Verbose level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public void Verbose( string msg, params object[ ] args )
		{
			GetSource( Severity.Verbose ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Info level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public void Info( string msg, params object[ ] args )
		{
			GetSource( Severity.Info ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Warning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public void Warning( string msg, params object[ ] args )
		{
			GetSource( Severity.Warning ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Error level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public void Error( string msg, params object[ ] args )
		{
			GetSource( Severity.Error ).Write( 1, msg, args );
		}

		#endregion

		#region Sources

		/// <summary>
		/// Returns a DebugSource for a given severity
		/// </summary>
		/// <param name="severity">Output severity</param>
		/// <returns>Returns an appropriate DebugSource</returns>
		public DebugSource GetDebugSource ( Severity severity )
		{
			return m_DebugSources[ ( int )severity ];
		}

		/// <summary>
		/// Returns a Source for a given severity
		/// </summary>
		/// <param name="severity">Output severity</param>
		/// <returns>Returns an appropriate Source</returns>
		public Source GetSource ( Severity severity )
		{
			return m_Sources[ ( int )severity ];
		}

		#endregion

		#region Public properties

        /// <summary>
        /// Returns the list of child tags associated with this tag
        /// </summary>
        public List< Tag > ChildTags
        {
            get { return m_ChildTags; }
        }

        /// <summary>
        /// Returns the array of sources associated with this tag
        /// </summary>
	    public Source[] Sources
	    {
	        get { return m_Sources; }
	    }

		/// <summary>
		/// Gets the full name of this tag
		/// </summary>
		public string FullName
		{
			get { return m_FullName; }
		}

		/// <summary>
		/// Gets the name of this tag
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the root tag
		/// </summary>
		public static Tag Root
		{
			get { return ms_Root; }
		}

		/// <summary>
		/// Returns true if this is the root tag
		/// </summary>
		public bool IsRootTag
		{
			get { return m_Parent == null; }
		}

        /// <summary>
        /// Gets the parent of this tag
        /// </summary>
	    public Tag Parent
	    {
	        get { return m_Parent; }
	    }

		/// <summary>
		/// Suppresses or enables log entries written from this and all child sources
		/// </summary>
		public bool Suppress
		{
			get { return m_Suppress; }
			set
			{
				m_Suppress = value;
				foreach ( Tag childTag in m_ChildTags )
				{
					childTag.Suppress = value;
				}
				foreach ( Source src in m_Sources )
				{
					src.Suppress = value;
				}
				foreach ( DebugSource src in m_DebugSources )
				{
					src.Suppress = value;
				}
			}
		}

		#endregion

		#region Private stuff
		
		private Source[ ]		m_Sources		= new Source[ ( int )Severity.Count ];
		private DebugSource[ ]	m_DebugSources	= new DebugSource[ ( int )Severity.Count ];
		private static Tag		ms_Root			= new Tag( );
		private List< Tag >		m_ChildTags		= new List< Tag >( );
		private string			m_Name;
		private string			m_FullName;
		private Tag				m_Parent;
		private bool			m_Suppress;

		/// <summary>
		/// Root tag constructor
		/// </summary>
		private Tag( )
		{
			m_Name = "";
		}

		/// <summary>
		/// Construction helper
		/// </summary>
		/// <param name="parent">Parent tag</param>
		/// <param name="name">Tag name</param>
		private void Construct ( Tag parent, string name )
		{
			m_Parent = parent;
			m_Name = name;
			m_Parent.m_ChildTags.Add( this );
			m_FullName = ( m_Parent.IsRootTag ? name : m_Parent.FullName + "." + name );

			for ( int severity = 0; severity < ( int )Severity.Count; ++severity )
			{
				m_DebugSources[ severity ]	= new DebugSource( this, ( Severity )severity );
				m_Sources[ severity ]		= new Source( this, ( Severity )severity );
			}
		}

		#endregion
	}
}
