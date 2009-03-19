using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Implementation of <see cref="IRenderOrderSorter"/> that sorts renderable objects into order by type
	/// </summary>
	public class TypeOrderedRenderOrderSorter : IRenderOrderSorter
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="excludeUnordered">If true, objects that are not instances of types specified in the setup of this sorter, are not returned by the sort</param>
		public TypeOrderedRenderOrderSorter( bool excludeUnordered )
		{
			m_ExcludeUnordered = excludeUnordered;
		}

		/// <summary>
		/// Adds type to the sorter. Objects that are instances of type T will be rendered first (after types added by previous calls to AddFirstType())
		/// </summary>
		public void AddFirstType<T>( )
			where T : IRenderable
		{
			AddFirstType( typeof( T ) );
		}

		/// <summary>
		/// Adds type to the sorter. Objects that are instances of "type" will be rendered first (after types added by previous calls to AddFirstType())
		/// </summary>
		public void AddFirstType( Type type )
		{
			Arguments.CheckNotNull( type, "type" );
			if ( m_TypeToScore.ContainsKey( type ) )
			{
				throw new ArgumentException( string.Format( "Type \"{0}\" already exists in the sorter", type ), "type" );
			}
			m_TypeToScore.Add( type, m_TypeToScore.Count );
		}

		/// <summary>
		/// Adds an array of types to the sorter
		/// </summary>
		public void AddFirstTypes( params Type[] types )
		{
			Arguments.CheckNotNull( types, "types" );
			foreach ( Type type in types )
			{
				AddFirstType( type );
			}
		}

		/// <summary>
		/// Adds type to the sorter. Objects that are instances of type T will be rendered last (before types added by previous calls to AddLastType())
		/// </summary>
		public void AddLastType<T>( )
			where T : IRenderable
		{
			AddLastType( typeof( T ) );
		}

		/// <summary>
		/// Adds type to the sorter. Objects that are instances of "type" will be rendered last (before types added by previous calls to AddLastType())
		/// </summary>
		public void AddLastType( Type type )
		{
			Arguments.CheckNotNull( type, "type" );
			if ( m_TypeToScore.ContainsKey( type ) )
			{
				throw new ArgumentException( string.Format( "Type \"{0}\" already exists in the sorter", type ), "type" );
			}
			m_TypeToScore.Add( type, LastScore - m_TypeToScore.Count );
		}

		/// <summary>
		/// Adds an array of types to the sorter
		/// </summary>
		public void AddLastTypes( params Type[] types )
		{
			Arguments.CheckNotNull( types, "types" );
			foreach ( Type type in types )
			{
				AddLastType( type );
			}
		}

		/// <summary>
		/// Sorts an array of renderable objects into the order that they should be rendered in
		/// </summary>
		/// <param name="renderables">Renderable object array to sort</param>
		/// <returns>Returns the same array, sorted in render order</returns>
		public IRenderable[] Sort( IRenderable[] renderables )
		{
			if ( renderables == null || renderables.Length <= 1 )
			{
				return renderables;
			}
			return m_ExcludeUnordered ? PartialSort( renderables ) : CompleteSort( renderables );
		}

		#region Private Members

		/// <summary>
		/// Complete sort, used when unordered renderables are included
		/// </summary>
		private IRenderable[] CompleteSort( IRenderable[] renderables )
		{
			//	Build a list of OrderedRenderable objects
			OrderedRenderable[] orderedRenderables = new OrderedRenderable[ renderables.Length ];
			for ( int renderableIndex = 0; renderableIndex < renderables.Length; ++renderableIndex )
			{
				IRenderable renderable = renderables[ renderableIndex ];
				int score = GetScore( renderable );
				orderedRenderables[ renderableIndex ] = new OrderedRenderable( renderable, score );
			}

			Array.Sort( orderedRenderables );

			//	Transfer the sorted ordered renderable array back into the renderables array
			for ( int renderableIndex = 0; renderableIndex < renderables.Length; ++renderableIndex )
			{
				renderables[ renderableIndex ] = orderedRenderables[ renderableIndex ].Renderable;
			}

			return renderables;
		}

		/// <summary>
		/// Partial sort, used when unordered renderables are excluded
		/// </summary>
		private IRenderable[] PartialSort( IRenderable[] renderables )
		{
			//	Build a list of OrderedRenderable objects
			List<OrderedRenderable> orderedRenderables = new List<OrderedRenderable>( renderables.Length );
			for ( int renderableIndex = 0; renderableIndex < renderables.Length; ++renderableIndex )
			{
				IRenderable renderable = renderables[ renderableIndex ];
				int score = GetScore( renderable );
				if ( score != UnorderedScore )
				{
					orderedRenderables.Add( new OrderedRenderable( renderable, score ) );
				}
			}

			orderedRenderables.Sort( );

			IRenderable[] orderedRenderablesArray = new IRenderable[ orderedRenderables.Count ];

			//	Transfer the sorted ordered renderable array back into the renderables array
			for ( int renderableIndex = 0; renderableIndex < orderedRenderables.Count; ++renderableIndex )
			{
				orderedRenderablesArray[ renderableIndex ] = orderedRenderables[ renderableIndex ].Renderable;
			}

			return orderedRenderablesArray;
		}

		private const int UnorderedScore = 10000;
		private const int LastScore = 20000;

		private readonly bool m_ExcludeUnordered;
		private readonly Dictionary<Type, int> m_TypeToScore = new Dictionary<Type, int>( );

		/// <summary>
		/// Ordered renderable object
		/// </summary>
		private struct OrderedRenderable : IComparable<OrderedRenderable>
		{
			public OrderedRenderable( IRenderable renderable, int score )
			{
				m_Renderable = renderable;
				m_Score = score;
			}

			public IRenderable Renderable
			{
				get { return m_Renderable; }
			}

			#region IComparable<OrderedRenderable> Members

			public int CompareTo( OrderedRenderable other )
			{
				return m_Score < other.m_Score ? -1 : ( m_Score > other.m_Score ? 1 : 0 );
			}

			#endregion

			private readonly IRenderable m_Renderable;
			private readonly int m_Score;

		}

		/// <summary>
		/// Gets the score of a renderable object
		/// </summary>
		private int GetScore( IRenderable renderable )
		{
			foreach ( KeyValuePair<Type, int> typePair in m_TypeToScore )
			{
				if ( typePair.Key.IsInstanceOfType( renderable ) )
				{
					return typePair.Value;
				}
			}
			return UnorderedScore;
		}

		#endregion
	}

}
