using System;
using System.Collections;

namespace Rb.World
{
    /// <summary>
    /// Options for IRayCaster
    /// </summary>
    public class RayCastOptions
    {
        #region	Constructors

        /// <summary>
        /// Default constructor. Sets up options to cast an infinite ray against all layers, with no excluded objects
        /// </summary>
        public RayCastOptions()
        {
        }

        /// <summary>
        /// Constructor. Sets up options to cast an infinite ray against specified layers, with no excluded objects
        /// </summary>
        public RayCastOptions( ulong layers )
        {
            m_Layers = layers;
        }

        /// <summary>
        /// Constructor. Sets up options to cast an infinite ray against specified layers, with some excluded objects
        /// </summary>
        public RayCastOptions( ulong layers, params object[] excludedObjects )
        {
            m_Layers = layers;
            Exclude( excludedObjects );
        }

        #endregion

        #region	Ray length

        /// <summary>
        /// true if the ray is infinitely long, as all rays should be. false if SetMaxRayLength() has been called
        /// </summary>
        public bool InfiniteRay
        {
            get
            {
                return m_MaxLength == float.MaxValue;
            }
        }

        /// <summary>
        /// Returns the maximum length of the ray. Should only be used if InfiniteRay is false
        /// </summary>
        public float MaxRayLength
        {
            get
            {
                return m_MaxLength;
            }
        }

        /// <summary>
        /// Sets the maximum length of the ray
        /// </summary>
        public RayCastOptions SetMaxRayLength( float length )
        {
            m_MaxLength = length;
            return this;
        }

        /// <summary>
        /// Sets the ray to be infinitely long (this is the default setup)
        /// </summary>
        public RayCastOptions SetInfiniteRayLength( )
        {
            m_MaxLength = float.MaxValue;
            return this;
        }

        #endregion

        #region	Excluded objects

        /// <summary>
        /// Returns true if a specified object has been added to the exclusions list
        /// </summary>
        public bool IsExcluded(Object obj)
        {
            return m_Exclusions.IndexOf( obj ) != -1;
        }

        /// <summary>
        /// Excludes an object from the raycast
        /// </summary>
        public RayCastOptions Exclude( Object obj )
        {
            m_Exclusions.Add(obj);
            return this;
        }

        /// <summary>
        /// Excludes a list of objects from the raycast
        /// </summary>
        public RayCastOptions Exclude( Object[] objects )
        {
            for (int index = 0; index < objects.Length; ++index)
            {
                m_Exclusions.Add( objects[ index ] );
            }
            return this;
        }

        #endregion

        #region	Layers

        /// <summary>
        /// Returns true if the specified layer is included in the layers stored in this object
        /// </summary>
        public bool IsLayerIncluded(ulong layer)
        {
            return ( layer == 0 ) || ( ( layer & m_Layers ) != 0 );
        }

        /// <summary>
        /// Access to the layers bit field
        /// </summary>
        public ulong Layers
        {
            get
            {
                return m_Layers;
            }
            set
            {
                m_Layers = value;
            }
        }

        /// <summary>
        /// Checks against all layers (this is the default setup)
        /// </summary>
        public RayCastOptions AllLayers( )
        {
            m_Layers = ulong.MaxValue;
            return this;
        }

        /// <summary>
        /// Checks against no layers
        /// </summary>
        public RayCastOptions NoLayers( )
        {
            m_Layers = 0;
            return this;
        }

        /// <summary>
        /// Adds a layer to check against
        /// </summary>
        public RayCastOptions AddLayers( ulong layerFlags )
        {
            m_Layers |= layerFlags;
            return this;
        }

        /// <summary>
        /// Adds then removes layers to check against
        /// </summary>
        public RayCastOptions ChangeLayers( ulong addLayerFlags, ulong removeLayerFlags )
        {
            m_Layers |= addLayerFlags;
            m_Layers &= ~removeLayerFlags;
            return this;
        }

        /// <summary>
        /// Removes layers to check against
        /// </summary>
        public RayCastOptions RemoveLayers( ulong layerFlags )
        {
            m_Layers &= ~layerFlags;
            return this;
        }

        #endregion

        /// <summary>
        /// Gets a static default constructed RayCastOptions object
        /// </summary>
        public static RayCastOptions Default
        {
            get
            {
                return ms_Default;
            }
        }

        #region	Private stuff

        private ArrayList               m_Exclusions    = new ArrayList( );
        private ulong                   m_Layers        = ulong.MaxValue;
        private float                   m_MaxLength     = float.MaxValue;

        private static RayCastOptions   ms_Default      = new RayCastOptions( );

        #endregion
    }
}
