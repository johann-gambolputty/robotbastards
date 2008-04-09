using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class Ship : IEntity, IRenderable
	{
		public Ship( )
		{
			//	TODO: AP: Oh dear... get a proper model, loser
			Graphics.Draw.StartCache( );
			Graphics.Draw.Cylinder( Graphics.Surfaces.Green, Point3.Origin + Vector3.XAxis * 1.5f, Point3.Origin + Vector3.YAxis * 3.0f, 1.0f );
			Graphics.Draw.Cylinder( Graphics.Surfaces.Green, Point3.Origin - Vector3.XAxis * 1.5f, Point3.Origin + Vector3.YAxis * 3.0f, 1.0f );
			Graphics.Draw.Cylinder( Graphics.Surfaces.Green, Point3.Origin, Point3.Origin + Vector3.YAxis * 2.0f, 0.5f );
			m_ShipGraphics = Graphics.Draw.StopCache( );
		}

		#region IEntity Members

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public UniTransform Transform
		{
			get { return m_Transform; }
		}

		#endregion

		#region Private Members

		private string m_Name;
		private readonly UniTransform m_Transform = new UniTransform( );
		private readonly IRenderable m_ShipGraphics;

		#endregion

		#region IRenderable Members

		public void Render( IRenderContext context )
		{
			if ( m_ShipGraphics == null )
			{
				return;
			}
			
			Graphics.Renderer.PushTransform( Rb.Rendering.Interfaces.Transform.LocalToWorld );
			UniCamera.SetRenderTransform( Rb.Rendering.Interfaces.Transform.LocalToWorld, Transform );
			m_ShipGraphics.Render( context );
			Graphics.Renderer.PopTransform( Rb.Rendering.Interfaces.Transform.LocalToWorld );
		}

		#endregion
	}
}
