#pragma once

#include "Logger/Logger.h"
#include "Core/IInitializable.h"
#include "Keyboard.h"
#include <Input/Mouse.h>
#include <Platform/Window/Window.h>

namespace sharkspirit::input
{
	class input_processor : public sharkspirit::core::IInitializable
	{
	public:
		input_processor(sharkspirit::platform::window::window_info* info) : m_keyboard(keyboard()), m_mouse(mouse())
		{
			m_windowInfo = info;
		};

		virtual ~input_processor() = default;

		virtual void Initialize()
		{
			sharkspirit::logger::Logger::LogInfo("Initialize Input Processor");
		}

		bool process_input();

		keyboard m_keyboard;
		mouse m_mouse;
	private:
		sharkspirit::platform::window::window_info* m_windowInfo;
	};
}