using System;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// A ModelSet is basically an in-memory directory tree of objects that are intended to be instanciable (implement the IInstanceable interface)
	/// </summary>
	/// <remarks>
	/// Objects stored in a modelset are accessible via filesystem-style path strings (either '/' or '\' can be used to delimit model sets)
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
	/// "Badger.jpg" would only be found by the following code:
	/// <code>
	/// ModelSet.Find( "graphics/images/badger" );
	/// </code>
	/// To get the "images" ModelSet:
	/// <code>
	/// ModelSet.Find( "graphics/images" );
	/// </code>
	/// </example>
	public class ModelSet : IParentObject, INamedObject
	{

		#region	Construction

		/// <summary>
		/// Constructor
		/// </summary>
		public ModelSet( string name )
		{
			m_Name = name;
		}

		#endregion

		#region	Public properties

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
		/// The root modelset. All created ModelSet objects are added to this
		/// </summary>
		public static ModelSet	Main
		{
			get
			{
				return ms_Main;
			}
		}

		#endregion

		#region	Searches

		/// <summary>
		/// Searches for a child model set
		/// </summary>
		/// <param name="name">Child model set name</param>
		/// <returns>Returns the named model set, or null if it could not be found</returns>
		public ModelSet			FindChildModelSet( string name )
		{
			foreach ( ModelSet curSet in m_ChildModelSets )
			{
				if ( String.Compare( curSet.Name, name, true ) == 0 )
				{
					return curSet;
				}
			}
			return null;
		}

		/// <summary>
		/// Finds an object in a model set with a given name
		/// </summary>
		/// <param name="path"> Object path. This is in the form of a file path, with model sets as directories, and the final name specifying the object </param>
		/// <returns>The named object</returns>
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

		#endregion

		#region IParentObject Members

		/// <summary>
		/// Event, invoked by AddChild() after a child object has been added
		/// </summary>
		public event ChildAddedDelegate		ChildAdded;

		/// <summary>
		/// Event, invoked by AddChild() before a child object has been removed
		/// </summary>
		public event ChildRemovedDelegate	ChildRemoved;

		/// <summary>
		/// Gets the child collection
		/// </summary>
		public ICollection					Children
		{
			get
			{
				return m_Children;
			}
		}

		/// <summary>
		/// Adds a child model set to this model set
		/// </summary>
		/// <param name="childModelSet">Child model set</param>
		public void AddChildModelSet( ModelSet childModelSet )
		{
			childModelSet.m_Parent = this;
			m_ChildModelSets.Add( childModelSet );

			childModelSet.m_Path = childModelSet.MakePath( );

			Output.WriteLineCall( Output.ComponentInfo, "Added model set \"{0}\"", childModelSet.Path );
		}

		/// <summary>
		/// Adds a child object to the model set
		/// </summary>
		/// <param name="childObject"> Object to add </param>
		public void AddChild( Object childObject )
		{
			if ( childObject is ModelSet )
			{
				AddChildModelSet( ( ModelSet )childObject );
			}
			else
			{
				m_Children.Add( childObject );
			}

			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}
		}

		/// <summary>
		/// Removes a child object from the model set
		/// </summary>
		/// <param name="childObject"> Object to remove </param>
		public void RemoveChild( Object childObject )
		{
			if ( childObject is ModelSet )
			{
				m_ChildModelSets.Remove( childObject );
			}
			else
			{
				m_Children.Remove( childObject );
			}

			if ( ChildRemoved != null )
			{
				ChildRemoved( this, childObject );
			}
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

			for ( ModelSet curSet = m_Parent; curSet != null; curSet = curSet.m_Parent )
			{
				path = curSet.Name + "/" + path;
			}

			return path;
		}

		#endregion

	}
}
