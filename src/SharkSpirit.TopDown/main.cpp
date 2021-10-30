#include "Platform/Window/Window.h"
#include "Core/Application.h"
#include "Core/ECS/Components/Components.h"
#include "Components/GameComponents.h"
#include <Core/ECS/Systems/SpriteRenderSystem.h>
#include "Components/PlayerInputComponent.h"
#include "Systems/PlayerInputSystem.h"

using namespace SharkSpirit;

class top_down_game : public application
{
public:
	top_down_game(application_create_info* applicationCreateInfo) : application(applicationCreateInfo)
	{

	}

	~top_down_game()
	{
		stop();
		m_player_input->~player_input_system();
		m_reg.clear();
	}

protected:
	void on_create() override
	{
		DirectX::XMFLOAT3 pos = { 0, 0, 0 };
		DirectX::XMFLOAT3 rot = { 0, 0, 0 };
		DirectX::XMFLOAT2 scale = { 64, 64 };

		player = m_reg.create();
		m_reg.emplace<transform_component>(player, pos, rot, scale);
		m_reg.emplace<player_input_component>(player, 0.1f);
		m_reg.emplace<sprite_component>(player, &m_graphics, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\survivor-idle_rifle_0.png");

		m_player_input = new player_input_system(&m_reg, &m_input, &m_graphics);
	}

	void on_update() override 
	{
		m_player_input->run();

		auto spriteView = m_reg.group<sprite_component, transform_component>();
		for (auto entity : spriteView)
		{
			sprite_render_system::render_sprite(
				&m_graphics, 
				&m_input,
				spriteView.get<sprite_component>(entity),
				spriteView.get<transform_component>(entity));
		}
	}

private:
	entt::entity player;
	player_input_system* m_player_input;
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

		application.~application();

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