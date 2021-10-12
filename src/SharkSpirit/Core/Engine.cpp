#include "Engine.h"

namespace SharkSpirit 
{
	std::unique_ptr<Engine> Engine::CreateEngine(std::unique_ptr<EngineConfiguration> engineConfiguration)
	{
		auto engine = std::make_unique<Engine>();
		engine->EngineConfig = std::move(engineConfiguration);
		engine->Initialize();

		Logger::LogInfo("Engine Instance Created");

		return engine;
	}

	std::unique_ptr<WindowConfiguration> Engine::CreateSSWindow(std::unique_ptr<WindowConfiguration> windowConfig)
	{
		Logger::LogInfo("CREATING WINDOW");

		windowConfig->Window = std::make_unique<Window>(windowConfig->Title, windowConfig->ClassName, windowConfig->Width, windowConfig->Height, windowConfig->Hinstance);

		auto& window = windowConfig->Window;

		auto wndProc = [&window](UINT msg, WPARAM wParam, LPARAM lParam) -> LRESULT
		{
			UINT width = {};
			UINT height = {};

			switch (msg)
			{
			case WM_SIZE:

				width = LOWORD(lParam);
				height = HIWORD(lParam);

				break;
			case WM_CLOSE:
				PostQuitMessage(0);
				break;

			case WM_PAINT:
				ValidateRect(window->GetHWND(), nullptr);
				break;
			case WM_MOUSEMOVE:

				break;
			}

			return DefWindowProc(window->GetHWND(), msg, wParam, lParam);
		};

		window->SetWndProc(wndProc);

		if ((windowConfig->Window) == nullptr)
		{
			Logger::LogError("CANNOT CREATE WINDOW");
		}

		return windowConfig;
	}

	bool Engine::Run()
	{
		Logger::LogInfo("ENGINE RUNNING...");

		_IsRunning = true;

		while (_IsRunning)
		{
			Input->ProcessInput();
			
			Timer->Tick();
		}

		return _IsRunning;
	}

	void Engine::Stop()
	{
		Logger::LogInfo("ENGINE STOPPED");

		_IsRunning = false;
	}
}