using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Stores a collection of RenderPass objects. For each pass, the technique applies it, then renders geometry using a callback
	/// </summary>
	public class RenderTechnique : Components.INamedObject, Components.IParentObject
	{
		#region	Setup

		/// <summary>
		/// Sets the name of this technique to an empty string
		/// </summary>
		public RenderTechnique( )
		{
			m_Name = "";
		}

		/// <summary>
		/// Sets up the name of this technique
		/// </summary>
		public RenderTechnique( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// Sets up this technique
		/// </summary>
		/// <param name="name"> Name of this technique </param>
		/// <param name="pass"> Pass to Add() to this technique </param>
		public RenderTechnique( string name, RenderPass pass )
		{
			m_Name = name;
			Add( pass );
		}

		/// <summary>
		/// Sets up this technique
		/// </summary>
		/// <param name="name"> Name of this technique </param>
		/// <param name="passes"> Array of passes to Add() to the technique </param>
		public RenderTechnique( string name, RenderPass[] passes )
		{
			m_Name = name;

			for ( int passIndex = 0; passIndex < passes.Length; ++passIndex )
			{
				Add( passes[ passIndex ] );
			}
		}

		/// <summary>
		/// Adds a new render pass to this technique
		/// </summary>
		/// <param name="pass"> The pass to add </param>
		public void Add( RenderPass pass )
		{
			m_Passes.Add( pass );
		}

		/// <summary>
		/// The effect that owns this technique
		/// </summary>
		public RenderEffect Effect
		{
			get
			{
				return m_Effect;
			}
			set
			{
				m_Effect = value;
			}
		}

		#endregion

		#region	Application

		/// <summary>
		/// The delegate type used by the Apply() method. It is used to render geometry between passes
		/// </summary>
		public delegate void RenderDelegate( );

		/// <summary>
		/// Applies this technique. Applies a pass, then invokes the render delegate to render stuff, for each pass
		/// </summary>
		/// <param name="render"></param>
		public void Apply( RenderDelegate render )
		{
			Begin( );

			foreach ( RenderPass pass in m_Passes )
			{
				pass.Begin( );
				render( );
				pass.End( );
			}

			End( );
		}

		/// <summary>
		/// Called by Apply() before any passes are applied
		/// </summary>
		protected virtual void Begin( )
		{
			if ( m_Effect != null )
			{
				m_Effect.Apply( );
			}
		}

		/// <summary>
		/// Called by Apply() after all passes are applied
		/// </summary>
		protected virtual void End( )
		{
		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// Access to the name of this technique
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		/// <summary>
		/// Event, invoked when the name of this technique has been changed
		/// </summary>
		public event Components.NameChangedDelegate NameChanged;

		#endregion

		#region IParentObject Members

		/// <summary>
		/// Adds a child object, but only if it's of type RenderPass
		/// </summary>
		/// <param name="childObject"> Child render pass</param>
		public void AddChild(Object childObject)
		{
			Add( ( RenderPass )childObject );
		}

		/// <summary>
		/// Visits all passes, calling visitor() for each
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public void VisitChildren( Components.ChildVisitorDelegate visitor )
		{
			for ( int childIndex = 0; childIndex < m_Passes.Count; ++childIndex )
			{
				if ( !visitor( m_Passes[ childIndex ] ) )
				{
					return;
				}
			}
		}

		#endregion

		#region	Private stuff

		private RenderEffect	m_Effect;
		private string			m_Name;
		private ArrayList		m_Passes = new ArrayList( );

		#endregion

	}
}
