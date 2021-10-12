#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include <ios>
#include <cstdio>
#include <io.h>
#include <fcntl.h>
#include "../SharkSpirit/Core/Engine.h"


int APIENTRY wWinMain(
	_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE prevInstance,
	_In_ LPWSTR lpCmdLine,
	_In_ int nCmdShow)
{
	AllocConsole();
	freopen("CONIN$", "r", stdin);
	freopen("CONOUT$", "w", stderr);
	freopen("CONOUT$", "w", stdout);

	HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
	if (FAILED(hr)) 
	{
		Logger::LogWarning("CoInitializeEx FAILED");
	}

	const wchar_t* title = L"Shark Spirit SANDBOX";

	auto windowConfiguration = WindowConfigurationFactory::CreateWindowConfiguration(1280, 720, title, title, hInstance);

	windowConfiguration = Engine::CreateSSWindow(std::move(windowConfiguration));

	ShowWindow(windowConfiguration->Window->GetHWND(), SW_SHOW);

	auto engineConfig = std::make_unique<EngineConfiguration>(std::move(windowConfiguration));

	auto engineInstance = Engine::CreateEngine(std::move(engineConfig));

	if (!engineInstance->Run())
	{
		windowConfiguration.release();
		engineConfig.release();
		engineInstance.release();
	}
	
	return 0;
}
