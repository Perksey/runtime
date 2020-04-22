// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

public struct S
{
    public int V;
}

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
[return: MarshalAs(UnmanagedType.TransparentStruct)]
public delegate S TestInstance(IntPtr pThis);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
[return: MarshalAs(UnmanagedType.TransparentStruct)]
public delegate S TestArgsInstance(IntPtr pThis, [MarshalAs(UnmanagedType.TransparentStruct)] S value);

unsafe partial class TransparentStructsTest
{
    [DllImport("TransparentStructsNative")]
    public static extern S Test();

    [DllImport("TransparentStructsNative")]
    public static extern S TestArgs(S value);

    [DllImport("TransparentStructsNative")]
    public static extern IntPtr NewInstance();

    [DllImport("TransparentStructsNative")]
    public static extern TestInstance GetTestInstance();

    [DllImport("TransparentStructsNative")]
    public static extern TestArgsInstance GetTestArgsInstance();

    public static int Main(string[] args)
    {
        try
        {
            Console.WriteLine("Testing Test()");
            if (Test().V != 1)
            {
                throw new Exception("Test() failed");
            }

            Console.WriteLine("Testing TestArgs(2)");
            if (TestArgs(new S() { V = 2 }).V != 2)
            {
                throw new Exception("TestArgs(2) failed");
            }

            var instance = NewInstance();

            Console.WriteLine("Testing TestInstance()");
            if (GetTestInstance()(instance).V != 3)
            {
                throw new Exception("TestInstance() failed");
            }

            Console.WriteLine("Testing TestArgsInstance(4)");
            if (GetTestArgsInstance()(instance, new S() { V = 4 }).V != 4)
            {
                throw new Exception("TestArgsInstance(4) failed");
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }
        return 100;
    }
}
