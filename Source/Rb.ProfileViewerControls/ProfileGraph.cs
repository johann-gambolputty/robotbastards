using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Rb.ProfileViewerControls
{
	public partial class ProfileGraph : UserControl
	{
		public ProfileGraph( )
		{
			InitializeComponent( );
		}
		
		public ProfileSection[] Sections
		{
			get { return m_Sections; }
			set
			{
				if ( m_Sections != null )
				{
					foreach ( ProfileSection section in m_Sections )
					{
						section.OnReset -= SectionReset;
					}
				}
				m_Sections = value;
				if ( m_Sections != null )
				{
					foreach ( ProfileSection section in m_Sections )
					{
						section.OnReset += SectionReset;
						if ( !m_SectionPens.ContainsKey( section ) )
						{
							Pen pen = ms_DefaultPens[ m_CurDefaultPen ];
							m_CurDefaultPen = ( m_CurDefaultPen + 1 ) % ms_DefaultPens.Length;
							m_SectionPens.Add( section, pen );
						}
					}
				}
				Invalidate( );
			}
		}

		private float m_Zoom = 1.0f;
		private const float DefaultPenWidth = 2.0f;
		private int m_CurDefaultPen;
		private readonly static Pen[] ms_DefaultPens = new Pen[]
			{
				new Pen( Color.Red, DefaultPenWidth ), 
				new Pen( Color.Green, DefaultPenWidth ), 
				new Pen( Color.Red, DefaultPenWidth ), 
				new Pen( Color.Yellow, DefaultPenWidth ), 
				new Pen( Color.Gray, DefaultPenWidth ), 
				new Pen( Color.Orange, DefaultPenWidth ), 
				new Pen( Color.Pink, DefaultPenWidth ), 
				new Pen( Color.Beige, DefaultPenWidth ), 
				new Pen( Color.Brown, DefaultPenWidth ), 
				new Pen( Color.Magenta, DefaultPenWidth ), 
				new Pen( Color.DeepSkyBlue, DefaultPenWidth )
			};

		public Pen GetSectionPen( ProfileSection section )
		{
			return !m_SectionPens.ContainsKey( section ) ? Pens.Black : m_SectionPens[ section ];
		}
		
		public void SetSectionPen( ProfileSection section, Pen colour )
		{
			if ( !m_SectionPens.ContainsKey( section ) )
			{
				m_SectionPens.Add( section, colour );
			}
			else
			{
				m_SectionPens[ section ] = colour;
			}
		}

		private readonly Dictionary<ProfileSection, Pen> m_SectionPens = new Dictionary<ProfileSection, Pen>( );
		private ProfileSection[] m_Sections;

		private void SectionReset( ProfileSection section )
		{
			Invalidate( );
		}


		private static double AverageNSamples( IEnumerator<ProfileSection.Sample> samplePos, int n, out bool complete )
		{
			double avg = 0;
			complete = false;

			int i = 0;
			for ( ; i < n; ++i )
			{
				avg += TinyTime.ToSeconds( samplePos.Current.TotalTime );
				if ( !samplePos.MoveNext( ) )
				{
					complete = true;
					break;
				}
			}
			return i == 0 ? 0 : avg / i;
		}

		private static void DrawSection( Graphics graphics, Pen pen, int samplesPerTick, float zoom, double maxTime, int startX, int xStep, int height, ProfileSection section )
		{
			IEnumerator<ProfileSection.Sample> samplePos = section.Samples.GetEnumerator( );
			if ( !samplePos.MoveNext( ) )
			{
				return;
			}

			//	Move past first total % samplesPerTick samples
			int skip = section.TotalSampleCount % samplesPerTick;
			for ( int i = 0; i < skip; ++i )
			{
				if ( !samplePos.MoveNext( ) )
				{
					return;
				}
			}

			bool done;
			int x = startX;
			double normalize = zoom / maxTime;
			float lastY = ( float )( AverageNSamples( samplePos, samplesPerTick, out done ) * normalize ) * height;

			while ( !done )
			{
				float y = ( float )( AverageNSamples( samplePos, samplesPerTick, out done ) * normalize ) * height;
				if ( !done )
				{
					graphics.DrawLine( pen, x - xStep, height - y, x, height - lastY );
				}

				lastY = y;
				x -= xStep;
				if ( x < 0 )
				{
					break;
				}
			}
		}

		private void ProfileGraph_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.Clear( Color.White );
			if ( m_Sections == null )
			{
				return;
			}

			double maxTime = 0.0000001;
			foreach ( ProfileSection section in m_Sections )
			{
				double curSectionMaxTime = TinyTime.ToSeconds( section.AverageTime ) * 3.0;
				maxTime = maxTime > curSectionMaxTime ? maxTime : curSectionMaxTime;
			}

			foreach ( ProfileSection section in m_Sections )
			{
				Pen sectionPen = GetSectionPen( section );
				DrawSection( e.Graphics, sectionPen, 10, m_Zoom, maxTime, Width, 10, Height, section );
			}
		}

		private void ProfileGraph_Load( object sender, System.EventArgs e )
		{
			DoubleBuffered = true;
		}


		private void zoomInButton_Click(object sender, System.EventArgs e)
		{
			m_Zoom = Utils.Min( m_Zoom + 0.1f, 4.0f );
		}

		private void zoomOutButton_Click(object sender, System.EventArgs e)
		{
			m_Zoom = Utils.Max( m_Zoom - 0.1f, 0.5f );
		}
	}
}
