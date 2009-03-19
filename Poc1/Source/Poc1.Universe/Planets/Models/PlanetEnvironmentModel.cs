using System;
using System.Reflection;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planet environment models
	/// </summary>
	public abstract class PlanetEnvironmentModel : IPlanetEnvironmentModel
	{
		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event raised when the model changes
		/// </summary>
		public event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet model composite that contains this model
		/// </summary>
		public virtual IPlanetModel PlanetModel
		{
			get { return m_PlanetModel; }
			set
			{
				if ( m_PlanetModel == value )
				{
					return;
				}
				if ( m_PlanetModel != null )
				{
					AssignToModel( m_PlanetModel, true );
				}
				m_PlanetModel = value;
				if ( m_PlanetModel != null )
				{
					AssignToModel( m_PlanetModel, false );
				}
			}
		}

		/// <summary>
		/// Visits this model
		/// </summary>
		/// <param name="visitor">Visitor to call back to</param>
		public T InvokeVisit<T>( IPlanetEnvironmentModelVisitor<T> visitor )
		{
			MethodInfo method = visitor.GetType( ).GetMethod( "Visit", new Type[] { GetType( ) } );
			if ( method == null )
			{
				throw new NotSupportedException( string.Format( "Unsupported Visit({0}) method", GetType( ) ) );
			}
			return ( T ) method.Invoke( visitor, new object[] { this } );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the planet that the planet model is attached to
		/// </summary>
		protected IPlanet Planet
		{
			get { return PlanetModel == null ? null : PlanetModel.Planet; }
		}

		/// <summary>
		/// Assigns the specified model to a planet model property
		/// </summary>
		protected abstract void AssignToModel( IPlanetModel planetModel, bool remove );

		/// <summary>
		/// Returns true if there are any subscribers to the <see cref="ModelChanged"/> event
		/// </summary>
		protected bool IsModelChangedUsed
		{
			get { return ModelChanged != null; }
		}

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with empty event args
		/// </summary>
		protected virtual void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event with specified event args
		/// </summary>
		protected virtual void OnModelChanged( EventArgs args )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, args );
			}
		}

		#endregion

		#region Private Members

		private IPlanetModel m_PlanetModel;

		#endregion
	}
}
