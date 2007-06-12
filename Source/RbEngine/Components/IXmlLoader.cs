using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Loads an object from XML
	/// </summary>
	/// <remarks>
	/// This is used by the EngineXmlLoader to handle elements that is does not understand while building an object.
	/// For example, given this XML:
	/// <code>
	/// <object type="MyApp.MyClass" name="foo">
	///		<object type="MyApp.TestClass" name="test"/>
	///		<fooCount value="10"/>
	/// </object>
	/// </code>
	/// The Components.XmlLoader will first create an instance of MyApp.MyClass, name it "foo" (using the INameObject interface).
	/// When it encounters the child "object" element, it will create an instance of MyApp.TestClass, name it "test", and
	/// add it to "foo" (using the IParentObject interface). The loader then encounters the "fooCount" element.  It does
	/// not know how to handle this, so defers to "foo"'s IXmlLoader implementation. If MyApp.MyClass does not implement
	/// the IXmlLoader interface, or it's ParseElement() call returns false, an RbXmlException will be thrown.
	/// </remarks>
	public interface IXmlLoader
	{
		/// <summary>
		/// Parses the XML element that was responsible for creating this object
		/// </summary>
		/// <param name="element"> XmlElement that generated this object </param>
		/// <returns>Returns true if the parse succeeded, otherwise false (exception thrown by loader)</returns>
		void ParseGeneratingElement( System.Xml.XmlElement element );

		/// <summary>
		/// Parses a given XML element
		/// </summary>
		/// <param name="element">Child XmlElement to the generating element</param>
		/// <returns>Returns true if the parse succeeded, otherwise false (exception thrown by loader)</returns>
		bool ParseElement( System.Xml.XmlElement element );
	}
}
