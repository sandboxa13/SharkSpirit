#include "Platform/Window/Window.h"
#include "Core/Application.h"
#include "Core/ECS/Components/Components.h"
#include "Components/GameComponents.h"
#include <Core/ECS/Systems/SpriteRenderSystem.h>
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
		//m_reg.emplace<TransformComponent>(player);
		m_reg.emplace<sprite_component>(player, &m_graphics, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\seamless_grass.jpg");
		//m_reg.emplace<PlayerAtackRaduisComponent>(player);
	}

	void on_update() override 
	{
		auto spriteView = m_reg.view<sprite_component>();
		for (auto entity : spriteView)
		{
			//sprite_component& sprite = ;
			sprite_render_system::render_sprites(&m_graphics, spriteView.get<sprite_component>(entity));
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
	try
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
	catch (const SSException& ex)
	{
		MessageBoxA(nullptr, ex.what(), ex.GetType(), MB_OK | MB_ICONEXCLAMATION);
	}
	catch (const std::exception& ex)
	{
		MessageBoxA(nullptr, ex.what(), "Standart exception", MB_OK | MB_ICONEXCLAMATION);
	}
	catch (...)
	{
		MessageBoxA(nullptr, "WTF", "WTF exception", MB_OK | MB_ICONEXCLAMATION);
	}
}