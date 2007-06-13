using System;
using Rb.Core.Components;

namespace Rb.World
{
    /// <summary>
    /// Scene builder. Notifies created ISceneObject objects which scene they were created for
    /// </summary>
    public class SceneBuilder : IBuilder
    {
        #region Public constructors

        /// <summary>
        /// Sets the scene that created ISceneObject objects will be attached to. The Builder singleton will be used to create objects
        /// </summary>
        /// <param name="scene">Scene</param>
        public SceneBuilder( Scene scene )
        {
            m_Scene = scene;
            m_Builder = null;
        }

        /// <summary>
        /// Sets the scene that created ISceneObject objects will be attached to. The specified IBuilder will be used to create objects
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="builder">The builder used to create objects</param>
        public SceneBuilder( Scene scene, IBuilder builder )
        {
            m_Scene = scene;
            m_Builder = builder;
        }

        #endregion

        #region IBuilder methods

        /// <summary>
        /// Creates an instance of Type
        /// </summary>
        /// <param name="type">Type to instance</param>
        /// <returns>Returns new instance of type</returns>
        public object CreateInstance( Type type )
        {
            IBuilder builder = ( m_Builder == null ? Builder.Instance : m_Builder );
            return OnNewInstance( builder.CreateInstance( type ) );
        }

        /// <summary>
        /// Creates an instance of Type, with the specified constructor arguments
        /// </summary>
        /// <param name="type">Type to instance</param>
        /// <param name="constructorArgs">Constructor arguments</param>
        /// <returns>Returns new instance of type</returns>
        public object CreateInstance( Type type, object[] constructorArgs )
        {
            IBuilder builder = ( m_Builder == null ? Builder.Instance : m_Builder );
            return OnNewInstance( builder.CreateInstance( type, constructorArgs ) );
        }

        #endregion

        #region Private stuff

        private Scene       m_Scene;
        private IBuilder    m_Builder;
        
        /// <summary>
        /// Handles a newly created object. If it implements ISceneObject, ISceneObject.SetSceneContext() is called, with the attached scene
        /// </summary>
        /// <param name="obj">New object</param>
        /// <returns>Returns obj</returns>
        private object OnNewInstance( object obj )
        {
            ISceneObject sceneObject = obj as ISceneObject;
            if ( sceneObject != null )
            {
                sceneObject.SetSceneContext( m_Scene );
            }
            return obj;
        }

        #endregion
    }
}