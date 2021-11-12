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
#include <Core/ECS/Systems/SpriteAnimationSystem.h>
#include "Systems/PlayerAnimationSystem.h"
#include <Core/ECS/Systems/SpriteLightRenderSystem.h>

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
		auto playerStartX = (float) 2560 / 2;
		auto playerStartY = (float) 2560 / 2;

		DirectX::XMFLOAT3 pos = { playerStartX, playerStartY, 0 };
		DirectX::XMFLOAT3 rot = { 0, 0, 0 };
		DirectX::XMFLOAT2 scale = { 128,  128 };

		const std::string& playerTextureName = "survivor-idle_rifle_0";
		const std::string& grassTextureName = "oryx_16bit_fantasy_world_65";
		const std::string& lightTextureName = "lightMask";
		std::vector<std::string> meleeAttackNames = {};
		std::vector<std::string> idleNames = {};
		std::vector<std::string> moveNames = {};
		std::vector<std::string> reloadNames = {};

		for (size_t i = 0; i < 15; i++)
		{
			const std::string name = std::format("survivor-meleeattack_rifle_{0}", i);
			meleeAttackNames.push_back(name);

			m_assets.load_texture(&m_graphics, name, std::format("C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\meleeattack\\survivor-meleeattack_rifle_{0}.png", i));
		}

		for (size_t i = 0; i < 20; i++)
		{
			const std::string name = std::format("survivor-idle_rifle_{0}", i);
			idleNames.push_back(name);

			m_assets.load_texture(&m_graphics, name, std::format("C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\idle\\survivor-idle_rifle_{0}.png", i));
		}

		for (size_t i = 0; i < 20; i++)
		{
			const std::string name = std::format("survivor-move_rifle_{0}", i);
			moveNames.push_back(name);

			m_assets.load_texture(&m_graphics, name, std::format("C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\move\\survivor-move_rifle_{0}.png", i));
		}

		for (size_t i = 0; i < 20; i++)
		{
			const std::string name = std::format("survivor-reload_rifle_{0}", i);
			reloadNames.push_back(name);

			m_assets.load_texture(&m_graphics, name, std::format("C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\reload\\survivor-reload_rifle_{0}.png", i));
		}

	    m_assets.load_texture(&m_graphics, playerTextureName, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\meleeattack\\survivor-meleeattack_rifle_0.png");
		m_assets.load_texture(&m_graphics, grassTextureName, "C:\\Repositories\\GitHub\\SharkSpirit\\src\\SharkSpirit.TopDown\\assets\\oryx_16bit_fantasy_world_65.png");
		m_assets.load_texture(&m_graphics, lightTextureName, "C:\\Repositories\\GitHub\\SharkSpirit\\assets\\textures\\light\\NaD6F.png");

		const std::wstring& playerPixelShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\ps_2d.cso";
		const std::wstring& pixelShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\ps_2d.cso";
		const std::wstring& pixellightShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\ps_light_2d.cso";
		const std::wstring& vertexShader = L"C:\\Repositories\\GitHub\\SharkSpirit\\assets\\shaders\\vs_2d.cso";

		auto playerSpriteCreateInfo = sprite_component_create_info(playerTextureName, playerPixelShader, vertexShader);
		auto lightSpriteCreateInfo = sprite_light_component_create_info(lightTextureName, pixellightShader, vertexShader);
		auto grassSpriteCreateInfo = sprite_component_create_info(grassTextureName, pixelShader, vertexShader);

		
		

		player = create_entity();
		m_reg.emplace<transform_component>(player, pos, rot, scale);
		m_reg.emplace<player_input_component>(player, 0.3f, 0.2f);
		m_reg.emplace<sprite_component>(player, &m_assets, &m_graphics, &playerSpriteCreateInfo);
		m_reg.emplace<sprite_light_component>(player, &m_assets, &m_graphics, &lightSpriteCreateInfo);

		for (size_t i = 0; i < 1; i++)
		{
			auto en = create_entity();
			m_reg.emplace<sprite_light_component>(en, &m_assets, &m_graphics, &lightSpriteCreateInfo);
			m_reg.emplace<transform_component>(en, pos, rot, scale);
		}

		auto& animation = m_reg.emplace<sprite_animation_component>(player);
		animation.add_animation("meleAtack", &meleeAttackNames, animation_type::once);
		animation.add_animation("reload", &reloadNames, animation_type::once);
		animation.add_animation("idle", &idleNames, animation_type::loop);
		animation.add_animation("move", &moveNames, animation_type::loop);

		animation.set_current_key("idle");

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

		m_player_input_system = new player_input_system(&m_reg, &m_input, &m_graphics, &m_assets);
		m_sprite_render_system = new sprite_render_system(&m_reg, &m_input, &m_graphics, &m_assets);
		m_sprite_animation_system = new sprite_animation_system(&m_reg, &m_input, &m_graphics, &m_assets);
		m_player_animation_system = new player_animation_system(&m_reg, &m_input, &m_graphics, &m_assets);
		m_sprite_light_render_system = new sprite_light_render_system(&m_reg, &m_input, &m_graphics, &m_assets);
		m_light_pass = new light_pass();
	}

	void on_update() override 
	{

		m_player_input_system->run();
		m_sprite_light_render_system->run();

		m_player_animation_system->run();
		m_sprite_animation_system->run();
		
		m_sprite_render_system->run();

		m_light_pass->execute(&m_graphics);

		float dt = m_timer.DeltaTime();
		float totalTime = m_timer.TotalTime();

		if (ImGui::Begin("Frame statistics :"))
		{
			ImGui::Text("FPS : %f", m_fps.get_fps());
			ImGui::Text("Delta time: %f", dt);
			ImGui::Text("Total time : %f", totalTime);
		}
	}

private:
	entt::entity player;
	player_input_system* m_player_input_system;
	sprite_render_system* m_sprite_render_system;
	sprite_animation_system* m_sprite_animation_system;
	player_animation_system* m_player_animation_system;
	sprite_light_render_system* m_sprite_light_render_system;
	light_pass* m_light_pass;
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

		//_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);

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