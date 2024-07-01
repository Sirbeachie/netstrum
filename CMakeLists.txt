cmake_minimum_required(VERSION 3.10)

project(netstrum)

include_directories(
    ${Boost_INCLUDE_DIR}
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/logging/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/communication/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/event/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/observer/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/command/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/core/hooking/
    ${CMAKE_CURRENT_SOURCE_DIR}/src/dll/
)

find_package(Boost COMPONENTS log system REQUIRED)

add_library(netstrum_dll SHARED
    src/core/event/Event.cpp
    src/core/event/LogEvent.cpp
    src/core/observer/Observer.cpp
    src/core/logging/LoggingService.cpp
    src/core/communication/NamedPipe.cpp
    src/core/command/Command.cpp
    src/core/command/RegisterObserverCommand.cpp
    src/core/command/SetLoggingLevelCommand.cpp
    src/core/hooking/DetourManager.cpp
    src/dll/PacketInstrumentationDLL.cpp
)

# find_package(Detours REQUIRED)
# target_link_libraries(netstrum_dll Detours)

target_link_libraries(netstrum_dll ${Boost_LIBRARIES})

install(TARGETS netstrum_dll DESTINATION bin)

# add_subdirectory(tests)