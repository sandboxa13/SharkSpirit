#include "Platform/Window/Window.h"
#include "Core/Application.h"
using namespace SharkSpirit;

class TopDownApplication : public application
{
public:
	TopDownApplication(SharkSpirit::window_info* windowInfo) : application(windowInfo)
	{

	}

protected:
	void on_create() override
	{

	}

	void on_update() override 
	{

	}
};

int APIENTRY wWinMain(
	_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE prevInstance,
	_In_ LPWSTR lpCmdLine,
	_In_ int nCmdShow)
{
	HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
	if (FAILED(hr)) 
	{
		Logger::LogWarning("CoInitializeEx FAILED");
	}

	const wchar_t* title = L"Top Down";

	auto windowCreateInfo = window_creation_info(720, 1280, title, title, hInstance);
	auto window = window_factory::create_window(&windowCreateInfo);
	
	auto application = TopDownApplication(window->get_window_info());

	window->show();

	application.run();

	return 0;
}