using System;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;
using Rb.Core.Threading;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Waves
{
	/// <summary>
	/// Creates a controller for a wave animator view (<see cref="IWaveAnimatorView"/>)
	/// </summary>
	public class WaveAnimatorController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View to control</param>
		/// <param name="model">Animation model</param>
		public WaveAnimatorController( IWaveAnimatorView view, WaveAnimationParameters model ) :
			this( view, model, ExtendedThreadPool.Instance )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View to control</param>
		/// <param name="model">Animation model</param>
		/// <param name="workQueue">Work queue</param>
		public WaveAnimatorController( IWaveAnimatorView view, WaveAnimationParameters model, IWorkItemQueue workQueue )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( model, "model" );
			Arguments.CheckNotNull( workQueue, "workQueue" );

			view.Model = model;
			view.GenerateAnimation += OnGenerateAnimation;

			m_View = view;
			m_WorkQueue = workQueue;
			m_Marshaller = new DelegateMarshaller( );
		}


		#region Private Members

		private readonly IWorkItemQueue m_WorkQueue;
		private readonly IWaveAnimatorView m_View;
		private readonly DelegateMarshaller m_Marshaller;

		/// <summary>
		/// Animation generator function
		/// </summary>
		private void GenerateAnimationWorker( IProgressMonitor monitor, WaveAnimationParameters model )
		{
			WaveAnimationGenerator generator = new WaveAnimationGenerator( );
			WaveAnimation animation = generator.GenerateHeightmapSequence( model, monitor );
			m_Marshaller.PostAction( m_View.ShowAnimation, animation );
		}

		/// <summary>
		/// Handles work completion
		/// </summary>
		private void OnWorkComplete( )
		{
			m_View.GenerationEnabled = true;
		}

		/// <summary>
		/// Handles work failure
		/// </summary>
		private void OnWorkFailed( Exception ex)
		{
			m_View.GenerationEnabled = true;
		}

		/// <summary>
		/// Generates an animation
		/// </summary>
		private void OnGenerateAnimation( )
		{
			if ( m_View.Model == null )
			{
				return;
			}
			m_View.GenerationEnabled = false;
			DelegateWorkItem.Builder builder = new DelegateWorkItem.Builder( );
			builder.SetDoWork( GenerateAnimationWorker, m_View.Model.Clone( ) );
			builder.SetWorkComplete( OnWorkComplete );
			builder.SetWorkFailed( OnWorkFailed );
			m_WorkQueue.Enqueue( builder.Build( ) );
		}

		#endregion


	}
}
