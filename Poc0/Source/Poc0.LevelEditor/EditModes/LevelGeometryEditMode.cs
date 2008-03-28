using System.Windows.Forms;
using Poc0.LevelEditor.EditModes.Controls;
using Poc0.LevelEditor.Properties;
using Rb.Rendering.Interfaces.Objects;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.EditModes;

namespace Poc0.LevelEditor.EditModes
{
	/// <summary>
	/// Edit mode used for editing level geometry
	/// </summary>
	public class LevelGeometryEditMode : EditMode, IRenderable
	{
		#region Construction

		public LevelGeometryEditMode( )
		{
			m_PolygonBrush	= new PolygonBrushEditor( );
			m_CircleBrush	= new CircleBrushEditor( 3 );
			m_CurrentBrush	= m_PolygonBrush;
		}

		#endregion

		#region EditMode overrides

		/// <summary>
		/// Returns a description of the edit mode inputs
		/// </summary>
		public override string InputDescription
		{
			get
			{
				return ( m_CurrentBrush == null ) ? "" : m_CurrentBrush.Description;
			}
		}

		/// <summary>
		/// Gets the display name of this edit mode
		/// </summary>
		public override string DisplayName
		{
			get { return Resources.LevelGeometryEditModeName; }
		}

		/// <summary>
		/// Creates a control for this edit mode
		/// </summary>
		public override Control CreateControl( )
		{
			return new LevelGeometryEditModeControl( this );
		}

		public override void Start( )
		{
			EditorState.Instance.CurrentScene.Renderables.Add( this );
			base.Start( );
		}

		public override void Stop( )
		{
			EditorState.Instance.CurrentScene.Renderables.Remove( this );
			base.Stop( );
		}

		/// <summary>
		/// Binds this edit mode to a control
		/// </summary>
		protected override void BindToControl( Control control )
		{
			m_Control = control;
			if ( m_CurrentBrush != null )
			{
				m_CurrentBrush.BindToControl( control );
			}
		}

		/// <summary>
		/// Unbinds this edit mode from a control
		/// </summary>
		protected override void UnbindFromControl( Control control )
		{
			if ( m_CurrentBrush != null )
			{
				m_CurrentBrush.UnbindFromControl( control );	
			}
			m_Control = null;
		}

		#endregion

		#region Public members

		/// <summary>
		/// Gets the polygon brush editor
		/// </summary>
		public PolygonBrushEditor PolygonBrush
		{
			get { return m_PolygonBrush; }
		}

		/// <summary>
		/// Gets the circle brush editor
		/// </summary>
		public CircleBrushEditor CircleBrush
		{
			get { return m_CircleBrush; }
		}

		/// <summary>
		/// Starts using a brush of the specified type
		/// </summary>
		public void UseBrush( IEditor newBrush )
		{
			if ( m_CurrentBrush != null )
			{
				if ( m_Control != null )
				{
					m_CurrentBrush.UnbindFromControl( m_Control );
				}
				m_CurrentBrush = null;
			}

			m_CurrentBrush = newBrush;

			if ( m_Control != null )
			{
				m_CurrentBrush.BindToControl( m_Control );
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( IRenderContext context )
		{
			if ( m_CurrentBrush != null )
			{
				m_CurrentBrush.Render( context );
			}
		}

		#endregion
		
		#region Private members

		private readonly PolygonBrushEditor m_PolygonBrush;
		private readonly CircleBrushEditor	m_CircleBrush;
		private IEditor						m_CurrentBrush;
		private Control						m_Control;

		#endregion
	}
}
