using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.Core
{
	public class PointLightFrame : Rb.World.LightSocket, IHasPosition
	{
		public PointLight PointLight
		{
			get { return ( PointLight )Light; }
		}

		#region IHasPosition Members

		public event PositionChangedDelegate PositionChanged;

		public Point3 Position
		{
			get { return PointLight.Position; }
			set
			{
				if ( PositionChanged == null )
				{
					PointLight.Position = value;
				}
				else
				{
					Point3 oldPos = new Point3( PointLight.Position );
					PointLight.Position = value;
					PositionChanged( this, oldPos, PointLight.Position );
				}
			}
		}

		#endregion
	}
}
