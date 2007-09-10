using System.Collections.Generic;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Singleton class that manages all IAssetProvider obejcts
	/// </summary>
	public class LocationManagers : IEnumerable< ILocationManager >
	{
		#region Public properties

		/// <summary>
		/// Gets the asset providers singleton
		/// </summary>
		public static LocationManagers Instance
		{
			get { return ms_Singleton;  }
		}

		/// <summary>
		/// Adds a location manager
		/// </summary>
		/// <param name="manager">Location manager to add</param>
		public void Add( ILocationManager manager )
		{
			m_Managers.Add( manager );
		}

		/// <summary>
		/// Removes a location manager
		/// </summary>
		/// <param name="manager">Location manager to remove</param>
		public void Remove( ILocationManager manager )
		{
			m_Managers.Remove( manager );
		}

		/// <summary>
		/// Removes all location managers
		/// </summary>
		public void Clear( )
		{
			m_Managers.Clear( );
		}

		#endregion

		#region IEnumerable<ILocationManager> Members

		/// <summary>
		/// Returns an enumerator that can step through all the asset providers
		/// </summary>
		public IEnumerator< ILocationManager > GetEnumerator()
		{
			return m_Managers.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that can step through all the asset providers
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( )
		{
			return m_Managers.GetEnumerator( );
		}

		#endregion

		#region Private stuff

		private readonly List< ILocationManager > m_Managers = new List< ILocationManager >( );
		private static readonly LocationManagers ms_Singleton = new LocationManagers( );

		#endregion
	}
}
