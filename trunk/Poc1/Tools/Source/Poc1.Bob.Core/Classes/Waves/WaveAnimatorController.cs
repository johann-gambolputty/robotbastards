using System;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;
using Rb.Core.Threading;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Waves
{
	/// <summary>
	/// Creates a controller for a wave animator view (<see cref="IWaveAnimatorView"/>)
	/// </summary>
	public class WaveAnimatorController : IProgressMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="uiProvider">UI provider. If null, no error messages are shown to the user</param>
		/// <param name="view">View to control</param>
		/// <param name="model">Animation model</param>
		public WaveAnimatorController( IMessageUiProvider uiProvider, IWaveAnimatorView view, WaveAnimationParameters model ) :
			this( uiProvider, view, model, ExtendedThreadPool.Instance )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="uiProvider">UI provider. If null, no error messages are shown to the user</param>
		/// <param name="view">View to control</param>
		/// <param name="model">Animation model</param>
		/// <param name="workQueue">Work queue</param>
		public WaveAnimatorController( IMessageUiProvider uiProvider, IWaveAnimatorView view, WaveAnimationParameters model, IWorkItemQueue workQueue )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( model, "model" );
			Arguments.CheckNotNull( workQueue, "workQueue" );

			view.Model = model;
			view.GenerateAnimation += OnGenerateAnimation;

			m_View = view;
			m_WorkQueue = workQueue;
			m_Marshaller = new DelegateMarshaller( );
			m_UiProvider = uiProvider;
		}


		#region Private Members

		private readonly IMessageUiProvider m_UiProvider;
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
			m_WorkQueue.Enqueue( builder.Build( ), this );
		}

		#endregion



		#region IProgressMonitor Members

		/// <summary>
		/// Sets the progress in the associated view (<see cref="IWaveAnimatorView.WaveAnimationGenerationProgress"/>)
		/// </summary>
		/// <param name="progress"></param>
		/// <returns></returns>
		public bool UpdateProgress( float progress )
		{
			m_Marshaller.PostAction( delegate { m_View.WaveAnimationGenerationProgress = progress; } );
			return true;
		}

		/// <summary>
		/// Handles exceptions thrown by during the animation process
		/// </summary>
		public void WorkFailed( object failureInfo )
		{
			m_View.WaveAnimationGenerationProgress = 0;

			if ( m_UiProvider != null )
			{
				m_UiProvider.ShowError( failureInfo as Exception, "Error occurred while generating wave animation" );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void WorkComplete( )
		{
			//	Nothing needs to be done here - the worker has already marshalled a call
			//	to the view, instructing it to show the animation.
			//	TODO: AP: To do this properly, use a source-sink delegate/add a new work item type
			//	that shares a state object
		}

		#endregion
	}
}
