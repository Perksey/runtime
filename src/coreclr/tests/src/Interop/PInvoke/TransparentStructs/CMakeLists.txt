project (TransparentStructsNative)
include ("${CLR_INTEROP_TEST_ROOT}/Interop.cmake")
set(SOURCES
  TransparentStructsNative.cpp
)
add_library (TransparentStructsNative SHARED ${SOURCES})
install (TARGETS TransparentStructsNative DESTINATION bin)
