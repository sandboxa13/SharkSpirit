#pragma once

#include "../Logger/Logger.h"
#include "../Render/RenderEngine.h"
#include "Window.h"
#include <cassert>

class WindowConfiguration 
{
public:
	WindowConfiguration(int height, int width, const wchar_t* title, const wchar_t* class_name, HINSTANCE& hInstance)
		: Width(width), Height(height), Title(title), ClassName(class_name), Hinstance(hInstance)
	{
	};
	virtual ~WindowConfiguration() = default;

	int Width;
	int Height;
	const wchar_t* Title;
	const wchar_t* ClassName;
	HINSTANCE Hinstance;
	std::unique_ptr<SSWindow::Window> Window;
};

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