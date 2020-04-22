// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include <stdio.h>
#include <stdint.h>
#include <xplatform.h>
#include <platformdefines.h>

struct Instance
{
    public: virtual int STDMETHODCALLTYPE TestArgs(int value)
    {
        return (this != nullptr) ? value : -value;
    }

    public: virtual int STDMETHODCALLTYPE Test()
    {
        return TestArgs(3);
    }
};

typedef int (Instance::* TestInstance)();
typedef int (Instance::* TestArgsInstance)(int);

extern "C" DLL_EXPORT int TestArgs(int value)
{
    return value;
}

extern "C" DLL_EXPORT int Test()
{
    return TestArgs(1);
}

extern "C" DLL_EXPORT Instance* NewInstance()
{
    return new Instance();
}

extern "C" DLL_EXPORT TestInstance GetTestInstance()
{
    return &Instance::Test;
}

extern "C" DLL_EXPORT TestArgsInstance GetTestArgsInstance()
{
    return &Instance::TestArgs;
}
