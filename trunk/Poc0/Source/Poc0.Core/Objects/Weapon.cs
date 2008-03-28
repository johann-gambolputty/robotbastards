using Rb.Animation;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using Rb.World;

namespace Poc0.Core.Objects
{
	public class Weapon : IChild, ISceneObject
	{
		public bool CanFire
		{
			get { return true; }
		}

		public bool Fire( )
		{
			if ( !CanFire )
			{
				return false;
			}

			return true;
		}

		#region IChild Members

		public void AddedToParent( object parent )
		{
			m_Owner = parent;
			Bind( );
		}

		public void RemovedFromParent( object parent )
		{
			Unbind( );
			m_Owner = null;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Weapon graphics object
		/// </summary>
		public IRenderable Graphics
		{
			get { return m_Graphics; }
			set { m_Graphics = value; }
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene scene )
		{
			m_Scene = scene;
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene scene )
		{
			m_Scene = null;
		}

		#endregion
		
		#region Private members

		private object			m_Owner;
		private Scene			m_Scene;
		private IRenderable 	m_Graphics;
		private bool			m_Bound;
		private IReferencePoint m_AttachPoint;
		
		[Dispatch]
		private MessageRecipientResult FireWeapon( FireWeaponMessage msg )
		{
			Point3 start = m_AttachPoint.Transform.Translation;
			Vector3 dir = m_AttachPoint.Transform.ZAxis;

			new Projectile( m_Scene, start, dir, 0.1f );

			return MessageRecipientResult.DeliverToNext;
		}
		
		/// <summary>
		/// Binds this weapon to its current owner
		/// </summary>
		private void Bind( )
		{
			if ( m_Bound )
			{
				return;
			}
			
			IMessageHub hub = ( IMessageHub )m_Owner;
			MessageHub.AddRecipient< FireWeaponMessage >( hub, FireWeapon, 0 );

			IReferencePoints refPoints = Parent.GetType< IReferencePoints >( m_Owner );
			m_AttachPoint = refPoints[ "Weapon" ];
			m_AttachPoint.OnRender += m_Graphics.Render;

			m_Bound = true;
		}

		/// <summary>
		/// Unbinds this weapon from its current owner
		/// </summary>
		private void Unbind( )
		{
			if ( !m_Bound )
			{
				return;
			}
			( ( IMessageHub )m_Owner ).RemoveRecipient( typeof( FireWeaponMessage ), this );
			m_AttachPoint.OnRender -= m_Graphics.Render;
			m_Bound = false;
		}


		#endregion
	}
}
