#pragma once

#include "Logger/Logger.h"
#include "Core/IInitializable.h"
#include "Keyboard.h"

namespace SharkSpirit
{
	class input_processor : public IInitializable
	{
	public:
		input_processor(HWND windowHandle) : m_keyboard(keyboard())
		{
			this->windowHandle = windowHandle;
		};

		virtual ~input_processor() = default;

		virtual void Initialize()
		{
			Logger::LogInfo("Initialize Input Processor");
		}

		bool process_input();


	private:
		HWND windowHandle;
		keyboard m_keyboard;
	};
}