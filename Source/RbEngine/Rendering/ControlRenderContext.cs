using System;
using System.Windows.Forms;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Abstract base class, used to determine a control's setup requirements for rendering
	/// </summary>
	/// <seealso cref="RbEngine.Rendering.Renderer.CreateControlContext"/>
	public abstract class ControlRenderContext
	{
			/// <summary>
			/// Returns the class styles required by the owner window
			/// </summary>
			/// <returns></returns>
			public virtual Int32					ClassStyles
			{
				get
				{
					const Int32 CS_VREDRAW	= 0x1;
					const Int32 CS_HREDRAW	= 0x2;
					const Int32 CS_OWNDC	= 0x20;
					return CS_VREDRAW | CS_HREDRAW | CS_OWNDC;
				}
			}

			/// <summary>
			/// Returns control styles that the control should apply
			/// </summary>
			public virtual ControlStyles			AddStyles
			{
				get
				{
					return	ControlStyles.AllPaintingInWmPaint	|
							ControlStyles.Opaque				|
							ControlStyles.ResizeRedraw			|
							ControlStyles.UserPaint;
				}
			}

			/// <summary>
			/// Returns control styles that the control should remove
			/// </summary>
			public virtual ControlStyles			RemoveStyles
			{
				get
				{
					return ControlStyles.DoubleBuffer;
				}
			}

			/// <summary>
			/// Creates an image that is displayed in this control in design mode
			/// </summary>
			/// <returns></returns>
			public abstract System.Drawing.Image	CreateDesignImage( );

			/// <summary>
			/// Creates the rendering context. Called when the control Load event fires
			/// </summary>
			public abstract void					Create( Control control, byte colourBits, byte depthBits, byte stencilBits );

			/// <summary>
			/// Called by the paint event handler prior to any rendering
			/// </summary>
			public virtual bool						BeginPaint( Control control )
			{
				Renderer.Inst.CurrentControl = control;
				return true;
			}

			/// <summary>
			/// Called when painting has finished
			/// </summary>
			public abstract void					EndPaint( );
	}
}
