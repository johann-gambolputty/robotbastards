using System;
using System.Collections.Generic;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A group of polygons
	/// </summary>
	public class LevelGeometryGroup : ILevelGeometryObject
	{
		/// <summary>
		/// Sets up this group
		/// </summary>
		public LevelGeometryGroup( IEnumerable< ILevelGeometryObject > objects )
		{
			m_Objects = new List< ILevelGeometryObject >( objects );
		}

		/// <summary>
		/// Adds this object to a level
		/// </summary>
		public void AddToLevel( LevelGeometry level )
		{
		//	level.Add( this );
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Removes this object from a level
		/// </summary>
		public void RemoveFromLevel( LevelGeometry level )
		{
		//	level.Remove( this );
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Removes this group from a level, and adds all its constitutent geometry objects
		/// </summary>
		public void Ungroup( LevelGeometry level )
		{
			//	Remove this group from the level
			RemoveFromLevel( level );

			//	Add all 
			foreach ( ILevelGeometryObject geometryObject in m_Objects )
			{
				geometryObject.AddToLevel( level );
			}
		}

		/// <summary>
		/// Gets the geometry objects making up this group
		/// </summary>
		public IList< ILevelGeometryObject > Objects
		{
			get { return m_Objects; }
		}

		#region Private members

		private readonly List< ILevelGeometryObject > m_Objects;

		#endregion
	}

}