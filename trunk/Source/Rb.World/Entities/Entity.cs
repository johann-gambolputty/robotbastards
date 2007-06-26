using System;
using Rb.Core.Components;
using Rb.Core.Resources;
using Rb.Core.Utils;
using Rb.Rendering;

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
        public void SetupController( Guid hostId, string controllerPath )
        {
            IHost host = m_Scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.HostType == HostType.Local ) )
            {
                //  Local hosts can't receive commands from remote controllers, so the controller
                //  must be created locally
				LoadParameters parameters = new ComponentLoadParameters( this );
                AddChild( ResourceManager.Instance.Load( controllerPath, parameters ) );
            }
            else if ( host.Id == hostId )
            {
				//  The scene host is the local controller host - create away
				LoadParameters parameters = new ComponentLoadParameters( this );
                AddChild( ResourceManager.Instance.Load( controllerPath, parameters ) );

                //  Also need listeners and so forth
            }
            else
            {
                //  Also need listeners and so forth
                throw new ApplicationException( "Unimplemented" );
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
        /// Sets the scene context that this object has been created in
        /// </summary>
        public virtual void SetSceneContext( Scene scene )
        {
            m_Scene = scene;
            scene.Renderables.Add( this );

			//	Subscribe to the update clock
			scene.GetClock( "updateClock" ).Subscribe( new Clock.TickDelegate( Update ) );
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
