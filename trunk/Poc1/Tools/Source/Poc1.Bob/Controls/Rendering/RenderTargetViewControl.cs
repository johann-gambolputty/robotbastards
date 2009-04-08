using System;
using System.Windows.Forms;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Bob.Controls.Rendering
{
	public partial class RenderTargetViewControl : UserControl
	{
		public RenderTargetViewControl( )
		{
			InitializeComponent( );
		}

		#region Private Members

		/// <summary>
		/// Adds a render target to the control
		/// </summary>
		private void AddRenderTarget( IRenderTarget renderTarget )
		{
			ListViewItem item = new ListViewItem( renderTarget.Name );
			item.SubItems.Add( renderTarget.Width.ToString( ) );
			item.SubItems.Add( renderTarget.Height.ToString( ) );
			item.SubItems.Add( renderTarget.Texture == null ? "None" : renderTarget.Texture.Format.ToString( ) );
			item.Tag = renderTarget;
			renderTargetListView.Items.Add( item );
		}

		/// <summary>
		/// Removes a render target from the control
		/// </summary>
		private void RemoveRenderTarget( IRenderTarget renderTarget )
		{
			foreach ( ListViewItem item in renderTargetListView.Items )
			{
				if ( item.Tag == renderTarget )
				{
					renderTargetListView.Items.Remove( item );
					return;
				}
			}
		}

		#region Event Handlers

		private void OnRenderTargetAdded( IRenderTarget renderTarget )
		{
			AddRenderTarget( renderTarget );
		}

		private void OnRenderTargetRemoved( IRenderTarget renderTarget )
		{
			RemoveRenderTarget( renderTarget );
		}

		private void RenderTargetViewControl_Load( object sender, EventArgs e )
		{
			RenderTargets.RenderTargetAdded += OnRenderTargetAdded;
			RenderTargets.RenderTargetRemoved += OnRenderTargetRemoved;

			foreach ( IRenderTarget renderTarget in RenderTargets.Created )
			{
				AddRenderTarget( renderTarget );
			}
		}  

		#endregion
		#endregion
	}
}
