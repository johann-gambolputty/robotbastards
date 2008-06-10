using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Reflection;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Keeps track of any implementations of UITypeEditor
	/// </summary>
	public class UITypeEditorFactory
	{
		/// <summary>
		/// Gets the UITypeEditor type associated with a given type to edit
		/// </summary>
		public static Type GetCustomUITypeEditorType( Type typeToEdit )
		{
			if ( !m_CustomEditorTypes.ContainsKey( typeToEdit ) )
			{
				return null;
			}
			return m_CustomEditorTypes[ typeToEdit ];
		}

		/// <summary>
		/// Returns the first UI type editor type found that derived from the specified base type
		/// </summary>
		public static Type GetUITypeEditorTypeDerivedFrom( Type baseType )
		{
			foreach ( Type type in m_UITypeEditorTypes )
			{
				if ( type.IsSubclassOf( baseType ) )
				{
					return type;
				}
			}
			return null;
		}

		#region Private Members

		private readonly static Dictionary<Type, Type> m_CustomEditorTypes = new Dictionary<Type, Type>( );
		private readonly static List<Type> m_UITypeEditorTypes = new List<Type>( );

		static UITypeEditorFactory( )
		{
			foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				CheckAssemblyForUITypeEditorTypes( assembly );
			}

			AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
		}

		private static void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			CheckAssemblyForUITypeEditorTypes( args.LoadedAssembly );
		}

		private static void CheckAssemblyForUITypeEditorTypes( Assembly assembly )
		{
			foreach ( Type type in assembly.GetTypes( ) )
			{
				if ( !type.IsSubclassOf( typeof( UITypeEditor ) ) )
				{
					continue;
				}
				m_UITypeEditorTypes.Add( type );
				CustomUITypeEditorAttribute[] attrs = ( CustomUITypeEditorAttribute[] )type.GetCustomAttributes( typeof( CustomUITypeEditorAttribute ), true );
				if ( attrs.Length == 0 )
				{
					continue;
				}
				foreach ( Type supportedType in attrs[ 0 ].SupportedTypes )
				{
					m_CustomEditorTypes.Add( supportedType, type );
				}
			}
		} 
		#endregion
	}
}
