﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Poc0.LevelEditor.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Poc0.LevelEditor.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Icon AppIcon {
            get {
                object obj = ResourceManager.GetObject("AppIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string ErrorCaption {
            get {
                return ResourceManager.GetString("ErrorCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to export scene to file &quot;{0}&quot;.
        /// </summary>
        internal static string FailedToExportScene {
            get {
                return ResourceManager.GetString("FailedToExportScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to open scene file &quot;{0}&quot;.
        /// </summary>
        internal static string FailedToOpenScene {
            get {
                return ResourceManager.GetString("FailedToOpenScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to save scene to file &quot;{0}&quot;.
        /// </summary>
        internal static string FailedToSaveScene {
            get {
                return ResourceManager.GetString("FailedToSaveScene", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No asset location managers with UI were available.
        /// </summary>
        internal static string NoLocationManagersWithUI {
            get {
                return ResourceManager.GetString("NoLocationManagersWithUI", resourceCulture);
            }
        }
    }
}
