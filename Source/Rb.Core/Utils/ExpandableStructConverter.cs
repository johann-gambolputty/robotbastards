using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Rb.Core.Utils
{
	public class ExpandableStructConverter : ExpandableObjectConverter
	{
		public override bool GetCreateInstanceSupported( ITypeDescriptorContext context )
		{
			return true;
		}

		public override object CreateInstance( ITypeDescriptorContext context, IDictionary propertyValues )
		{
			Type type = context.PropertyDescriptor.PropertyType;
			object obj = Activator.CreateInstance( type );
			foreach ( DictionaryEntry entry in propertyValues )
			{
				PropertyInfo property = type.GetProperty( ( string )entry.Key );
				property.SetValue( obj, entry.Value, null );
			}

			return obj;
		}
	}
}
