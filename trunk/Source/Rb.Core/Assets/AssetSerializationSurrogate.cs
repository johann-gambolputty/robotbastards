using System.Reflection;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

namespace Rb.Core.Assets
{
	public class AssetSurrogateSelector : ISurrogateSelector
	{
		public void Add( AssetHandle handle )
		{
			AssetSerializationSurrogate surrogate;
			if ( !m_TypeMap.TryGetValue( handle.Asset.GetType( ), out surrogate ) )
			{
				surrogate = new AssetSerializationSurrogate( );
				m_TypeMap.Add( handle.Asset.GetType( ), surrogate );
			}
			surrogate.Add( handle );
		}

		#region ISurrogateSelector Members

		public void ChainSelector( ISurrogateSelector selector )
		{
			m_Next = selector;
		}

		public ISurrogateSelector GetNextSelector( )
		{
			return m_Next;
		}

		public ISerializationSurrogate GetSurrogate( Type type, StreamingContext context, out ISurrogateSelector selector )
		{
			AssetSerializationSurrogate surrogate;
			if ( !m_TypeMap.TryGetValue( type, out surrogate ) )
			{
				selector = null;
				return m_Next == null ? null : m_Next.GetSurrogate( type, context, out selector );
			}
			selector = this;
			return surrogate;
		}

		#endregion

		private ISurrogateSelector m_Next;
		private readonly Dictionary< Type, AssetSerializationSurrogate > m_TypeMap = new Dictionary< Type, AssetSerializationSurrogate >( );
	}

	public class AssetSerializationSurrogate : ISerializationSurrogate
	{
		public void Add( AssetHandle handle )
		{
			m_Assets.Add( handle );
		}

		private List< AssetHandle > m_Assets = new List< AssetHandle >( );

		#region ISerializationSurrogate Members

		public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
		{
			AssetHandle asset = FindAsset( obj );
			if ( asset == null )
			{
				SetFields( obj, info );
				return;
			}
		}

		public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
		{
			GetFields( obj, info );
			return obj;
		}

		#endregion

		private static void GetFields( object obj, SerializationInfo info )
		{
			//	TODO: AP: This is very inefficient - ideally, SetObjectData() should be able to return some value
			//	to indiciate to the serializer that the object should be serialized without using the surrogate
			foreach ( FieldInfo field in obj.GetType( ).GetFields( ) )
			{
				field.SetValue( obj, info.GetValue( field.Name, field.FieldType ) );
			}
		}

		private static void SetFields( object obj, SerializationInfo info )
		{
			//	TODO: AP: This is very inefficient - ideally, GetObjectData() should be able to return some value
			//	to indiciate to the serializer that the object should be serialized without using the surrogate
			foreach ( FieldInfo field in obj.GetType( ).GetFields( ) )
			{
				info.AddValue( field.Name, field.GetValue( obj ) );
			}
		}

		private AssetHandle FindAsset( object obj )
		{
			foreach ( AssetHandle asset in m_Assets )
			{
				if ( ReferenceEquals( asset.Asset, obj ) )
				{
					return asset;
				}
			}
			return null;
		}

		private class SaveAsAsset
		{
			
		}

		private class SaveAsObject
		{
			
		}
	}
}
