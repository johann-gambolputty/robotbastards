using System;
using System.Reflection;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Abstract planet environment model
	/// </summary>
	public class AbstractPlanetEnvironmentModel : IPlanetEnvironmentModel
	{

		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event raised when this model changes
		/// </summary>
		public event EventHandler<ModelChangedEventArgs> ModelChanged;

		/// <summary>
		/// Gets the planet that this model is attached to (via the planet model)
		/// </summary>
		public IPlanet Planet
		{
			get { return m_PlanetModel == null ? null : m_PlanetModel.Planet; }
		}

		/// <summary>
		/// Gets the planet model that this model is a part of
		/// </summary>
		public IPlanetModel PlanetModel
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
					m_PlanetModel.Remove( this );
					OnRemovedFromPlanetModel( m_PlanetModel );
				}
				m_PlanetModel = value;
				if ( m_PlanetModel != null )
				{
					if ( !m_PlanetModel.Components.Contains( this ) )
					{
						m_PlanetModel.Add( this );
					}
					OnAddedToPlanetModel( m_PlanetModel );
				}
			}
		}

		/// <summary>
		/// Invokes the correct Visit() call on the specified visitor object
		/// </summary>
		/// <typeparam name="TReturn">Visitor return type</typeparam>
		/// <param name="visitor">Visitor interface to invoke</param>
		/// <returns>Returns the result of the visit call</returns>
		public TReturn InvokeVisit<TReturn>( IPlanetEnvironmentModelVisitor<TReturn> visitor )
		{
			try
			{
				return ( TReturn )visitor.GetType( ).InvokeMember( "Visit", BindingFlags.InvokeMethod, null, visitor, new object[] { this } );
			}
			catch ( TargetInvocationException ex )
			{
				throw ex.InnerException;
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the <see cref="ModelChanged"/> event
		/// </summary>
		protected void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, ModelChangedEventArgs.Empty );
			}
		}

		/// <summary>
		/// Raises <see cref="ModelChanged"/> with the specifed arguments
		/// </summary>
		/// <param name="args">Event arguments</param>
		protected void OnModelChanged( ModelChangedEventArgs args )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, args );
			}
		}

		/// <summary>
		/// Called after this environment model has been removed from the specified planet model
		/// </summary>
		/// <param name="model">Planet model (not null)</param>
		protected virtual void OnRemovedFromPlanetModel( IPlanetModel model )
		{
		}

		/// <summary>
		/// Called after this environment model has been added to the specified planet model
		/// </summary>
		/// <param name="model">Planet model (not null)</param>
		protected virtual void OnAddedToPlanetModel( IPlanetModel model )
		{
		}

		#endregion

		#region Private Members

		private IPlanetModel m_PlanetModel;

		#endregion
	}

}
