using System;

using Rb.Core.Resources;
using Rb.Core.Components;

namespace Rb.World.Entities
{
    /// <summary>
    /// A controller that is hosted either locally
    /// </summary>
    public class HostedController : ISceneObject, IChild
    {
        /// <summary>
        /// If the current host's ID is equal to hostId, then the specified local controller is added
        /// </summary>
        /// <param name="hostId">ID of the local controller's host</param>
        /// <param name="path">Local controller path</param>
        public virtual void LoadLocalController( Guid hostId, string path )
        {
            if ( m_Scene.GetService< IHost >( ).Id == hostId )
            {
                //  The scene host is the local controller host - create away
                m_Parent.AddChild( ResourceManager.Instance.Load( path ) );
            }
            else
            {
                throw new ApplicationException( "Controllers can only be hosted locally" );
            }
        }

        #region ISceneObject Members

        /// <summary>
        /// Sets the scene context that this object has been created in
        /// </summary>
        public void SetSceneContext( Scene scene )
        {
            m_Scene = scene;
        }

        #endregion

        #region Protected stuff

        protected IParent   m_Parent;
        protected Scene     m_Scene;

        #endregion


        #region IChild Members

        /// <summary>
        /// Called when this object is added to the parent
        /// </summary>
        public void AddedToParent( object parent )
        {
            m_Parent = ( IParent )parent;
        }

        #endregion
    }
}
