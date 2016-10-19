using UnityEngine;
using System;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.Examples {

	/// <summary>
	/// This example registers a method named sqrt() for use in Lua.
	/// </summary>
	public class LuaExample : MonoBehaviour {

		/// <summary>
		/// Starts the component by registering the sqrt() function.
		/// </summary>
		void Start() {
			// Alternate way to register: Lua.RegisterFunction("sqrt", this, this.GetType().GetMethod("sqrt"));
			Lua.RegisterFunction("sqrt", null, SymbolExtensions.GetMethodInfo(() => sqrt(0)));
		}
	
		/// <summary>
		/// This is an example function implemented in C# and made available to Lua using Lua.RegisterFunction().
		/// </summary>
		/// <returns>
		/// The square root of x.
		/// </returns>
		/// <param name='x'>
		/// A float value.
		/// </param>
		public static float sqrt(float x) {
			return Mathf.Sqrt(x);
		}
		
	}

}
