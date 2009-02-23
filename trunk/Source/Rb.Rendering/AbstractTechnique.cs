
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Abstract technique class
	/// </summary>
	public abstract class AbstractTechnique : ITechnique
	{
		/// <summary>
		/// Default constructor. Names the technique after this object's type
		/// </summary>
		public AbstractTechnique( )
		{
			m_Name = GetType( ).Name;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Technique name. Cannot be null or empty</param>
		public AbstractTechnique( string name )
		{
			Arguments.CheckNotNullOrEmpty( name, "name" );
			m_Name = name;
		}

		#region ITechnique Members

		/// <summary>
		/// Gets the name of this technique
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets/sets the effect that this technique belongs to
		/// </summary>
		public virtual IEffect Effect
		{
			get { return m_Effect;  }
			set { m_Effect = value; }
		}

		/// <summary>
		/// Applies this technique
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Renderable object</param>
		public abstract void Apply( IRenderContext context, IRenderable renderable );

		/// <summary>
		/// Applies this technique
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="render">Rendering delegate</param>
		public abstract void Apply( IRenderContext context, RenderDelegate render );

		/// <summary>
		/// Returns true if this technique is a substitude for another
		/// </summary>
		public bool IsSubstituteFor( ITechnique technique )
		{
			return Name == technique.Name;
		}

		#endregion

		#region Private Members

		private IEffect m_Effect;
		private string m_Name;

		#endregion
	}
}
