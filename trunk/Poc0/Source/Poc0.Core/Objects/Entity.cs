using System;
using Rb.Core.Components;
using Rb.Core.Maths;
using Component=Rb.Core.Components.Component;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Entity
	/// </summary>
	[Serializable]
	public class Entity : Component, IHasWorldFrame, INamed
	{
		#region IHasWorldFrame Members

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

		#endregion

		#region INamed Members

		/// <summary>
		/// Name of the entity
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region Private members

		private readonly Matrix44 m_Frame = new Matrix44( );
		private string m_Name = "Bob";

		#endregion
	}
}
