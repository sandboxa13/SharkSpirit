#pragma once

#include "BSlogger.hpp"
#include <stdlib.h>
#include <Windows.h>
#include <cassert>

namespace SharkSpirit 
{
    enum class ConsoleColor : uint8_t
    {
        BLACK,
        DARK_BLUE,
        DARK_GREEN,
        DARK_CYAN,
        DARK_RED,
        DARK_MAGENTA,
        DARK_YELLOW,
        DARK_GRAY,
        GRAY,
        BLUE,
        GREEN,
        CYAN,
        RED,
        MAGENTA,
        YELLOW,
        WHITE,
    };

    constexpr WORD ColorTable[] =
    {
        0,
        FOREGROUND_BLUE,
        FOREGROUND_GREEN,
        FOREGROUND_GREEN | FOREGROUND_BLUE,
        FOREGROUND_RED,
        FOREGROUND_RED | FOREGROUND_BLUE,
        FOREGROUND_RED | FOREGROUND_GREEN,
        FOREGROUND_INTENSITY,
        FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE,
        FOREGROUND_INTENSITY | FOREGROUND_BLUE,
        FOREGROUND_INTENSITY | FOREGROUND_GREEN,
        FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE,
        FOREGROUND_INTENSITY | FOREGROUND_RED,
        FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_BLUE,
        FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN,
        FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE
    };

    class Logger
    {
    public:
        Logger() {};
        virtual ~Logger() = default;

        static void LogInfo(std::string const& message);
        static void LogError(std::string const& error);
        static void LogWarning(std::string const& warn);
    };
}