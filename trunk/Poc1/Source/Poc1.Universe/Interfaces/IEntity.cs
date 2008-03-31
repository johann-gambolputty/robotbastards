
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces
{
	/*
	 * Universe rendering:
	 *	- Render solar system objects
	 *		- Scale down all positions
	 *		- No Z writes/reads (all solar system objects must be convex and Z-sorted)
	 *		- If a solar system object is within Nkm of the camera, render it normally, in the next stage
	 *			(so there's no Z fighting with atmospheric effects and other objects)
	 *	- Render Nkm region around camera
	 *		- Z writes/reads on
	 *		- Solar system objects have a minimum radius of N/2km, so no objects behind a SSO will be rendered
	 * 
	 */


	public interface IEntity
	{
		UniPoint3 Position
		{
			get;
		}

		Vector3 XAxis
		{
			get;
		}

		Vector3 YAxis
		{
			get;
		}

		Vector3 ZAxis
		{
			get;
		}
	}
}
