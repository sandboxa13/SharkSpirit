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