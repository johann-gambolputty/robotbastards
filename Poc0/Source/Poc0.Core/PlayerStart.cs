using System;
using Rb.Core.Components;
using Rb.Core.Maths;

namespace Poc0.Core
{
	/// <summary>
	/// Player start point
	/// </summary>
	[Serializable]
	public class PlayerStart : Component, IHasWorldFrame
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
		/// Gets the world frame for this object
		/// </summary>
		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		private readonly Matrix44 m_Frame = new Matrix44( );
	}
}
