using System.ComponentModel;

namespace Poc1.Bob.Core.Classes.Biomes.Models
{
	/// <summary>
	/// Maintains a list of information about biomes
	/// </summary>
	public class BiomeListModel
	{
		/// <summary>
		/// Gets the list of models
		/// </summary>
		public BindingList<BiomeModel> Models
		{
			get { return m_Models; }
		}

		#region Private Members

		private readonly BindingList<BiomeModel> m_Models = new BindingList<BiomeModel>( );

		#endregion
	}
}
