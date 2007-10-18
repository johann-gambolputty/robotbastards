using System;
using Rb.Core.Components;
using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Signpost
	/// </summary>
	[Serializable]
	public class Signpost : Component, IPlaceable, INamed
	{
		#region IPlaceable members

		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// The start position
		/// </summary>
		public Point3 Position
		{
			get { return m_Frame.Translation; }
			set
			{
				if ( PositionChanged == null )
				{
					m_Frame.Translation = value;
				}
				else
				{
					Point3 oldPos = m_Frame.Translation;
					m_Frame.Translation = value;
					PositionChanged( this, oldPos, m_Frame.Translation );
				}
			}
		}

		/// <summary>
		/// The angle of objects created at the start position
		/// </summary>
		public float Angle
		{
			get
			{
				return ( float )Math.Atan2( -m_Frame.ZAxis.Z, -m_Frame.ZAxis.X );
			}
			set
			{
				float angle = value;
				float sinA = ( float )Math.Sin( angle );
				float cosA = ( float )Math.Cos( angle );
				m_Frame.ZAxis = new Vector3( cosA, 0, sinA );
				m_Frame.XAxis = new Vector3( -sinA, 0, cosA );
			}
		}

		/// <summary>
		/// The frame of this object
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		#endregion

		#region INamed Members

		/// <summary>
		/// Gets/sets the name of this signpost
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region Private stuff

		private readonly Matrix44 m_Frame = new Matrix44( );
		private string m_Name;

		#endregion

	}
}
