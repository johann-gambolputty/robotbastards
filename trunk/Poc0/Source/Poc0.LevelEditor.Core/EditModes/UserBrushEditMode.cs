using System;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Performs a CSG operation
	/// </summary>
	public class UserBrushEditMode : DefinePolygonEditMode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="operation">CSG operation to perform with the brush</param>
		public UserBrushEditMode( Csg.Operation operation )
		{
			m_LevelGeometry = EditorState.Instance.CurrentScene.Objects.GetFirstOfType< LevelGeometry >( );
			if ( m_LevelGeometry == null )
			{
				throw new InvalidOperationException( "Expected a LevelGeometry object to be present in the scene" );
			}
			m_Operation = operation;
		}

		/// <summary>
		/// Starts this edit mode
		/// </summary>
		public override void Start( )
		{
			m_LevelGeometry.ShowFlat = true;
			base.Start( );
		}

		/// <summary>
		/// Stops this edit mode
		/// </summary>
		public override void Stop( )
		{
			m_LevelGeometry.ShowFlat = false;
			base.Stop( );
		}

		/// <summary>
		/// Returns the input description of this edit mode
		/// </summary>
		public override string InputDescription
		{
			get
			{
				string addPoint = ResourceHelper.MouseButtonName( Buttons );
				string closePoly = Keys.Return.ToString( );
				string clearPoly = Keys.Escape.ToString( );

				return string.Format( Properties.Resources.UserBrushInputs, addPoint, closePoly, clearPoly );
			}
		}

		/// <summary>
		/// Sets/gets the current CSG operation that will be applied when the defined polygon is closed
		/// </summary>
		public Csg.Operation Operation
		{
			get { return m_Operation; }
			set { m_Operation = value; }
		}

		/// <summary>
		/// Called when the base edit mode finishes defining a polygon
		/// </summary>
		/// <param name="points">Polygon points</param>
		protected override void OnPolygonClosed( Point3[] points )
		{
			try
			{
				Point2[] points2 = new Point2[ points.Length ];
				for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
				{
					points2[ ptIndex ] = new Point2( points[ ptIndex ].X, points[ ptIndex ].Z );
				}

				UiPolygon brush = new UiPolygon( "", points2 );
				m_LevelGeometry.Csg.Combine( m_Operation, brush );
				AppLog.Info( "Combined brush with current level geometry" );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to combine brush with current level geometry" );

				MessageBox.Show( Properties.Resources.FailedToCombineCsgBrush, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		private readonly LevelGeometry m_LevelGeometry;
		private Csg.Operation m_Operation;
	}
}