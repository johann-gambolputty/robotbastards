using System;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Log;

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
			m_Operation = operation;
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
		protected override void OnPolygonClosed( Point2[] points )
		{
			try
			{
				CsgBrush brush = new CsgBrush( "", points );
				EditModeContext.Instance.Scene.LevelGeometry.Csg.Combine( m_Operation, brush );
			}
			catch ( Exception ex )
			{
				AppLog.Error( "Failed to combine brush with current level geometry" );
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );

				MessageBox.Show( Properties.Resources.FailedToCombineCsgBrush, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		private Csg.Operation m_Operation;
	}
}