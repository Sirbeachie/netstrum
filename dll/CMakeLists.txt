cmake_minimum_required(VERSION 3.15)

project(netstrum_hook LANGUAGES CXX)

if(NOT WIN32)
    message(STATUS "netstrum_hook is only built on Windows; skipping.")
    return()
endif()

find_package(fmt CONFIG REQUIRED)
find_package(detours CONFIG REQUIRED)

add_library(netstrum_hook SHARED src/NetstrumHook.cpp)

set_target_properties(netstrum_hook PROPERTIES
    OUTPUT_NAME "netstrum_hook"
)

target_compile_features(netstrum_hook PRIVATE cxx_std_17)
target_link_libraries(netstrum_hook PRIVATE fmt::fmt detours ws2_32)
