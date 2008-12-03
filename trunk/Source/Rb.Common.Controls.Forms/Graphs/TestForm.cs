using System;
using System.Threading;
using System.Windows.Forms;
using Rb.Common.Controls.Graphs.Classes;
using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;
using Timer=System.Threading.Timer;

namespace Rb.Common.Controls.Forms.Graphs
{
	public partial class TestForm : Form
	{
		[STAThread]
		public static void Main()
		{
			Application.Run(new TestForm());
		}

		public TestForm()
		{
			InitializeComponent();

			IGraph2dSource data = new Graph2dSourceUniformValue( Graph2dSourceUniformValue.Axis.X, 0.5f );
			GraphComponent component = new GraphComponent( "test0", data, data.CreateRenderer( ), new Graph2dUniformValueController( ) );
			component.Renderer.Colour = m_Colours.NextColour( );
			graphControl1.AddGraphComponent( component );

			data = new GraphX2dSourceFunction( 0, 100, -1, 1, delegate( float x ) { return ( float )Math.Sin( x ); } );
			component = new GraphComponent( "test1", data, data.CreateRenderer( ), null );
			component.Renderer.Colour = m_Colours.NextColour( );
			graphControl1.AddGraphComponent( component );

			if ( m_EnableDynamicSource )
			{
				data = m_DynamicSource;
				m_DynamicSource.MaximumNumberOfSamples = 100;
				component = new GraphComponent( "dynamic source", data, data.CreateRenderer( ), null );
				component.Renderer.Colour = m_Colours.NextColour( );
				graphControl1.AddGraphComponent( component );
			}

			GraphX2dSourceLineSegments segData = new GraphX2dSourceLineSegments( );
			segData.AddControlPoint( new Graph2dSourcePiecewiseLinear.ControlPoint( 0, 1 ) );
			segData.AddControlPoint( new Graph2dSourcePiecewiseLinear.ControlPoint( 0.2f, 0.3f ) );
			segData.AddControlPoint( new Graph2dSourcePiecewiseLinear.ControlPoint( 0.6f, 0.8f ) );
			IGraph2dRenderer renderer = segData.CreateRenderer( );
			renderer.Colour = m_Colours.NextColour( );
			graphControl1.AddGraphComponent( new GraphComponent( "very long graph name that will overrun bounds", segData, renderer, new GraphX2dControlPointController( ) ) );

			foreach ( GraphComponent graphComponent in graphControl1.GraphComponents )
			{
				graphComponentsControl1.AddGraphComponent( graphComponent );
			}

			graphComponentsControl1.AssociatedGraphControl = graphControl1;
		}

		private readonly bool m_EnableDynamicSource = false;
		private readonly GraphDefaultColours m_Colours = new GraphDefaultColours( );
		private GraphX2dSourceSamples m_DynamicSource = new GraphX2dSourceSamples( );
		private int m_DynData = 0;
		private Timer m_Timer;

		private void DynamicDataTestTimer_SynchTick(object state)
		{
			m_DynamicSource.AddValue( m_DynData / 10.0f );
			m_DynData = ( m_DynData + 1 ) % 10;
			graphControl1.ShowDataX( m_DynamicSource.MaximumX );
		}

		private void DynamicDataTestTimer_Tick(object state)
		{
			Invoke( new Action<object>( DynamicDataTestTimer_SynchTick ), state );
		}

		private void TestForm_Load(object sender, EventArgs e)
		{
			if ( m_EnableDynamicSource )
			{
				m_Timer = new Timer( DynamicDataTestTimer_Tick, null, new TimeSpan( 0 ), TimeSpan.FromMilliseconds( 500 ) );
			}
		}

		private void TestForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			if ( m_Timer != null )
			{
				WaitHandle handle = new AutoResetEvent( false );
				m_Timer.Dispose( handle );
				handle.WaitOne( 1000, false );
			}
		}
	}
}