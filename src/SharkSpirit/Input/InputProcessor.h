#pragma once

#include "Logger/Logger.h"
#include "Core/IInitializable.h"
#include "Keyboard.h"
#include <Input/Mouse.h>
#include <Platform/Window/Window.h>

namespace SharkSpirit
{
	class input_processor : public IInitializable
	{
	public:
		input_processor(window_info* info) : m_keyboard(keyboard()), m_mouse(mouse())
		{
			m_windowInfo = info;
		};

		virtual ~input_processor() = default;

		virtual void Initialize()
		{
			Logger::LogInfo("Initialize Input Processor");
		}

		bool process_input();


	private:
		window_info* m_windowInfo;
		keyboard m_keyboard;
		mouse m_mouse;
	};
}