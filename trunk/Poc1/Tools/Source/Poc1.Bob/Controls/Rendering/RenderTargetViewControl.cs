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
			m_Sampler = Graphics.Factory.CreateTexture2dSampler( );
			m_Sampler.WrapS = TextureWrap.Repeat;
			m_Sampler.WrapT = TextureWrap.Repeat;
		}

		#region Private Members

		private ITexture2dSampler m_Sampler;

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

		private void renderTargetDisplay_OnRender( IRenderContext context )
		{
			if ( renderTargetListView.SelectedItems.Count == 0 )
			{
				return;
			}
			IRenderTarget renderTarget = ( IRenderTarget )renderTargetListView.SelectedItems[ 0 ].Tag;

			m_Sampler.Texture = renderTarget.Texture;
			m_Sampler.Begin( );

			float w = renderTargetDisplay.Width;
			float h = renderTargetDisplay.Height;

			Graphics.Renderer.Push2d( );
			Graphics.Draw.BeginPrimitiveList( PrimitiveType.QuadList );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, 0, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 0 );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, w, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 0 );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, w, h );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 1 );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, 0, h );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 1 );

			Graphics.Draw.EndPrimitiveList( );
			Graphics.Renderer.Pop2d( );

			m_Sampler.End( );
		}
		
		private void continuousRefreshCheckbox_CheckedChanged( object sender, EventArgs e )
		{
			refreshViewButton.Enabled = !continuousRefreshCheckbox.Checked;
			renderTargetDisplay.ContinuousRendering = continuousRefreshCheckbox.Checked;
		}

		#endregion

		#endregion
	}
}
