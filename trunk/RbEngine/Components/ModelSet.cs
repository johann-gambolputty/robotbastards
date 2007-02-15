using System;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for ModelSet.
	/// </summary>
	public class ModelSet : IParentObject, INamedObject
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelSet( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// The name of this model set
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
		}

		/// <summary>
		/// The full path of this model set
		/// </summary>
		public string Path
		{
			get
			{
				return m_Path;
			}
		}

		/// <summary>
		/// Finds an object in a model set with a given name
		/// </summary>
		/// <param name="path"> Object path. This is in the form of a file path, with model sets as directories, and the final name specifying the object </param>
		/// <returns>The named object</returns>
		/// <remarks>
		/// Uses file path conventions to specify nested model sets (either '/' or '\' can be used to delimit model sets)
		/// </remarks>
		/// <example>
		/// Given the following setup
		/// <code escaped="true">
		///		<modelSet name="graphics">
		///			<modelSet name="images">
		///				<resource path="Resources/Badger.jpg" name="badger"/>
		///			</modelSet>
		///		</modelSet>
		/// </code>
		///
		/// "Badger.jpg" would only be found by the following code:
		/// <code>
		/// ModelSet.Find( "graphics/images/badger" );
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown if the name cannot be resolved to an object</exception>
		public static Object	Find( string name )
		{
			return ms_Main.Find( 0, name.Split( new char[] { '/', '\\' } ) );
		}

		/// <summary>
		/// Finds a model set with a given name, throwing an exception if it can't be found
		/// </summary>
		/// <param name="path"> Model set path. This is in the form of a file path, with model sets as directories </param>
		/// <returns>The named model set</returns>
		/// <remarks>
		/// Uses file path conventions to specify nested model sets (either '/' or '\' can be used to delimit model sets)
		/// </remarks>
		/// <example>
		/// Given the following setup
		/// <code escaped="true">
		///		<modelSet name="graphics">
		///			<modelSet name="images">
		///				<resource path="Resources/Badger.jpg" name="badger"/>
		///			</modelSet>
		///		</modelSet>
		/// </code>
		///
		/// The "graphics/images" modelset would only be found by the following code:
		/// <code>
		/// ModelSet.Find( "graphics/images" );
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown if the name cannot be resolved to an object</exception>
		public static ModelSet	FindModelSet( string name )
		{
			return ms_Main.FindModelSet( 0, name.Split( new char[] { '/', '\\' } ), true );
		}

		/// <summary>
		/// Finds a model set with a given name, optionally throwing an exception if it can't be found
		/// </summary>
		/// <param name="path"> Model set path. This is in the form of a file path, with model sets as directories </param>
		/// <param name="throwOnFail"> If true, and the model set path can't be resolved, an exception is thrown </param>
		/// <returns>The named model set</returns>
		/// <remarks>
		/// Uses file path conventions to specify nested model sets (either '/' or '\' can be used to delimit model sets)
		/// </remarks>
		/// <example>
		/// Given the following setup
		/// <code escaped="true">
		///		<modelSet name="graphics">
		///			<modelSet name="images">
		///				<resource path="Resources/Badger.jpg" name="badger"/>
		///			</modelSet>
		///		</modelSet>
		/// </code>
		///
		/// The "graphics/images" modelset would only be found by the following code:
		/// <code>
		/// ModelSet.Find( "graphics/images" );
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown if the name cannot be resolved to an object, and throwOnFail is true</exception>
		public static ModelSet FindModelSet( string name, bool throwOnFail )
		{
			return ms_Main.FindModelSet( 0, name.Split( new char[] { '/', '\\' } ), throwOnFail );
		}

		/// <summary>
		/// Finds an object from a path specification
		/// </summary>
		/// <param name="pathIndex"> Current index in the path array </param>
		/// <param name="path"> List of ModelSet names, terminated by an object name </param>
		/// <returns> Returns the final referenced object, or null if it could not be found </returns>
		private Object	Find( int pathIndex, string[] path )
		{
			if ( pathIndex == ( path.Length - 1 ) )
			{
				string objectName = path[ pathIndex ];
				foreach ( Object childObj in m_Children )
				{
					//	TODO: Use object name map instead of flat array
					if ( ( childObj is INamedObject ) && ( String.Compare( ( ( INamedObject )childObj ).Name, objectName ) == 0 ) )
					{
						return childObj;
					}
				}

				throw new System.ApplicationException( String.Format( "Could not find object \"{0}\" in model set \"{1}\"", path[ pathIndex ], Name ) );
			}
			foreach ( ModelSet curSet in m_ChildModelSets )
			{
				if ( String.Compare( curSet.Name, path[ pathIndex ], true ) == 0 )
				{
					return curSet.Find( pathIndex + 1, path );
				}
			}

			throw new System.ApplicationException( String.Format( "Could not find model set \"{0}\" in model set \"{1}\"", path[ pathIndex ], Name ) );
		}

		/// <summary>
		/// Finds an object from a path specification
		/// </summary>
		/// <param name="pathIndex"> Current index in the path array </param>
		/// <param name="path"> List of ModelSet names, terminated by an object name </param>
		/// <param name="throwOnFail"> If true, and the path cannot be resolved, an exception is thrown </param>
		/// <returns> Returns the final referenced object, or null if it could not be found </returns>
		private ModelSet	FindModelSet( int pathIndex, string[] path, bool throwOnFail )
		{
			if ( pathIndex == path.Length )
			{
				return this;
			}

			foreach ( ModelSet curSet in m_ChildModelSets )
			{
				if ( String.Compare( curSet.Name, path[ pathIndex ], true ) == 0 )
				{
					return curSet.FindModelSet( pathIndex + 1, path, throwOnFail );
				}
			}

			if ( throwOnFail )
			{
				throw new System.ApplicationException( String.Format( "Could not find model set \"{0}\" in model set \"{1}\"", path[ pathIndex ], Name ) );
			}
			return null;
		}

		#region IParentObject Members

		/// <summary>
		/// Adds a child object to the model set
		/// </summary>
		/// <param name="childObject"> Object to add </param>
		public void AddChild( Object childObject )
		{
			if ( childObject is ModelSet )
			{
				ModelSet childModelSet = ( ModelSet )childObject;

				childModelSet.m_Parent = this;
				m_ChildModelSets.Add( childModelSet );

				childModelSet.m_Path = childModelSet.MakePath( );

				System.Diagnostics.Debug.WriteLine( String.Format( "Added model set \"{0}\"", childModelSet.Path ) );
			}
			else
			{
				m_Children.Add( childObject );
			}
		}

		#endregion

		/// <summary>
		/// The root modelset. All created ModelSet objects are added to this
		/// </summary>
		public static ModelSet	Main
		{
			get
			{
				return ms_Main;
			}
		}

		#region	Private stuff

		private string			m_Name;
		private string			m_Path;
		private ArrayList		m_Children			= new ArrayList( );
		private ArrayList		m_ChildModelSets	= new ArrayList( );
		private ModelSet		m_Parent;

		private static ModelSet	ms_Main				= new ModelSet( );

		private ModelSet( )
		{
			//	ms_Main constructor only
		}

		private string MakePath( )
		{
			string path = Name;

			for ( ModelSet curSet = m_Parent; curSet.m_Parent != null; curSet = curSet.m_Parent )
			{
				path = curSet.Name + "/" + path;
			}

			return path;
		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// ModelSet name
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		/// <summary>
		/// ModelSet name changed event
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		#endregion
	}
}
