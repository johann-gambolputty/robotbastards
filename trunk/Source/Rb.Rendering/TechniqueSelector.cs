using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rb.Rendering
{
	/// <summary>
	/// Selects a technique from one or more effects. Standard implementation of <see cref="ITechniqueSelector"/>
	/// </summary>
	[Serializable]
	public class TechniqueSelector : ITechniqueSelector, ITechnique, ISerializable
    {
		/// <summary>
		/// Default constructor
		/// </summary>
        public TechniqueSelector( )
        {
        }

		/// <summary>
		/// Sets the effect, and selects the first stored technique
		/// </summary>
        public TechniqueSelector( IEffect effect )
        {
            Effect = effect;
        }

		/// <summary>
		/// Sets the effect, and selects the named technique
		/// </summary>
		public TechniqueSelector( IEffect effect, string name )
		{
			Effect = effect;
			Select( name );
		}

		/// <summary>
		/// Sets the selected technique
		/// </summary>
        public TechniqueSelector( ITechnique technique )
        {
            Technique = technique;
        }

		/// <summary>
		/// Serialization constructor
		/// </summary>
		/// <param name="info">Serialization info</param>
		/// <param name="context">Serialization context</param>
		public TechniqueSelector( SerializationInfo info, StreamingContext context )
		{
			m_Effect = ( IEffect )info.GetValue( EffectName, typeof( IEffect ) );
			Select( ( string )info.GetValue( TechniqueName, typeof( string ) ) );
		}

		/// <summary>
		/// Selects a named technique from the current effect
		/// </summary>
		/// <param name="name">Technique name</param>
		/// <exception cref="ArgumentException">Thrown if name does not correspond to a technique in the current effect</exception>
        public void Select( string name )
        {
            Technique = Effect.GetTechnique( name );
        }
		
		/// <summary>
		/// Access to the effect that the technique is selected from
		/// </summary>
        public IEffect Effect
        {
            get { return m_Effect; }
            set
            {
                m_Effect = value;
				if ( m_Effect == null )
				{
					m_Technique = null;
				}
				else
				{
					IEnumerator<ITechnique> techEnum = m_Effect.Techniques.GetEnumerator( );
					m_Technique = techEnum.MoveNext( ) ? techEnum.Current : null;
				}
            }
        }

		/// <summary>
		/// Access to the selected technique
		/// </summary>
        public ITechnique Technique
        {
            get { return m_Technique; }
            set
            {
                m_Technique = value;
                if ( m_Technique != null )
                {
                    m_Effect = m_Technique.Effect;
                }
            }
        }

		/// <summary>
		/// Applies the selected technique (<see cref="ITechnique.Apply(IRenderContext, IRenderable)"/>) to render the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
        public void Apply( IRenderContext context, IRenderable renderable )
        {
            if ( m_Technique != null )
            {
                m_Technique.Apply( context, renderable );
            }
            else
            {
                renderable.Render( context );
            }
        }
		
		/// <summary>
		/// Applies the selected technique (<see cref="ITechnique.Apply(IRenderContext, RenderDelegate)"/>) to render the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="render">Render delegate</param>
        public void Apply( IRenderContext context, RenderDelegate render )
        {
            if ( m_Technique != null )
            {
                m_Technique.Apply( context, render );
            }
            else
            {
                render( context );
            }
        }

		/// <summary>
		/// Returns true if this technique is a reasonable substitute for the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>true if this technique can substitute the specified technique</returns>
		public bool IsSubstituteFor( ITechnique technique )
		{
			return ( Technique != null ) && ( Technique.Name == technique.Name );
		}

		#region Serialization

		#endregion

		#region INamed Members

		/// <summary>
		/// Gets the name of the currently selected technique
		/// </summary>
		public string Name
		{
			get { return m_Technique == null ? "" : m_Technique.Name; }
			set
			{
				throw new NotSupportedException( "Can't set the name of a technique selector" );
			}
		}

		#endregion

		#region Private members
		private IEffect m_Effect;
		private ITechnique m_Technique;

		private const string EffectName = "effect";
		private const string TechniqueName = "tname";

		#endregion

		#region ISerializable Members

		/// <summary>
		/// Gets object data
		/// </summary>
		/// <param name="info">Serialization information</param>
		/// <param name="context">Streaming context</param>
		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( EffectName, m_Effect );
			info.AddValue( TechniqueName, Name );
		}

		#endregion
	}
}
