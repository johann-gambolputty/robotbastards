
using System.Collections.Generic;
using System.ComponentModel;
using Poc1.Universe.Interfaces.Rendering;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Builds terrain quad patches in a separate thread
	/// </summary>
	internal class TerrainQuadPatchBuilder
	{
		/// <summary>
		/// Gets the singleton instance of this class
		/// </summary>
		public static TerrainQuadPatchBuilder Instance
		{
			get { return ms_Instance; }
		}

		/// <summary>
		/// Adds a request to generate vertices for 4 child patches of a given patch
		/// </summary>
		public void AddRequest( TerrainQuadPatch patch, IPlanetTerrain terrain )
		{
			lock ( m_RequestList )
			{
				m_RequestList.Add( new Request( patch, terrain ) );

				if ( !m_Worker.IsBusy )
				{
					m_Worker.RunWorkerAsync( );
				}
			}
		}

		#region Private Members

		#region Request Class

		private class Request
		{
			public Request( TerrainQuadPatch patch, IPlanetTerrain terrain )
			{
				m_Patch = patch;
				m_Terrain = terrain;
			}

			public IPlanetTerrain Terrain
			{
				get { return m_Terrain; }
			}

			public TerrainQuadPatch Patch
			{
				get { return m_Patch; }
			}

			#region Private Members

			private readonly TerrainQuadPatch m_Patch;
			private readonly IPlanetTerrain m_Terrain;

			#endregion
		}

		#endregion

		private readonly static TerrainQuadPatchBuilder ms_Instance = new TerrainQuadPatchBuilder( );
		private readonly List<Request> m_RequestList = new List<Request>( );
		private readonly BackgroundWorker m_Worker;

		private TerrainQuadPatchBuilder( )
		{
			BackgroundWorker worker = new BackgroundWorker( );
			worker.DoWork += ProcessRequest;
			worker.RunWorkerCompleted += RequestComplete;
			m_Worker = worker;
		}

		private void ProcessRequest( object sender, DoWorkEventArgs args )
		{
			Request request;
			lock ( m_RequestList )
			{
				request = m_RequestList[ 0 ];
				m_RequestList.RemoveAt( 0 );
			}
			args.Result = request;
		}

		private void RequestComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			Request completed = ( Request )args.Result;
			completed.Patch.RequestComplete( );
			if ( m_RequestList.Count > 0 )
			{
				m_Worker.RunWorkerAsync( );
			}
		}

		#endregion
	}
}
