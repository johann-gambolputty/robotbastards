using System.ComponentModel;
using System.Threading;
using Poc1.AtmosphereTest;
using Rb.Core.Maths;
using Rb.Core.Utils;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Test.AtmosphereTest
{
	class AtmosphereCalculatorWorker
	{
		/// <summary>
		/// Called when a vertex batch is completed
		/// </summary>
		/// <param name="vertices">Atmosphere vertex array</param>
		public delegate void VertexBatchCompleteDelegate( AtmosphereSurface.Vertex[] vertices );

		/// <summary>
		/// Event, raised on the main rendering thread when a vertex batch is completed
		/// </summary>
		public event VertexBatchCompleteDelegate VertexBatchComplete;

		/// <summary>
		/// Default constructor
		/// </summary>
		public AtmosphereCalculatorWorker( )
		{
			m_WorkerThread = new BackgroundWorker( );
			m_WorkerThread.DoWork += CalculateVertexColours;
			m_WorkerThread.RunWorkerCompleted += VertexColourCalculationComplete;
			m_WorkerThread.WorkerSupportsCancellation = true;
		}

		/// <summary>
		/// Gets/sets the camera position
		/// </summary>
		public Point3 CameraPosition
		{
			get { return m_CameraPos; }
			set { m_CameraPos = value; }
		}

		/// <summary>
		/// Gets/sets the camera direction
		/// </summary>
		public Vector3 CameraDirection
		{
			get { return m_CameraDir; }
			set { m_CameraDir = value; }
		}

		/// <summary>
		/// Gets/sets the sun direction
		/// </summary>
		public Vector3 SunDirection
		{
			get { return m_SunDir; }
			set { m_SunDir = value; }
		}

		/// <summary>
		/// Gets the model used by the calculator
		/// </summary>
		public AtmosphereCalculatorModel Model
		{
			get { return m_Model; }
		}

		/// <summary>
		/// Returns the length of time, in seconds, that it took for the last calculation to run
		/// (even if the run was cancelled).
		/// </summary>
		public float DurationOfLastBatchInSeconds
		{
			get { return m_DurationOfLastBatch; }
		}

		/// <summary>
		/// Starts building
		/// </summary>
		public void Start( AtmosphereSurface.Vertex[] vertices )
		{
			Arguments.CheckNotNull( vertices, "vertices" );
			m_CanRun.WaitOne( );
			m_Vertices = vertices;
			m_WorkerThread.RunWorkerAsync( );
		}

		/// <summary>
		/// Stops the current build
		/// </summary>
		public void Stop( )
		{
			m_WorkerThread.CancelAsync( );
		}

		#region Private Members

		private AtmosphereSurface.Vertex[] m_Vertices;
		private Point3 m_CameraPos;
		private Vector3 m_CameraDir;
		private Vector3 m_SunDir;
		private readonly AtmosphereCalculatorModel m_Model = new AtmosphereCalculatorModel( );
		private readonly BackgroundWorker m_WorkerThread;
		private readonly AutoResetEvent m_CanRun = new AutoResetEvent( true );
		private float m_DurationOfLastBatch = 0;
		
		/// <summary>
		/// Worker thread runner
		/// </summary>
		private void CalculateVertexColours( object sender, DoWorkEventArgs args )
		{
			long startTime = TinyTime.CurrentTime;
			AtmosphereCalculator calculator = new AtmosphereCalculator( CameraPosition, CameraDirection, Model.Clone( ) );
			try
			{
				for ( int i = 0; i < m_Vertices.Length; ++i )
				{
					if ( m_WorkerThread.CancellationPending )
					{
						return;
					}
					m_Vertices[ i ].Colour = calculator.CalculateColour( m_Vertices[ i ].Position );
				}
			}
			finally
			{
				m_DurationOfLastBatch = ( float )TinyTime.ElapsedSeconds( startTime );
				m_CanRun.Set( );
			}
		}

		/// <summary>
		/// Called when the current batch of vertices have been updated
		/// </summary>
		private void VertexColourCalculationComplete( object sender, RunWorkerCompletedEventArgs args)
		{
			if ( VertexBatchComplete != null )
			{
				VertexBatchComplete( m_Vertices );
			}
		}

		#endregion
	}
}
