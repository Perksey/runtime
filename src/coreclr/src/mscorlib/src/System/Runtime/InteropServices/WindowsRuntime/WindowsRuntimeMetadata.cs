// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//

namespace System.Runtime.InteropServices.WindowsRuntime
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    
    internal static class WindowsRuntimeMetadata
    {
        private static EventHandler<DesignerNamespaceResolveEventArgs> DesignerNamespaceResolve;
        
        internal static string[] OnDesignerNamespaceResolveEvent(AppDomain appDomain, string namespaceName)
        {
            EventHandler<DesignerNamespaceResolveEventArgs> eventHandler = DesignerNamespaceResolve;
            if (eventHandler != null)
            {
                Delegate[] ds = eventHandler.GetInvocationList();
                int len = ds.Length;
                for (int i = 0; i < len; i++)
                {
                    DesignerNamespaceResolveEventArgs eventArgs = new DesignerNamespaceResolveEventArgs(namespaceName);

                    ((EventHandler<DesignerNamespaceResolveEventArgs>)ds[i])(appDomain, eventArgs);

                    Collection<string> assemblyFilesCollection = eventArgs.ResolvedAssemblyFiles;
                    if (assemblyFilesCollection.Count > 0)
                    {
                        string[] retAssemblyFiles = new string[assemblyFilesCollection.Count];
                        int retIndex = 0;
                        foreach (string assemblyFile in assemblyFilesCollection)
                        {
                            if (String.IsNullOrEmpty(assemblyFile))
                            {   // DesignerNamespaceResolve event returned null or empty file name - that is not allowed
                                throw new ArgumentException(Environment.GetResourceString("Arg_EmptyOrNullString"), "DesignerNamespaceResolveEventArgs.ResolvedAssemblyFiles");
                            }
                            retAssemblyFiles[retIndex] = assemblyFile;
                            retIndex++;
                        }

                        return retAssemblyFiles;
                    }
                }
            }
            
            return null;
        }
    }
    

    internal class DesignerNamespaceResolveEventArgs : EventArgs
    {
        private string _NamespaceName;
        private Collection<string> _ResolvedAssemblyFiles;

        public Collection<string> ResolvedAssemblyFiles
        {
            get
            {
                return _ResolvedAssemblyFiles;
            }
        }

        public DesignerNamespaceResolveEventArgs(string namespaceName)
        {
            _NamespaceName = namespaceName;
            _ResolvedAssemblyFiles = new Collection<string>();
        }
    }
}
