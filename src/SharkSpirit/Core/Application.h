#pragma once

#include "Logger/Logger.h"
#include "Render/RenderEngine.h"
#include "Input/InputProcessor.h"
#include "Core/Timer/Timer.h"
#include "IInitializable.h"
#include "Platform/Window/Window.h"

class application 
{
public:
	application(SharkSpirit::window_info* windowInfo)
		: m_input(SharkSpirit::input_processor(windowInfo)),
		  m_timer(SharkSpirit::Timer()),
		  m_isRunning(false)
	{
		on_create();
	}

	bool run()
	{
		m_isRunning = true;

		while (m_isRunning)
		{
			m_isRunning = m_input.process_input();
			m_timer.Tick();

			on_update();
		}

		return m_isRunning;
	}

	void stop()
	{
		m_isRunning = false;
	}

protected:
	SharkSpirit::input_processor m_input;
	SharkSpirit::Timer m_timer;

	virtual void on_create() 
	{

	}

	virtual void on_update()
	{

	}

private:
	bool m_isRunning;
};