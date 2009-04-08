using System;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Keeps track of all created render targets
	/// </summary>
	/// <remarks>
	/// For diagnostic purposes only. Although this list is maintained in release builds, it shouldn't be
	/// used for anything other than debugging.
	/// </remarks>
	public static class RenderTargets
	{
		/// <summary>
		/// Event raised when a render target is added to the list
		/// </summary>
		public static Action<IRenderTarget> RenderTargetAdded;

		/// <summary>
		/// Event raised when a render target is removed to the list
		/// </summary>
		public static Action<IRenderTarget> RenderTargetRemoved;

		/// <summary>
		/// Adds a created render target to the created render target list
		/// </summary>
		/// <param name="renderTarget">Created render target</param>
		public static void AddCreatedRenderTarget( IRenderTarget renderTarget )
		{
			Arguments.CheckNotNull( renderTarget, "renderTarget" );
			lock ( s_RenderTargets )
			{
				s_RenderTargets.Add( renderTarget );
			}
			if ( RenderTargetAdded != null )
			{
				RenderTargetAdded( renderTarget );
			}
		}

		/// <summary>
		/// Removes a created render target from the created render target list
		/// </summary>
		/// <param name="renderTarget"></param>
		public static void RemoveCreatedRenderTarget( IRenderTarget renderTarget )
		{
			Arguments.CheckNotNull( renderTarget, "renderTarget" );
			lock ( s_RenderTargets )
			{
				s_RenderTargets.Remove( renderTarget );
			}
			if ( RenderTargetRemoved != null )
			{
				RenderTargetRemoved( renderTarget );
			}
		}

		/// <summary>
		/// Gets an array of all created render targets
		/// </summary>
		public static IRenderTarget[] Created
		{
			get
			{
				lock ( s_RenderTargets )
				{
					return s_RenderTargets.ToArray( );
				}
			}
		}

		#region Private Members

		private static List<IRenderTarget> s_RenderTargets = new List<IRenderTarget>( );

		#endregion
	}
}
