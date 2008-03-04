using System.Drawing;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.World;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Simple implementation of the <see cref="IProjectile"/> interface
	/// </summary>
	public class Projectile : IProjectile, IRenderable
	{
		public Projectile( Scene scene, Point3 start, Vector3 dir, float speed )
		{
			m_Pos.Set( start );
			m_Dir = dir;
			m_Speed = speed;
			m_Scene = scene;

			m_Scene.Renderables.Add( this );

			IUpdateService updater = scene.GetService< IUpdateService >( );
			updater[ "updateClock" ].Subscribe( Update );
		}

		private void Update( Clock clock )
		{
			Environment.Environment env = m_Scene.Objects.GetFirstOfType<Environment.Environment>( );
			IEnvironmentCollisions collisions = env.PointCollisions;

			float t = 1.0f / ( float )TinyTime.ToSeconds( clock.LastInterval );

			Vector3 move = m_Dir * ( m_Speed * t );
			Collision col = collisions.CheckMovement( m_Pos.End, move );
			
			if ( col == null )
			{
				m_Pos.End += move;
			}
			else
			{
				m_Scene.Renderables.Remove( this );
			}

			m_Pos.Step( clock.CurrentTickTime );
		}

		#region Private members

		private readonly Scene m_Scene;
		private readonly float m_Speed;
		private readonly Vector3 m_Dir;
		private readonly Point3Interpolator m_Pos = new Point3Interpolator( );

		#endregion

		#region IProjectile Members

		public Point3 Position
		{
			get { return m_Pos.Current; }
		}

		public Vector3 Direction
		{
			get { return m_Dir; }
		}

		public float Speed
		{
			get { return m_Speed; }
		}

		#endregion

		#region IRenderable Members

		static IRenderable ms_Cache;
		static Draw.IMould ms_Mould;
		static Projectile( )
		{
			ms_Mould = Graphics.Draw.NewMould( Color.LimeGreen );
			ms_Mould.State.EnableCap( RenderStateFlag.DepthWrite );
			ms_Mould.State.EnableCap( RenderStateFlag.DepthTest );

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( ms_Mould, Point3.Origin, 0.2f );
			ms_Cache = Graphics.Draw.StopCache( );
		}


		public virtual void Render( IRenderContext context )
		{
			m_Pos.UpdateCurrent( context.RenderTime );

			Point3 pos = m_Pos.Current;

			Graphics.Renderer.PushTransform( Transform.LocalToWorld );
			Graphics.Renderer.Translate( Transform.LocalToWorld, pos.X, pos.Y, pos.Z );
			ms_Cache.Render( context );
			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
		}

		#endregion
	}
}
