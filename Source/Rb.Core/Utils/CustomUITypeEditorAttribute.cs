
using System;

namespace Rb.Core.Utils
{
	public class CustomUITypeEditorAttribute : Attribute
	{
		public CustomUITypeEditorAttribute( params Type[] supportedTypes )
		{
			m_SupportedTypes = supportedTypes;
		}

		public Type[] SupportedTypes
		{
			get { return m_SupportedTypes; }
		} 

		private readonly Type[] m_SupportedTypes;

	}
}
