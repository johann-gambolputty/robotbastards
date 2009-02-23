namespace Rb.Core.Components.Generic
{
	/// <summary>
	/// Typed template type
	/// </summary>
	/// <typeparam name="T">Model template type supported by this class</typeparam>
	public class ComponentType<T> : ComponentType
		where T : new( )
	{
		public ComponentType( )
			: base( typeof( T ) )
		{
		}
	}
}
