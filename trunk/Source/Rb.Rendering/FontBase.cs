using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Handy abstract base class for implementations of <see cref="IFont"/>
	/// </summary>
	public abstract class FontBase : IFont
	{
		#region IFont Members

		public abstract Size MeasureString( string text );

		public abstract int MaximumHeight
		{
			get;
		}

		public void Write( int x, int y, Color colour, string format, params object[] args )
		{
			Write( x, y, FontAlignment.TopLeft, colour, string.Format( format, args ) );
		}

		public void Write( int x, int y, FontAlignment alignment, Color colour, string format, params object[] args )
		{
			Write( x, y, FontAlignment.TopLeft, colour, string.Format( format, args ) );
		}

		public void Write( float x, float y, float z, Color colour, string format, params object[] args )
		{
			Write( x, y, z, FontAlignment.TopLeft, colour, string.Format( format, args ) );
		}

		public void Write( float x, float y, float z, FontAlignment alignment, Color colour, string format, params object[] args )
		{
			Write( x, y, z, FontAlignment.TopLeft, colour, string.Format( format, args ) );
		}

		#endregion

		#region Protected Members
	
		public abstract void Write( int x, int y, FontAlignment alignment, Color colour, string str );

		public abstract void Write( float x, float y, float z, FontAlignment alignment, Color colour, string str );

		#endregion

	}
}
