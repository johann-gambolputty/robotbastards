using System;
using System.Windows.Forms;

namespace RbEngine.Rendering
{
	public class ClientView
	{
	}



	/// <summary>
	/// Rendering context
	/// </summary>
	public class RenderContext
	{
		/// <summary>
		/// The control being rendered to
		/// </summary>
		public Control			Control
		{
			get
			{
				return m_Control;
			}
		}

		/// <summary>
		/// Client view being rendered
		/// </summary>
		public ClientView		ClientView
		{
			get
			{
				return m_View;
			}
		}

		/// <summary>
		/// Client being rendered
		/// </summary>
		public Network.Client	Client
		{
			get
			{
				return m_View.Client;
			}
		}

		/// <summary>
		/// The current camera
		/// </summary>
		public Cameras.CameraBase	Camera
		{
			get
			{
				return m_Camera;
			}
			set
			{
				m_Camera = value;
			}
		}

		/// <summary>
		/// Scene being rendered
		/// </summary>
		public Scene.SceneDb	Scene
		{
			get
			{
				return Client.Scene;
			}
		}

		/// <summary>
		/// Render clock time
		/// </summary>
		public long				RenderTime
		{
			get
			{
				return m_Time;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="renderTime"></param>
		/// <param name="view"></param>
		public RenderContext( long renderTime, ClientView view )
		{
			m_Time	= time;
			m_View	= view;
		}

		private Control				m_Control;
		private long				m_Time;
		private ClientView			m_View;
		private Cameras.CameraBase	m_Camera;
	}
}
