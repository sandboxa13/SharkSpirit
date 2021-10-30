#pragma once
#include <string>

std::wstring string_to_wide(std::string str)
{
	std::wstring wide_string(str.begin(), str.end());
	return wide_string;
}