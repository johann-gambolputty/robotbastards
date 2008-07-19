using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// A graphics factory implementation that decorates another GraphicsFactory, auditing all creation calls
	/// </summary>
	public class AuditedGraphicsFactory : IGraphicsFactory
	{
		/// <summary>
		/// Sets the base factory implementation
		/// </summary>
		public AuditedGraphicsFactory( IGraphicsFactory factory )
		{
			m_Impl = factory;
		}

		/// <summary>
		/// Returns the number of items created of a given type.
		/// </summary>
		public int Count< T >( )
		{
			if ( !m_Instances.ContainsKey( typeof( T ) ) )
			{
				return 0;
			}
			return GetCleaned( typeof( T ) ).Count;
		}

		/// <summary>
		/// Returns the items of a given type created by this factory.
		/// </summary>
		public IEnumerable< T > Get< T >( )
		{
			if ( m_Instances.ContainsKey( typeof( T ) ) )
			{
				List< WeakReference > references = GetCleaned( typeof( T ) );
				foreach ( WeakReference reference in references )
				{
					object result = reference.Target;
					if ( result != null )
					{
						yield return ( T )result;
					}
				}
			}
		}

		#region IGraphicsFactory Members

		public string ApiName
		{
			get { return m_Impl.ApiName; }
		}

		public IRenderer CreateRenderer( )
		{
			return Add( m_Impl.CreateRenderer( ) );
		}

		public IEffectDataSources CreateEffectDataSources( )
		{
			return Add( m_Impl.CreateEffectDataSources( ) );
		}

		public IDraw CreateDraw( )
		{
			return Add( m_Impl.CreateDraw( ) );
		}

		public IDisplaySetup CreateDisplaySetup( )
		{
			return Add( m_Impl.CreateDisplaySetup( ) );
		}

		public IRenderTarget CreateRenderTarget( )
		{
			return Add( m_Impl.CreateRenderTarget( ) );
		}

		public ITexture2d CreateTexture2d( )
		{
			return Add( m_Impl.CreateTexture2d( ) );
		}
		
		public ITexture3d CreateTexture3d( )
		{
			return Add( m_Impl.CreateTexture3d( ) );
		}

		public ICubeMapTexture CreateCubeMapTexture( )
		{
			return Add( m_Impl.CreateCubeMapTexture( ) );
		}

		public ITexture2dSampler CreateTexture2dSampler( )
		{
			return Add( m_Impl.CreateTexture2dSampler( ) );
		}

		public ICubeMapTextureSampler CreateCubeMapTextureSampler( )
		{
			return Add( m_Impl.CreateCubeMapTextureSampler( ) );
		}

		public IMaterial CreateMaterial( )
		{
			return Add( m_Impl.CreateMaterial( ) );
		}

		public IRenderState CreateRenderState( )
		{
			return Add( m_Impl.CreateRenderState( ) );
		}

		public IFont CreateFont( FontData data )
		{
			return Add( m_Impl.CreateFont( data ) );
		}

		public IVertexBuffer CreateVertexBuffer( )
		{
			return Add( m_Impl.CreateVertexBuffer( ) );
		}

		public IIndexBuffer CreateIndexBuffer( )
		{
			return Add( m_Impl.CreateIndexBuffer( ) );
		}

		public LibraryBuilder CustomTypes
		{
			get { return m_Impl.CustomTypes; }
		}

		#endregion

		#region Private Members

		private readonly Dictionary< Type, List< WeakReference > > m_Instances = new Dictionary<Type, List<WeakReference>>( );
		private readonly IGraphicsFactory m_Impl;

		private T Add< T >( T instance )
		{
			Add( typeof( T ), instance );
			return instance;
		}

		private void Add( Type t, object instance )
		{
			List<WeakReference> references;
			if ( !m_Instances.TryGetValue( t, out references ) )
			{
				references = new List<WeakReference>( );
				m_Instances.Add( t, references );
			}
			references.Add( new WeakReference( instance ) );
		}
		
		private List<WeakReference> GetCleaned( Type t )
		{
			List< WeakReference > references = m_Instances[ t ];
			for ( int index = 0; index < references.Count; )
			{
				WeakReference reference = references[ index ];
				if ( !reference.IsAlive )
				{
					references.RemoveAt( index );
				}
				else
				{
					++index;
				}
			}
			return references;
		}

		#endregion
	}
}
