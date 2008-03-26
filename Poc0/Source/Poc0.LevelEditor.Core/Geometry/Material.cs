using System;
using Rb.Assets.Interfaces;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// A static geometry material
	/// </summary>
	[Serializable]
	public class Material
	{
		/// <summary>
		/// Sets up this material
		/// </summary>
		/// <param name="name">Material display name</param>
		/// <param name="effectSource">Material effect asset source</param>
		public Material( string name, ISource effectSource )
		{
			m_Name = name;
			m_EffectSource = effectSource;
		}

		/// <summary>
		/// Gets the source of the rendering effect for this material
		/// </summary>
		public ISource EffectSource
		{
			get { return m_EffectSource; }
		}

		/// <summary>
		/// Gets the name of this material
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Returns the name of this material
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		#region Private members

		private readonly ISource m_EffectSource;
		private readonly string m_Name;

		#endregion
	}
}
