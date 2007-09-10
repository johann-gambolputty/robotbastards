using System;
using Rb.Core.Assets;
using Rb.Core.Components;

namespace Rb.World.Entities
{
	/// <summary>
	/// This component allows an entity to be controlled by a remote or local host
	/// </summary>
	public class EntityRemoteControl : Component, ISceneObject
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EntityRemoteControl( Guid hostId, string controllerPath )
		{
			m_HostId = hostId;
			m_ControllerPath = controllerPath;
		}
		
		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context
		/// </summary>
		public void SetSceneContext( Scene scene )
		{
            IHost host = scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.HostType == HostType.Local ) )
            {
                //  Local hosts can't receive commands from remote controllers, so the controller
                //  must be created locally
                LoadController( );
            }
            else if ( host.Id == m_HostId )
            {
				//  The scene host is the local controller host - create away
                LoadController( );

                //  Also need listeners and so forth
            }
            else
            {

            }
		}

		#endregion

		#region Private stuff

		private readonly Guid	m_HostId;
		private readonly string	m_ControllerPath;

		private void LoadController( )
		{
			Location controllerAssetLocation = new Location( m_ControllerPath );

			LoadState loader = AssetManager.Instance.CreateLoadState( controllerAssetLocation, null );
			loader.Parameters.Target = this;
			
			AddChild( loader.Load( ) );
		}

		#endregion

	}
}
