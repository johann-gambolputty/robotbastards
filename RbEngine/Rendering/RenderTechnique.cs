using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Stores a collection of RenderPass objects. For each pass, the technique applies it, then renders geometry using a callback
	/// </summary>
	public class RenderTechnique : Components.Node, Components.INamedObject, ITechnique
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
		/// <param name="name">Name of this technique</param>
		/// <param name="passes">Parameter array of passes to Add() to the technique</param>
		public RenderTechnique( string name, params RenderPass[] passes )
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
			AddChild( pass );
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

		#region	Overriding technique

		/// <summary>
		/// The override technique
		/// </summary>
		public static RenderTechnique	Override
		{
			get
			{
				return ms_Override;
			}
			set
			{
				Output.DebugAssert( ( value == null ) || ( ms_Override == null ), Output.RenderingError, "Only one override technique can be active at any one time" );

				ms_Override = value;
			}
		}

		private static RenderTechnique	ms_Override;

		#endregion

		#region	Application

		/// <summary>
		/// Gets the technique that should be applied (either this technique, the override technique, or a technique in the parent effect with the same
		/// name as the override technique)
		/// </summary>
		protected RenderTechnique TechniqueToApply
		{
			get
			{
				if ( ms_Override == null )
				{
					return this;
				}
				if ( Effect == null )
				{
					return ms_Override;
				}
				RenderTechnique eqvTechnique = Effect.FindTechnique( ms_Override.Name );
				return eqvTechnique == null ? ms_Override : eqvTechnique;
			}
		}

		#endregion

		#region	ITechnique Members

		/// <summary>
		/// Applies this technique. Applies a pass, then invokes the render delegate to render stuff, for each pass
		/// </summary>
		/// <param name="render"></param>
		public virtual void Apply( TechniqueRenderDelegate render )
		{
			RenderTechnique techniqueToApply = TechniqueToApply;
			techniqueToApply.Begin( );

			if ( techniqueToApply.Children.Count > 0 )
			{
				foreach ( RenderPass pass in techniqueToApply.Children )
				{
					pass.Begin( );

					render( );

					pass.End( );
				}
			}
			else
			{
				render( );
			}

			techniqueToApply.End( );
		}

		/// <summary>
		/// Called by Apply() before any passes are applied
		/// </summary>
		protected virtual void Begin( )
		{
			if ( m_Effect != null )
			{
				m_Effect.Begin( );
			}
		}

		/// <summary>
		/// Called by Apply() after all passes are applied
		/// </summary>
		protected virtual void End( )
		{
			if ( m_Effect != null )
			{
				m_Effect.End( );
			}
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
		public override void AddChild( Object childObject )
		{
			base.AddChild( ( RenderPass )childObject );
		}

		#endregion

		#region	Private stuff

		private RenderEffect	m_Effect;
		private string			m_Name;

		#endregion
	}
}
