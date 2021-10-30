#include <sstream>
#include "SSException.h"

SSException::SSException(int line, const char* file) noexcept
	:
	line(line),
	file(file)
{}

const char* SSException::what() const noexcept
{
	std::ostringstream oss;
	oss << GetType() << std::endl
		<< GetOriginString();
	whatBuffer = oss.str();
	return whatBuffer.c_str();
}

const char* SSException::GetType() const noexcept
{
	return "Exception";
}

int SSException::GetLine() const noexcept
{
	return line;
}

const std::string& SSException::GetFile() const noexcept
{
	return file;
}

std::string SSException::GetOriginString() const noexcept
{
	std::ostringstream oss;
	oss << "[File] " << file << std::endl
		<< "[Line] " << line;
	return oss.str();
}