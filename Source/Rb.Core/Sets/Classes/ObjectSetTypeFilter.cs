using System;
using Rb.Core.Sets.Interfaces;

namespace Rb.Core.Sets.Classes
{
	/// <summary>
	/// Filters the objects in an object set by type.
	/// </summary>
	/// <remarks>
	/// If the object set has an <see cref="ObjectSetTypeMapService"/>, then the filter uses 
	///	that to retrieve the typed objects.
	/// </remarks>
	public class ObjectSetTypeFilter : IObjectSetFilter
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="types">
		/// Types to filter (if an object in a set is assignable to one of these types, it is
		/// added to the result set.
		/// </param>
		public ObjectSetTypeFilter( params Type[] types )
		{
			m_Types = types;
		}

		#region IObjectSetFilter Members

		/// <summary>
		/// Filters the objects in a set
		/// </summary>
		/// <param name="set">Set to filter</param>
		/// <param name="resultSet">Set to store filtered objects</param>
		public virtual void Filter( IObjectSet set, IObjectSet resultSet )
		{
			if ( set == null )
			{
				throw new ArgumentNullException( "set" );
			}
			if ( resultSet == null )
			{
				throw new ArgumentNullException( "set" );
			}

			ObjectSetTypeMapService typeMap = set.Services.Service<ObjectSetTypeMapService>( );
			if ( typeMap != null )
			{
				foreach ( Type type in m_Types )
				{
					foreach ( object obj in typeMap[ type ] )
					{
						resultSet.Add( obj );
					}
				}
				return;
			}

			foreach ( object obj in set.Objects )
			{
				resultSet.Add( obj );
			}
		}

		#endregion

		#region Private Members

		private readonly Type[] m_Types;

		#endregion
	}
}
