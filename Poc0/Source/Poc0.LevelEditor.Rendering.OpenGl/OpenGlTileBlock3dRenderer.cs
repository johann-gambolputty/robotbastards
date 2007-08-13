using System;
using System.Collections.Generic;
using System.Text;
using Poc0.LevelEditor.Core;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	public class OpenGlTileBlock3dRenderer : TileBlockRenderer
	{
		public OpenGlTileBlock3dRenderer( TileGrid grid, TileGridEditState editState ) :
			base( grid, editState )
		{
		}

		protected override TileBlockRenderer.TileRenderer NewTileRenderer( TileTypeSet set )
		{
			return new TileRenderer( set );
		}

		protected override TileBlockRenderer.TileBlock NewTileBlock( int x, int y, int width, int height )
		{
			return new TileBlock( x, y, width, height );
		}

		private new class TileRenderer : TileBlockRenderer.TileRenderer
		{
			public TileRenderer( TileTypeSet set ) :
				base( set )
			{
			}

			public void PreRender( )
			{
			}

			public override void Render( )
			{
				base.Render( );
			}

			protected override void RenderType( TileType type, byte code )
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		private new class TileBlock : TileBlockRenderer.TileBlock
		{
			public TileBlock( int x, int y, int width, int height ) :
				base( x, y, width, height )
			{
			}

			public override bool Valid
			{
				get { throw new Exception( "The method or operation is not implemented." ); }
			}

			public override void Destroy( )
			{
				throw new Exception( "The method or operation is not implemented." );
			}

			public override void Update( TileGrid grid, TileBlockRenderer.TileRenderer renderer )
			{
				TileRenderer glRenderer = ( TileRenderer )renderer;
				throw new Exception( "The method or operation is not implemented." );
			}

			public override void Render( )
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}
	}
}
