#include "Platform/Window/Window.h"
#include "Core/Application.h"
#include "Core/ECS/Components/Components.h"
#include "Components/GameComponents.h"
#include <Core/ECS/Systems/SpriteRenderSystem.h>
#include "Components/PlayerInputComponent.h"
#include "Systems/PlayerInputSystem.h"
#include <ios>
#include <cstdio>
#include <io.h>
#include <fcntl.h>
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>

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
		m_player_input_system->~player_input_system();
		m_reg.clear();
	}

protected:
	void on_create() override
	{
		DirectX::XMFLOAT3 pos = { 1000, 1000, 0 };
		DirectX::XMFLOAT3 rot = { 0, 0, 0 };
		DirectX::XMFLOAT2 scale = { 64, 64 };

		const std::string& playerTextureName = "survivor-idle_rifle_0";
		const std::string& grassTextureName = "oryx_16bit_fantasy_world_65";

	    m_assets.load_texture(&m_graphics, playerTextureName, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\survivor-idle_rifle_0.png");
		m_assets.load_texture(&m_graphics, grassTextureName, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\oryx_16bit_fantasy_world_65.png");
		
		const std::wstring& pixelShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\ps_2d.cso";
		const std::wstring& vertexShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\vs_2d.cso";

		auto playerSpriteCreateInfo = sprite_component_create_info(playerTextureName, pixelShader, vertexShader);
		auto grassSpriteCreateInfo = sprite_component_create_info(grassTextureName, pixelShader, vertexShader);

		player = create_entity();
		m_reg.emplace<transform_component>(player, pos, rot, scale);
		m_reg.emplace<player_input_component>(player, 0.3f);
		m_reg.emplace<sprite_component>(player, &m_assets, &m_graphics, &playerSpriteCreateInfo);

		auto grass = create_entity();
		auto tmp = m_reg.emplace<sprite_component>(grass, &m_assets, &m_graphics, &grassSpriteCreateInfo);

		for (size_t x = 0; x < 2560; x += 256)
		{
			for (size_t y = 0; y < 2560; y += 256)
			{
				grass = m_reg.create();
				DirectX::XMFLOAT3 g_pos = { (float)x, (float)y, 0 };
				DirectX::XMFLOAT3 g_rot = { 0, 0, 0 };
				DirectX::XMFLOAT2 g_sc = { 256, 256 };
				m_reg.emplace<transform_component>(grass, g_pos, g_rot, g_sc);
				m_reg.emplace<sprite_component>(grass, tmp);
			}
		}

		m_player_input_system = new player_input_system(&m_reg, &m_input, &m_graphics);
		m_sprite_render_system = new sprite_render_system(&m_reg, &m_input, &m_graphics);
	}

	void on_update() override 
	{
		m_player_input_system->run();
		m_sprite_render_system->run();
	}

private:
	entt::entity player;
	player_input_system* m_player_input_system;
	sprite_render_system* m_sprite_render_system;
};

int APIENTRY wWinMain(
	_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE prevInstance,
	_In_ LPWSTR lpCmdLine,
	_In_ int nCmdShow)	
{
	try
	{
		AllocConsole();
		freopen("CONIN$", "r", stdin);
		freopen("CONOUT$", "w", stderr);
		freopen("CONOUT$", "w", stdout);

		_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);


		HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
		if (FAILED(hr))
		{
			Logger::LogWarning("CoInitializeEx FAILED");
		}

		const wchar_t* title = L"Top Down";

		auto windowCreateInfo = window_creation_info(1080, 1920, title, title, hInstance);
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