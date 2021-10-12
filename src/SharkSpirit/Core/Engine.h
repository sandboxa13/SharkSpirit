#pragma once

#include "../Logger/Logger.h"
#include "../Render/RenderEngine.h"
#include "Window.h"
#include <cassert>



class EngineConfiguration
{
public:
	EngineConfiguration(std::unique_ptr<WindowConfiguration> windowConfiguration) : WindowConfiguration(std::move(windowConfiguration))
	{

	};
	virtual ~EngineConfiguration() = default;

	std::unique_ptr<WindowConfiguration> WindowConfiguration;
};

class Engine
{
public:
	Engine() : _IsRunning(false) {};
	virtual ~Engine() = default;

public:
	std::unique_ptr<EngineConfiguration> EngineConfig;


	static std::unique_ptr<Engine> CreateEngine(std::unique_ptr<EngineConfiguration> engineConfig);
	static std::unique_ptr<WindowConfiguration> CreateSSWindow(std::unique_ptr<WindowConfiguration> windowConfig);
	bool Run();
	void Stop();
private:
	bool _IsRunning;
	
};