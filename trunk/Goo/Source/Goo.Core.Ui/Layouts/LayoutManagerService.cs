using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Ui.Layouts
{
	/// <summary>
	/// Layout manager service implementatio
	/// </summary>
	public class LayoutManagerService : ILayoutManagerService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="services">Service provider</param>
		public LayoutManagerService( IServiceProvider services )
		{
			Arguments.CheckNotNull( services, "services" );
			m_Services = services;
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		#region ILayoutManagerService Members

		/// <summary>
		/// Loads layouts
		/// </summary>
		public void LoadLayouts( Stream stream )
		{
			LayoutData[] layouts = ( LayoutData[] )( new BinaryFormatter( ).Deserialize( stream ) );
			foreach ( LayoutData layout in layouts )
			{
				LoadLayout( layout );
			}
		}

		/// <summary>
		/// Saves layouts
		/// </summary>
		public void SaveLayouts( Stream stream )
		{
			List<LayoutData> layouts = new List<LayoutData>( );
			foreach ( object service in m_Services.Services )
			{
				ILayoutSerializerService serializerService = service as ILayoutSerializerService;
				if ( serializerService == null )
				{
					continue;
				}
				LayoutData layout = SaveLayout( serializerService );
				if ( layout != null )
				{
					layouts.Add( layout);
				}
			}
			BinaryFormatter formatter = new BinaryFormatter( );
			formatter.Serialize( stream, layouts.ToArray( ) );
		}

		#endregion

		#region Private Members

		private readonly ILog m_Log;
		private readonly IServiceProvider m_Services;

		[Serializable]
		private class LayoutData
		{
			public byte[] Data
			{
				get { return m_Data; }
				set { m_Data = value; }
			}

			public Type LayoutServiceType
			{
				get { return m_LayoutServiceType; }
				set { m_LayoutServiceType = value; }
			}

			private Type m_LayoutServiceType;
			private byte[] m_Data;
		}

		/// <summary>
		/// Loads a layout
		/// </summary>
		private void LoadLayout( LayoutData layout )
		{
			foreach ( object service in m_Services.Services )
			{
				if ( service.GetType( ) == layout.LayoutServiceType )
				{
					using ( MemoryStream ms = new MemoryStream( layout.Data ) )
					{
						( ( ILayoutSerializerService )service ).Load( ms );
						return;
					}
				}
			}
			m_Log.Error( "Could not find service that could load layout intended for a " + layout.LayoutServiceType );
		}

		/// <summary>
		/// Creates a LayoutData object containing the saved results of a serializerService
		/// </summary>
		private static LayoutData SaveLayout( ILayoutSerializerService serializerService )
		{
			using ( MemoryStream ms = new MemoryStream( ) )
			{
				if ( !serializerService.Save( ms ) )
				{
					return null;
				}
				LayoutData layout = new LayoutData( );
				layout.Data = ms.ToArray( );
				layout.LayoutServiceType = serializerService.GetType( );
				return layout;
			}
		}
		
		#endregion
	}
}
