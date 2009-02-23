using System.ComponentModel;
using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;

namespace Poc1.Bob.Controls.Planet
{
	public partial class BiomeTabControl : TabControl
	{
		/// <summary>
		/// Tab control factory
		/// </summary>
		public interface ITabFactory
		{
			/// <summary>
			/// Creates a control for the specified biome model
			/// </summary>
			Control CreateBiomeTabControl( BiomeModel model );
		}

		public BiomeTabControl( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the list of biomes displayed by this control
		/// </summary>
		public BiomeListModel Biomes
		{
			get { return m_Biomes; }
			set
			{
				if ( value != m_Biomes )
				{
					if ( m_Biomes != null )
					{
						m_Biomes.Models.ListChanged -= OnBiomeListChanged;
					}
					m_Biomes = value;
					if ( m_Biomes != null )
					{
						m_Biomes.Models.ListChanged += OnBiomeListChanged;
					}
				}
			}
		}

		/// <summary>
		/// Gets/sets the factory used to create controls on biome tab pages
		/// </summary>
		public ITabFactory TabFactory
		{
			get { return m_TabFactory; }
			set { m_TabFactory = value; }
		}

		#region Private Members

		private ITabFactory m_TabFactory;
		private BiomeListModel m_Biomes;


		#region Event Handlers

		/// <summary>
		/// Handles changes to the biome list
		/// </summary>
		private void OnBiomeListChanged( object sender, ListChangedEventArgs args )
		{
			switch ( args.ListChangedType )
			{
				case ListChangedType.ItemAdded :
					{
						BiomeModel biome = m_Biomes.Models[ args.NewIndex ];
						TabPage newPage = new TabPage( biome.Name );
						if ( TabFactory != null )
						{
							newPage.Controls.Add( TabFactory.CreateBiomeTabControl( biome ) );
						}
						TabPages.Add( newPage );
						break;
					}
				case ListChangedType.ItemDeleted :
					{
						TabPages.RemoveAt( args.NewIndex );
						break;
					}
			}
		}
		
		#endregion

		#endregion

	}
}
