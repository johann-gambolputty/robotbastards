
using System.ComponentModel;
using System.Drawing;
using Poc1.Tools.TerrainTextures.Core;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.PlanetBuilder
{
	/// <summary>
	/// Creates bitmaps for terrain visualiser controls
	/// </summary>
	public class TerrainVisualiserBitmapBuilder
	{
		/// <summary>
		/// Work completed delegate type
		/// </summary>
		/// <param name="result">Work results</param>
		public delegate void WorkCompleteDelegate( Bitmap result );

		/// <summary>
		/// Adds a work request. When the work is complete, the specified delegate will
		/// be called on the main UI thread
		/// </summary>
		/// <param name="model">Terrain model to use to generate the bitamp</param>
		/// <param name="width">Bitmap width</param>
		/// <param name="height">Bitmap height</param>
		/// <param name="workComplete">Delegate to call when the request has been completed</param>
		public static void AddRequest( IPlanetTerrainModel model, int width, int height, WorkCompleteDelegate workComplete )
		{
			s_SampleUpdateWorker.RunWorkerAsync( new BuildRequest( model, width, height, workComplete ) );
		}

		#region Private Members

		#region BuildRequest Class

		/// <summary>
		/// Build request
		/// </summary>
		private class BuildRequest
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public BuildRequest( IPlanetTerrainModel model, int width, int height, WorkCompleteDelegate workComplete )
			{
				m_Model = model;
				m_Width = width;
				m_Height = height;
				m_WorkComplete = workComplete;
			}

			/// <summary>
			/// Gets the delegate to invoke when the build request is complete
			/// </summary>
			public WorkCompleteDelegate WorkComplete
			{
				get { return m_WorkComplete; }
			}

			/// <summary>
			/// Gets the result bitmap
			/// </summary>
			public Bitmap BitmapResult
			{
				get { return m_BitmapResult; }
			}

			/// <summary>
			/// Runs the request
			/// </summary>
			public void Build( )
			{
				m_BitmapResult = ( ( ISpherePlanetTerrainModel )m_Model ).CreateMarbleTextureFace( CubeMapFace.PositiveX, m_Width, m_Height );
			}

			private Bitmap m_BitmapResult;
			private readonly IPlanetTerrainModel m_Model;
			private readonly int m_Width;
			private readonly int m_Height;
			private readonly WorkCompleteDelegate m_WorkComplete;
		}

		#endregion

		private readonly static BackgroundWorker s_SampleUpdateWorker = new BackgroundWorker( );

		static TerrainVisualiserBitmapBuilder( )
		{
			s_SampleUpdateWorker.DoWork +=
				delegate( object sender, DoWorkEventArgs e )
				{
					( ( BuildRequest )e.Argument ).Build( );
					e.Result = e.Argument;
				};

			s_SampleUpdateWorker.RunWorkerCompleted +=
				delegate( object sender, RunWorkerCompletedEventArgs e )
				{
					BuildRequest request = ( BuildRequest )e.Result;
					request.WorkComplete( request.BitmapResult );
				};
		}

		#endregion
	}
}
