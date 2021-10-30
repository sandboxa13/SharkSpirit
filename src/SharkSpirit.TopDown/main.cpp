#include "Platform/Window/Window.h"
#include "Core/Application.h"
#include "Core/ECS/Components/Components.h"

using namespace SharkSpirit;

class top_down_game : public application
{
public:
	top_down_game(application_create_info* applicationCreateInfo) : application(applicationCreateInfo)
	{

	}

protected:
	void on_create() override
	{
		player = m_reg.create();
		m_reg.emplace<TransformComponent>(player);
		m_reg.emplace<SpriteComponent>(player);
	}

	void on_update() override 
	{
		auto transformView = m_reg.view<TransformComponent>();

		for (auto entity : transformView)
		{
			TransformComponent& transfrom = transformView.get<TransformComponent>(entity);
		}
	}

private:
	entt::entity player;
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
	auto windowInfo = window_factory::create_window(&windowCreateInfo);

	auto applicationCreateInfo = application_create_info(windowInfo);
	
	auto application = top_down_game(&applicationCreateInfo);

	application.show_window();
	application.run();

	return 0;
}