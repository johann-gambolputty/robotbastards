using System;

namespace Rb.Rendering
{
	/// <summary>
	/// This attribute is used to associate classes as part of rendering library 
	/// </summary>
	/// <remarks>
	/// Classes that have this attribute (either directly, or inherited from a base class or interface)
	/// will be picked up by <see cref="Interfaces.IGraphicsFactory"/>, and can be created using <see cref="Interfaces.IGraphicsFactory.Create(Type)"/>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface )]
	public class RenderingLibraryTypeAttribute : Attribute
	{
	}
}
