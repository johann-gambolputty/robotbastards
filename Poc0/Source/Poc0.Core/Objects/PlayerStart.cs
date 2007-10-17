using System;
using System.ComponentModel;
using Rb.Core.Maths;
using Component=Rb.Core.Components.Component;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Player start point
	/// </summary>
	[Serializable]
	public class PlayerStart : Component, IPlaceable
	{
		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Object's position
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
		/// Object's position over time
		/// </summary>
		[Browsable( false )]
		public Point3Interpolator Travel
		{
			get
			{
				throw new InvalidOperationException( "Should not be accessing the travel property of the player start object" );
			}
		}

		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		/// <summary>
		/// Gets/sets the player index
		/// </summary>
		public int PlayerIndex
		{
			get { return m_PlayerIndex; }
			set { m_PlayerIndex = value; }
		}

		private int m_PlayerIndex;
		private readonly Matrix44 m_Frame = new Matrix44( );
	}
}
