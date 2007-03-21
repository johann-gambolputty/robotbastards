using System;
using System.Reflection;
using System.Collections;


/*
 * Problem:
 * 
 * To be able to create objects that can determine context to satisfy internal dependencies.
 * 
 * For example:
 *	An AiObject 'A' needs to subscribe to the AI timer
 * 
 * Example solutions:
 * 
 *	This could be done manually by the code creating 'A', or, better yet, using a factory or builder pattern to create and set up 'A'. The problem with
 *	these solutions is that creation of 'A' becomes non-trivial. Directly supplying 'A's context implies that wherever an AiObject is created, the creating
 *	code must know about that context, which is a huge dependency issue. Using a factory, or builder, is much better, because the dependency is localised
 *	to a single function, but it means that 'A' can't be created with a simple "new AiObject()". The appropriate factory or builder must be selected and used.
 * 
 *	Alternatively, the creation context can be a singleton, which is the perfect case - it's totally unobtrusive. 'A' can get what it needs without any onus
 *	on the creator of 'A' passing it additional information. What if the creation context doesn't conform to the singleton pattern, though? What if there are
 *	20 different AI timers?
 * 
 *	The current solution in RB is aimed at a specific context dependency - that of an object to the scene in which it belongs. A scene contains a bunch
 *	of sub-systems (for example, AI timers), so it's a fairly generic solution. It's achieved by scene-dependent objects implementing the ISceneObject interface.
 *	The scene watches closely for new objects being added to the scene with that interface, and calls ISceneObject.AddedToScene() when appropriate.
 * 
 *	The problem with this is the "watches closely" part - the scene has to set up an elaborate series of callbacks to check whenever an object is added to the
 *	scene or whenever an object is added to an object that is already part of the scene. It's a mess, and there are ways to add objects (e.g. by setting properties)
 *	that the scene callback system is unaware of (and making it aware would be far too intrusive - the current system is horrible enough). That said, it's a good
 *	start because it's totally transparent - all 'A' has to do is implement ISceneObject, and, in most cases, it'll know when it has been added to the scene, and
 *	there's no wierdness on the part of the code creating 'A' - it just calls "new AiObject()", relates it to an existing object, and everything's OK.
 *	
 * There's a twist, though, related	
 *  
 */

namespace RbEngine.Components
{
	/// <summary>
	/// The Builder singleton is responsible for building components and objects
	/// </summary>
	public class Builder : IBuilder
	{
		/// <summary>
		/// Creates a new object of the specified type
		/// </summary>
		public virtual object	Create( Type objectType )
		{
			return System.Activator.CreateInstance( objectType );
		}

		/// <summary>
		/// Builds an existing object
		/// </summary>
		public virtual object	Build( object obj )
		{
			return obj;
		}

		/// <summary>
		/// Builder singleton access
		/// </summary>
		public static Builder	Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		private static Builder	ms_Singleton = new Builder( );

	}
}
