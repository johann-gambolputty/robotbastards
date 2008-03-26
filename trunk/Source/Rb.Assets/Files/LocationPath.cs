using System;
using System.IO;
using Rb.Assets.Interfaces;
using System.Runtime.Serialization;

namespace Rb.Assets.Files
{
	[Serializable]
	public abstract class LocationPath : ILocation
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="locations">Location manager that created this object</param>
		/// <param name="path">Path to this location</param>
		public LocationPath( ILocationManager locations, string path )
		{
			m_Locations = locations;
			m_Path = path;
			m_Name = System.IO.Path.GetFileName( m_Path );
		}

		#region Public members

		/// <summary>
		/// Returns the location path
		/// </summary>
		public override string ToString( )
		{
			return Path;
		}

		/// <summary>
		/// Deserialization handler
		/// </summary>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			m_Locations = Locations.Instance.Find( this );
			m_Name = System.IO.Path.GetFileName( m_Path );
		}

		#endregion

		#region Protected members

		protected ILocationManager LocationManager
		{
			get { return m_Locations; }
		}

		protected abstract FileSystemWatcher CreateWatcher( );

		protected void AttachWatcher( )
		{
			if ( m_Watcher == null )
			{
				m_Watcher = CreateWatcher( );
				m_Watcher.Changed += InvokeOnChanged;
				m_Watcher.Deleted += InvokeOnChanged;
				m_Watcher.Created += InvokeOnChanged;
				m_Watcher.EnableRaisingEvents = true;
			}
		}
		
		protected void DetachWatcher( )
		{
			if ( m_Watcher != null )
			{
				m_Watcher.Dispose( );
				m_Watcher = null;
			}
		}

		protected void InvokeOnChanged( object sender, FileSystemEventArgs args )
		{
			if ( m_OnChanged != null )
			{
				m_OnChanged( this );
			}
		}

		#endregion

		#region ILocation Members

		/// <summary>
		/// Gets the folder that contains this location. Returns null if this location is the root folder
		/// </summary>
		public IFolder ParentFolder
		{
			get
			{
				if ( m_ParentFolder == null )
				{
					m_ParentFolder = m_Locations.GetParentFolder( m_Path );
				}
				return m_ParentFolder;
			}
		}

		/// <summary>
		/// Gets the object that manages this location
		/// </summary>
		public ILocationManager FileSystem
		{
			get { return m_Locations; }
		}

		#endregion

		#region ISource Members

		/// <summary>
		/// Event, invoked if the source changes
		/// </summary>
		public event Action<ISource> SourceChanged
		{
			add
			{
				if ( m_OnChanged == null )
				{
					AttachWatcher( );
				}
				m_OnChanged += value;
			}
			remove
			{
				m_OnChanged -= value;
				if ( m_OnChanged == null )
				{
					DetachWatcher( );
				}
			}
		}

		/// <summary>
		/// Path to this location (e.g. "c:\a\b\c.txt")
		/// </summary>
		public string Path
		{
			get { return m_Path; }
		}

		/// <summary>
		/// Gets the name of this location. For files, this includes the extension (e.g. "c.txt")
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the location of this source (returns this)
		/// </summary>
		public ILocation Location
		{
			get { return this; }
		}

		/// <summary>
		/// Gets the extension of this source. For folders, this returns an empty string
		/// </summary>
		public virtual string Extension
		{
			get { return ""; }
		}

		/// <summary>
		/// Returns true if this source has a specified extension
		/// </summary>
		public bool HasExtension( string extension )
		{
			return string.Compare( extension, Extension, StringComparison.InvariantCultureIgnoreCase ) == 0;
		}

		#endregion

		#region Private members

		private readonly string m_Path;

		[NonSerialized]
		private Action<ISource> m_OnChanged;

		[NonSerialized]
		private string m_Name;

		[NonSerialized]
		private IFolder m_ParentFolder;

		[NonSerialized]
		private ILocationManager m_Locations;

		[NonSerialized]
		private FileSystemWatcher m_Watcher;

		#endregion

	}
}
