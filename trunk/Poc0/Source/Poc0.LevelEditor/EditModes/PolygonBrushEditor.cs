using System;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.Geometry;
using Poc0.LevelEditor.Properties;
using Rb.Core.Maths;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;
using System.Windows.Forms;

namespace Poc0.LevelEditor.EditModes
{
	public class PolygonBrushEditor : EdgeListEditor
	{
		/// <summary>
		/// Returns the input description of this edit mode
		/// </summary>
		public override string Description
		{
			get
			{
				string addPoint		= ResourceHelper.MouseButtonName( ms_AddPointButton );
				string closePoly	= Keys.Return.ToString( );
				string clearPoly	= Keys.Escape.ToString( );

				return string.Format( Resources.PolygonBrushInputs, addPoint, closePoly, clearPoly );
			}
		}

		/// <summary>
		/// Called when a edge list is finished
		/// </summary>
		protected override void OnFinished( Point3[] points, bool loop )
		{
			try
			{
				LevelGeometry level = LevelGeometry.FromCurrentScene( );

				Point2[] points2 = new Point2[ points.Length ];
				for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
				{
					points2[ ptIndex ] = new Point2( points[ ptIndex ].X, points[ ptIndex ].Z );
				}

				UiPolygon brush = new UiPolygon( "", points2 );
				level.Add( brush, false, false );

				AppLog.Info( "Combined brush with current level geometry" );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to combine brush with current level geometry" );
				ErrorMessageBox.Show( Resources.FailedToCombineCsgBrush );
			}
		}
	}
}
