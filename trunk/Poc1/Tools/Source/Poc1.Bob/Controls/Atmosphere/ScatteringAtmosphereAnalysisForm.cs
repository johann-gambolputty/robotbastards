using System.Drawing;
using System.Windows.Forms;
using Rb.Common.Controls.Graphs.Classes;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Classes.Sources;

namespace Poc1.Bob.Controls.Atmosphere
{
	public partial class ScatteringAtmosphereAnalysisForm : Form
	{
		/// <summary>
		/// Calculates wavelength intensities
		/// </summary>
		public delegate float CalculateIntensityDelegate( float height, float viewDirection, float sunDirection );

		/// <summary>
		/// Default constructor
		/// </summary>
		public ScatteringAtmosphereAnalysisForm( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the red intensity calculator
		/// </summary>
		public CalculateIntensityDelegate RedIntensityCalculator
		{
			get { return m_RedIntensityCalculator; }
			set { m_RedIntensityCalculator = value; }
		}

		/// <summary>
		/// Gets/sets the green intensity calculator
		/// </summary>
		public CalculateIntensityDelegate GreenIntensityCalculator
		{
			get { return m_GreenIntensityCalculator; }
			set { m_GreenIntensityCalculator = value; }
		}

		/// <summary>
		/// Gets/sets the blue intensity calculator
		/// </summary>
		public CalculateIntensityDelegate BlueIntensityCalculator
		{
			get { return m_BlueIntensityCalculator; }
			set { m_BlueIntensityCalculator = value; }
		}

		/// <summary>
		/// Gets/sets the mie intensity calculator
		/// </summary>
		public CalculateIntensityDelegate MieIntensityCalculator
		{
			get { return m_MieIntensityCalculator; }
			set { m_MieIntensityCalculator = value; }
		}

		#region Private Members

		private CalculateIntensityDelegate m_RedIntensityCalculator;
		private CalculateIntensityDelegate m_GreenIntensityCalculator;
		private CalculateIntensityDelegate m_BlueIntensityCalculator;
		private CalculateIntensityDelegate m_MieIntensityCalculator;

		/// <summary>
		/// X axis data sources
		/// </summary>
		private enum XAxisSource
		{
			Height,
			ViewDirection,
			SunDirection
		}

		/// <summary>
		/// Gets the current x-axis source
		/// </summary>
		private XAxisSource CurrentXAxisSource
		{
			get { return ( XAxisSource )xAxisSourceComboBox.SelectedItem; }
		}

		/// <summary>
		/// Gets the normalized value of a trackbar
		/// </summary>
		private static float GetTrackBarNormalizedValue( TrackBar trackBar )
		{
			return ( trackBar.Value - trackBar.Minimum ) / ( float )( trackBar.Maximum - trackBar.Minimum );
		}

		/// <summary>
		/// Returns x if x-axis source is height, otherwise returns fixed height
		/// </summary>
		private float GroundHeight( float x )
		{
			return ( CurrentXAxisSource == XAxisSource.Height ) ? x : GetTrackBarNormalizedValue( heightTrackBar );
		}

		/// <summary>
		/// Returns x if x-axis source is view direction, otherwise returns fixed height
		/// </summary>
		private float ViewDirection( float x )
		{
			return ( CurrentXAxisSource == XAxisSource.ViewDirection ) ? x : GetTrackBarNormalizedValue( viewDirectionTrackBar );
		}

		/// <summary>
		/// Returns x if x-axis source is sun direction, otherwise returns fixed height
		/// </summary>
		private float SunDirection( float x )
		{
			return ( CurrentXAxisSource == XAxisSource.SunDirection ) ? x : GetTrackBarNormalizedValue( sunDirectionTrackBar );
		}

		/// <summary>
		/// Red intensity calculation
		/// </summary>
		private float RedIntensity( float x )
		{
			return RedIntensityCalculator == null ? 0 : RedIntensityCalculator( GroundHeight( x ), ViewDirection( x ), SunDirection( x ) );
		}

		/// <summary>
		/// Green intensity calculation
		/// </summary>
		private float GreenIntensity( float x )
		{
			return GreenIntensityCalculator == null ? 0 : GreenIntensityCalculator( GroundHeight( x ), ViewDirection( x ), SunDirection( x ) );
		}

		/// <summary>
		/// Blue intensity calculation
		/// </summary>
		private float BlueIntensity( float x )
		{
			return BlueIntensityCalculator == null ? 0 : BlueIntensityCalculator( GroundHeight( x ), ViewDirection( x ), SunDirection( x ) );
		}

		/// <summary>
		/// Mie intensity calculation
		/// </summary>
		private float MieIntensity( float x )
		{
			return MieIntensityCalculator == null ? 0 : MieIntensityCalculator( GroundHeight( x ), ViewDirection( x ), SunDirection( x ) );
		}

		#region Event Handlers

		private void yAxisLegendPanel_Paint( object sender, PaintEventArgs e )
		{
			StringFormat format = new StringFormat( StringFormatFlags.DirectionVertical );
			e.Graphics.DrawString( "Intensity", Font, Brushes.Black, e.ClipRectangle, format );
		}

		private void ScatteringAtmosphereAnalysisForm_Load( object sender, System.EventArgs e )
		{
			xAxisSourceComboBox.Items.Add( XAxisSource.Height );
			xAxisSourceComboBox.Items.Add( XAxisSource.ViewDirection );
			xAxisSourceComboBox.Items.Add( XAxisSource.SunDirection );

			xAxisSourceComboBox.SelectedItem = XAxisSource.Height;

			atmosphereGraph.AddGraphComponent( new GraphComponent( "R", new GraphX2dSourceFunction( 0, 0, 1, 1, RedIntensity ), new GraphX2dLineRenderer( Color.Red ) ) );
			atmosphereGraph.AddGraphComponent( new GraphComponent( "G", new GraphX2dSourceFunction( 0, 0, 1, 1, GreenIntensity ), new GraphX2dLineRenderer( Color.Green ) ) );
			atmosphereGraph.AddGraphComponent( new GraphComponent( "B", new GraphX2dSourceFunction( 0, 0, 1, 1, BlueIntensity ), new GraphX2dLineRenderer( Color.Blue ) ) );
			atmosphereGraph.AddGraphComponent( new GraphComponent( "B", new GraphX2dSourceFunction( 0, 0, 1, 1, MieIntensity ), new GraphX2dLineRenderer( Color.Gray ) ) );
		}

		private void xAxisSourceComboBox_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			atmosphereGraph.XAxis.Title = xAxisSourceComboBox.SelectedText;
		}

		private void viewDirectionTrackBar_Scroll( object sender, System.EventArgs e )
		{
			atmosphereGraph.Invalidate( );
		}

		private void sunDirectionTrackBar_Scroll( object sender, System.EventArgs e )
		{
			atmosphereGraph.Invalidate( );
		}

		private void heightTrackBar_Scroll( object sender, System.EventArgs e )
		{
			atmosphereGraph.Invalidate( );
		}

		#endregion

		#endregion
	}
}