#pragma once

#include "Logger/Logger.h"
#include "Core/IInitializable.h"

namespace SharkSpirit
{
	class InputProcessor : public IInitializable
	{
	public:
		InputProcessor(HWND windowHandle)
		{
			this->windowHandle = windowHandle;
		};

		virtual ~InputProcessor() = default;

		virtual void Initialize()
		{
			Logger::LogInfo("Initialize Input Processor");
		}

		void ProcessInput();

	private:
		HWND windowHandle;
	};
}