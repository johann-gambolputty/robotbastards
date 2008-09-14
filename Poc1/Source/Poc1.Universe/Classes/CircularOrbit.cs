using System;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Poc1.Universe.Classes
{
	public class CircularOrbit : IOrbit
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="centre">Orbital centre</param>
		/// <param name="radius">Orbital radius</param>
		/// <param name="year">Time taken for a complete orbit</param>
		public CircularOrbit( IBody centre, double radius, TimeSpan year )
		{
			m_Centre = centre;
			m_Radius = radius;
			m_AnglePerSecond = Math.PI * 2.0 / year.TotalSeconds;
		}

		/// <summary>
		/// Entity at the centre of the orbit
		/// </summary>
		public IBody Centre
		{
			get { return m_Centre; }
		}

		/// <summary>
		/// Radius of the orbit
		/// </summary>
		public double Radius
		{
			get { return m_Radius; }
		}

		#region IOrbit Members

		/// <summary>
		/// Updates the transform of an orbiting entity
		/// </summary>
		/// <param name="entity">Orbiting entity</param>
		/// <param name="updateTime">Update time, in ticks (<see cref="TinyTime"/>)</param>
		public void Update( IBody entity, long updateTime )
		{
			double angleIncrement = m_AnglePerSecond * TinyTime.ToSeconds( m_LastUpdate, updateTime );
			m_Angle = Utils.Wrap( m_Angle + angleIncrement, 0, Math.PI * 2.0 );

			double x = Math.Sin( m_Angle ) * m_Radius;
			double z = Math.Cos( m_Angle ) * m_Radius;

			entity.Transform.Position.X = m_Centre.Transform.Position.X + Units.Convert.MetresToUni( x );
			entity.Transform.Position.Y = m_Centre.Transform.Position.Y;
			entity.Transform.Position.Z = m_Centre.Transform.Position.Z + Units.Convert.MetresToUni( z );

			m_LastUpdate = updateTime;
		}

		#endregion

		#region Private Members

		private readonly IBody m_Centre;
		private readonly double m_Radius;
		private double m_Angle;
		private readonly double m_AnglePerSecond;
		private long m_LastUpdate;

		#endregion
	}
}
