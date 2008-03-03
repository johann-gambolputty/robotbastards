using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	public interface IProjectile
	{
		Point3 Position
		{
			get;
		}

		Vector3 Direction
		{
			get;
		}

		float Speed
		{
			get;
		}
	}
}
