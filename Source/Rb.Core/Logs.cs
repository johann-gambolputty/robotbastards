using Rb.Log;

namespace Rb.Core
{
	/// <summary>
	/// Static log class for maths
	/// </summary>
	/// <example>
	/// MathsLog.Error( "No tea" );
	/// </example>
	public class MathsLog : StaticTag< MathsLog >
	{
		public override string TagName
		{
			get { return "Maths"; }
		}
	}

	/// <summary>
	/// Static log class for components
	/// </summary>
	/// <example>
	/// ComponentLog.Error( "No tea" );
	/// </example>
	public class ComponentLog : StaticTag< ComponentLog >
	{
		public override string TagName
		{
			get { return "Components"; }
		}
	}

	/// <summary>
	/// Static log class for object sets
	/// </summary>
	/// <example>
	/// ObjectSetLog.Error( "No tea" );
	/// </example>
	public class ObjectSetLog : StaticTag<ObjectSetLog>
	{
		public override string TagName
		{
			get { return "ObjectSets"; }
		}
	}
}
