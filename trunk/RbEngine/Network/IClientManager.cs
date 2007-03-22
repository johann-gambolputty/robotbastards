using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Summary description for IClientManager.
	/// </summary>
	public interface IClientManager
	{
		IConnection ClientConnection
		{
			get;
			set;
		}
	}
}
