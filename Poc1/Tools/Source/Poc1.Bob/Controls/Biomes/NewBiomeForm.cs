using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class NewBiomeForm : Form, INewBiomeView
	{
		public NewBiomeForm( )
		{
			InitializeComponent( );
		}

		#region INewBiomeView Members

		/// <summary>
		/// Gets the biome name
		/// </summary>
		public string BiomeName
		{
			get { return nameTextBox.Text; }
		}

		/// <summary>
		/// Shows the view
		/// </summary>
		/// <returns>
		/// true if biome should be created
		/// </returns>
		public bool ShowView( )
		{
			return ShowDialog( ) == DialogResult.OK;
		}

		#endregion

		#region Private Members

		private void nameTextBox_TextChanged( object sender, EventArgs e )
		{
			okButton.Enabled = ( nameTextBox.Text.Length > 0 );
		}

		#endregion
	}
}