using System;
using System.Collections.Generic;
using Rb.Assets.Interfaces;

namespace Rb.Assets
{
	/// <summary>
	/// Manages a set of file system objects
	/// </summary>
	public class Locations
	{

		#region Public members

		/// <summary>
		/// Helper function that creates a new ILocation object from a path
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if the path does not refer to a valid location</exception>
		public static ILocation NewLocation( string path )
		{
			ILocation location = Instance.GetLocation( path );
			if ( location == null )
			{
				throw new ArgumentException( string.Format( "Location \"{0}\" does not exist", path ), "path" );
			}
			return location;
		}

		/// <summary>
		/// Gets the singleton instance
		/// </summary>
		public static Locations Instance
		{
			get { return ms_Singleton; }
		}

		/// <summary>
		/// Gets the list of location managers
		/// </summary>
		public IList< ILocationManager > Systems
		{
			get { return m_Systems;  }
		}

		/// <summary>
		/// Finds a location manager that can find a specified location
		/// </summary>
		public ILocationManager Find( ILocation location )
		{
			foreach ( ILocationManager manager in Systems )
			{
				if ( manager.IsPathValid( location.Path ) )
				{
					return manager;
				}
			}

			return null;
		}

		#endregion

		#region IFileSystem members

		/// <summary>
		/// Returns true if a specified path refers to an existing location
		/// </summary>
		public bool IsPathValid(string path)
		{
			foreach (ILocationManager manager in m_Systems)
			{
				if (manager.IsPathValid(path))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the location (folder or file) at a specified path
		/// </summary>
		/// <param name="path">Location path</param>
		/// <returns>Returns the specified location, or null if nothing exists at the path</returns>
		public ILocation GetLocation( string path )
		{
			foreach ( ILocationManager manager in m_Systems )
			{
				ILocation location = manager.GetLocation( path );
				if ( location != null )
				{
					return location;
				}

			}
			return null;
		}

		/// <summary>
		/// Returns the folder at a specified path
		/// </summary>
		/// <param name="path">Folder path</param>
		/// <returns>Returns the specified folder, or null if no folder exists at the path</returns>
		public IFolder GetFolder(string path)
		{
			foreach ( ILocationManager manager in m_Systems )
			{
				IFolder location = manager.GetFolder( path );
				if ( location != null )
				{
					return location;
				}

			}
			return null;
		}

		/// <summary>
		/// Returns the file at a specified path
		/// </summary>
		/// <param name="path">File path</param>
		/// <returns>Returns the specified file, or null if no files exists at the path</returns>
		public IFile GetFile( string path )
		{
			foreach ( ILocationManager manager in m_Systems )
			{
				IFile location = manager.GetFile( path );
				if ( location != null )
				{
					return location;
				}

			}
			return null;
		}


		#endregion

		#region Private members

		private readonly List< ILocationManager > m_Systems = new List< ILocationManager >( );
		private static readonly Locations ms_Singleton = new Locations( );

		#endregion
	}
}