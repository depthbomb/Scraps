﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scraps.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Scraps.Resources.Strings", typeof(Strings).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to Use of this tool is very much against Scrap.TF&apos;s community guidelines.
        ///
        ///There is a very real, albeit, low possibility that your Scrap.TF account can get banned.
        ///
        ///If you value your Scrap.TF account then do not use this program (or at least use an alternative account).
        ///
        ///By continuing to use this tool, you take full responsibility of what may or may not happen to your Scrap.TF account.
        ///
        ///This message will only be displayed once..
        /// </summary>
        internal static string DisclaimerText {
            get {
                return ResourceManager.GetString("DisclaimerText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copyright (C) 2021 Caprine Logic
        ///
        ///This program is free software: you can redistribute it and/or modify
        ///it under the terms of the GNU General Public License as published by
        ///the Free Software Foundation, either version 3 of the License, or
        ///(at your option) any later version.
        ///
        ///This program is distributed in the hope that it will be useful,
        ///but WITHOUT ANY WARRANTY; without even the implied warranty of
        ///MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        ///GNU General Public License for more de [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LicenseText {
            get {
                return ResourceManager.GetString("LicenseText", resourceCulture);
            }
        }
    }
}
