#include "Logger.h"

namespace sharkspirit::log
{
    void SetConsoleColor(ConsoleColor color)
    {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(h, ColorTable[(size_t)color]);
    }

    void Logger::LogInfo(std::string const& message)
    {
        SetConsoleColor(ConsoleColor::GREEN);

        LOG_INIT_COUT();

        log(LOG_INFO) << message << "\n";
    }

    void Logger::LogError(std::string const& error)
    {
        SetConsoleColor(ConsoleColor::RED);

        LOG_INIT_COUT();

        log(LOG_ERROR) << error << "\n";
    }

    void Logger::LogWarning(std::string const& warn)
    {
        SetConsoleColor(ConsoleColor::YELLOW);

        LOG_INIT_COUT();

        log(LOG_WARNING) << warn << "\n";
    }
}
