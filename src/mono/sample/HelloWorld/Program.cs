// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace HelloWorld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool isMono = typeof(object).Assembly.GetType("Mono.RuntimeStructs") != null;
            Console.WriteLine($"Hello World {(isMono ? "from Mono!" : "from CoreCLR!")}");
            Console.WriteLine(typeof(object).Assembly.FullName);
            Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly ());
            Console.WriteLine(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

            Console.WriteLine(typeof(Bar));

Bar b = new();

// However, since Bar implements IDynamicInterfaceCastable, GetInterfaceImplementation will be called.
IFoo bf = (IFoo)b;
// bf is a Bar.IFooImpl with a Bar 'this'

// Will call Bar.IFooImpl.CallMe()
Console.WriteLine(bf.CallMe(27));
        }
    }
interface IFoo
{
    int CallMe(int i);
}

class Bar : IDynamicInterfaceCastable
{
    // Call when cast is performed on an instance of Bar but the type isn't in Bar's metadata.
    public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
    {
        if (interfaceType.Value == typeof(IFoo).TypeHandle.Value)
            return typeof(IFooImpl).TypeHandle;

        return default;
    }

    public bool IsInterfaceImplemented(RuntimeTypeHandle iType, bool @throw)
    {
        var a = GetInterfaceImplementation(iType);
        if (a.Value == default && @throw)
        {
            var typeName = Type.GetTypeFromHandle(iType).FullName;
            throw new InvalidCastException($"Don't support {typeName}");
        }

        return a.Value != default;
    }

    // An "implemented" interface instance that will handle "this" of type "Bar".
    // Note that when this default interface implementation is called, the "this" will
    // be typed as a "IFooImpl".
    [DynamicInterfaceCastableImplementation]
    public interface IFooImpl : IFoo
    {
        int IFoo.CallMe(int i)
        {
            return (int) this.GetType().Name[0];
        }
    }
}
}
