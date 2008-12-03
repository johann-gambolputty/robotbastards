
namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Implements IGraph2dData using a delegate to perform evaluation
	/// </summary>
	public class GraphX2dSourceFunction : GraphX2dSourceAbstract
	{
		/// <summary>
		/// Evaluation function delegate
		/// </summary>
		public delegate float EvaluateDelegate( float x );

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="minX">Minimum X value that can be passed into the function</param>
		/// <param name="minY">Minimum Y value that can be passed into the function</param>
		/// <param name="maxX">Maximum X value that can be passed into the function</param>
		/// <param name="maxY">Maximum Y value that can be passed into the function</param>
		/// <param name="evaluate">Evaluation function</param>
		public GraphX2dSourceFunction( float minX, float minY, float maxX, float maxY, EvaluateDelegate evaluate )
		{
			MinimumX = minX;
			MaximumX = maxX;
			MinimumY = minY;
			MaximumY = maxY;
			m_Evaluate = evaluate;
		}

		#region IGraph2dDataX Members

		/// <summary>
		/// Evaluates the graph function with input x
		/// </summary>
		public override float Evaluate( float x )
		{
			return m_Evaluate( x );
		}

		#endregion

		#region Private Members

		private readonly EvaluateDelegate m_Evaluate;

		#endregion
	}
}
