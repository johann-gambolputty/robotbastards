using System;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// EventArgs class passed by implementations of IPlanetTerrainModel.ModelChanged
	/// </summary>
	/// <remarks>
	/// Used to indicate to event handler that either geometry or textures have changed in the model.
	/// Event can be invoked when neither is true (e.g. MaximumHeight changes)
	/// </remarks>
	public class PlanetTerrainModelChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="texturesChanged">true if terrain model textures changed</param>
		/// <param name="geometryChanged">true if terrain model geometry changed</param>
		public PlanetTerrainModelChangedEventArgs( bool texturesChanged, bool geometryChanged )
		{
			m_TexturesChanged = texturesChanged;
			m_GeometryChanged = geometryChanged;
		}

		/// <summary>
		/// Returns true if the planet terrain textures have changed
		/// </summary>
		public bool TexturesChanged
		{
			get { return m_TexturesChanged; }
		}

		/// <summary>
		/// Returns true if the planet terrain geometry changed
		/// </summary>
		public bool GeometryChanged
		{
			get { return m_GeometryChanged; }
		}

		#region Private Members

		private readonly bool m_TexturesChanged;
		private readonly bool m_GeometryChanged;

		#endregion

	}
}
