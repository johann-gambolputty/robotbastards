using Rb.Animation;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
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

		private void Bind( )
		{
			if ( m_Bound )
			{
				return;
			}
			
			IMessageHub hub = ( IMessageHub )m_Owner;
			MessageHub.AddRecipient< FireWeaponMessage >( hub, FireWeapon, 0 );

			IReferencePoints refPoints = ( IReferencePoints )Parent.GetType< EntityGraphics >( m_Owner ).Graphics;
			refPoints[ "Weapon" ].OnRender += m_Graphics.Render;
			m_Bound = true;
		}

		private void Unbind( )
		{
			if ( !m_Bound )
			{
				return;
			}
			( ( IMessageHub )m_Owner ).RemoveRecipient( typeof( FireWeaponMessage ), this );
			m_Bound = false;
		}

		[Dispatch]
		private MessageRecipientResult FireWeapon( FireWeaponMessage msg )
		{
			IPlaceable placeable = (IPlaceable)m_Owner;
			Point3 start = placeable.Frame.Translation + placeable.Frame.YAxis * 1.5f;
			Vector3 dir = placeable.Frame.ZAxis;

			new Projectile(m_Scene, start, dir, 0.1f);

			return MessageRecipientResult.DeliverToNext;
		}

		private bool m_Bound;

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

		public IRenderable Graphics
		{
			get { return m_Graphics; }
			set
			{
				m_Graphics = value;
			}
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

		private object		m_Owner;
		private Scene		m_Scene;
		private IRenderable m_Graphics;

		#endregion
	}
}
