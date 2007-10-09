using System;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Network.Runt;
using Rb.World.Services;

namespace Rb.World.Entities
{
    /// <summary>
    /// Entity class
    /// </summary>
    public class Entity : Component, ISceneObject, IRenderable
    {
        #region Controller

        /// <summary>
        /// If the current host's ID is equal to hostId, then the specified local controller is added
        /// </summary>
        /// <param name="hostId">ID of the local controller's host</param>
        /// <param name="controllerPath">Path to the resource that describes the controller</param>
        /// <param name="parameters">Parameters to pass to the controller resource loader</param>
        public void SetupController( Guid hostId, string controllerPath, LoadParameters parameters )
        {
            IHost host = m_Scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.HostType == HostType.Local ) )
            {
                //  Local hosts can't receive commands from remote controllers, so the controller
                //  must be created locally
				parameters = ( LoadParameters )parameters.Clone( );
				parameters.Target = this;
                AddChild( AssetManager.Instance.Load( controllerPath, parameters ) );
            }
            else if ( host.Id == hostId )
			{
				//	The ChildUpdateProvider does stuff...
				ChildUpdateProvider provider = new ChildUpdateProvider( );
				provider.UpdateMessageType = typeof( CommandMessage );
				provider.Source = this;
				provider.Target = m_Scene.GetService< UpdateSource >( );
            	provider.RemoveBufferedMessages = false;
				provider.UpdateMessageType = typeof( CommandMessage );

				//  The scene host is the local controller host - create away
				parameters = ( LoadParameters )parameters.Clone( );
				parameters.Target = this;
				AddChild( AssetManager.Instance.Load( controllerPath, parameters ) );
            }
            else
			{
				//	The ChildUpdateHandler listens out for update mesages sent to the scene UpdateTarget, passing them on to this entity
				ChildUpdateHandler handler = new ChildUpdateHandler( );
				handler.Target = this;
				handler.Source = m_Scene.GetService< UpdateTarget >( );
				AddChild( handler );

				//	The ChildUpdateProvider does stuff...
				//ChildUpdateProvider provider = new ChildUpdateProvider( );
				//provider.UpdateMessageType = typeof( CommandMessage );
				//provider.Source = this;
				//provider.Target = m_Scene.GetService< UpdateSource >( );
				//provider.IgnoreUpdateHandlerMessages = false;
				//provider.RemoveBufferedMessages = false;
				//provider.UpdateMessageType = typeof( CommandMessage );
            }
        }

        #endregion

        #region Graphics

        /// <summary>
        /// Entity graphical representation
        /// </summary>
        public IRenderable Graphics
        {
            get { return m_Graphics; }
            set { m_Graphics = value; }
        }

        #endregion

		#region Updates

		/// <summary>
		/// Updates the entity
		/// </summary>
		/// <param name="updateClock">Clock causing updates</param>
		public virtual void Update( Clock updateClock )
		{
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
        public virtual void AddedToScene( Scene scene )
        {
            m_Scene = scene;
            scene.Renderables.Add( this );

			//	Subscribe to the update clock
			scene.GetService< IUpdateService >( )[ "updateClock" ].Subscribe( Update );
        }

		/// <summary>
		/// Called when this object is removed from the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public virtual void RemovedFromScene( Scene scene )
		{
			//	Remove from renderable list
			scene.Renderables.Remove( this );

			//	Unsubscribe from the update clock
			scene.GetService< IUpdateService >( )[ "updateClock" ].Unsubscribe( Update );
		}

        #endregion

        #region IRenderable Members

        public virtual void Render( IRenderContext context )
		{
            if ( Graphics != null )
            {
                Graphics.Render( context );
            }
        }

        #endregion

        #region Protected stuff

        protected Scene m_Scene;

        #endregion

        #region Private stuff

        private IRenderable m_Graphics;

        #endregion
    }
}
