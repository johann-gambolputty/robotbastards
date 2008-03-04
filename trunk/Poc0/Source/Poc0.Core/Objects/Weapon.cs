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

		#region IChild Members

		public void AddedToParent( object parent )
		{
			m_Owner = parent;
			IMessageHub hub = ( IMessageHub )m_Owner;
			MessageHub.AddRecipient< FireWeaponMessage >( hub, FireWeapon, 0 );
		}

		[Dispatch]
		private MessageRecipientResult FireWeapon( FireWeaponMessage msg )
		{
			IPlaceable placeable = ( IPlaceable )m_Owner;
			Point3 start = placeable.Frame.Translation + placeable.Frame.YAxis * 1.5f;
			Vector3 dir =  placeable.Frame.ZAxis;

			new Projectile( m_Scene, start, dir, 0.1f );

			return MessageRecipientResult.DeliverToNext;
		}

		public void RemovedFromParent( object parent )
		{
			m_Owner = null;
		}

		#endregion

		#region Public properties

		public IRenderable Graphics
		{
			get { return m_Graphics; }
			set { m_Graphics = value; }
		}

		#endregion

		#region Private members

		private object m_Owner;
		private Scene m_Scene;
		private IRenderable m_Graphics;

		#endregion

		#region ISceneObject Members

		public void AddedToScene( Scene scene )
		{
			m_Scene = scene;
		}

		public void RemovedFromScene( Scene scene )
		{
			m_Scene = null;
		}

		#endregion
	}
}
