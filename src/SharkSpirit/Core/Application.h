#pragma once

#include "Logger/Logger.h"
#include "Input/InputProcessor.h"
#include "Core/Timer/Timer.h"
#include "IInitializable.h"
#include "Platform/Window/Window.h"
#include <Assets/AssetsManager.h>
#include <ImGui/ImGuiManager.h>
#include <Core/FpsManager.h>
#include <Render/Device.h>
#include <Render/RenderGraph.h>
#include "../../external/entt/entt.hpp"
#include <Core/ECS/Systems/SpriteUpdateSystem.h>
#include <Core/ECS/Systems/SpriteAnimationSystem.h>
#include <Core/ECS/Systems/SpriteLightUpdateSystem.h>

namespace sharkspirit::core
{
	class application_create_info 
	{
	public:
		application_create_info(sharkspirit::platform::window::window_info* windowInfo) 
			: m_window_info(windowInfo)
		{

		}

		sharkspirit::platform::window::window_info* m_window_info;
	};

	class application
	{
	public:
		application(application_create_info* applicationCreateInfo)
			: m_input(sharkspirit::input::input_processor(applicationCreateInfo->m_window_info)),
			  m_timer(Timer()),
			  m_isRunning(false),
			  m_applicationCreateInfo(applicationCreateInfo),
			  m_reg(entt::registry()),
			  m_assets(sharkspirit::assets::assets_manager()),
			  m_imgui(sharkspirit::imgui::imgui_manager()),
			  m_fps(fps_manager()),
			  m_device(sharkspirit::render::device()),
			  m_render_graph(nullptr)
		{
		}

		~application()
		{
			m_input.~input_processor();
			m_timer.~Timer();
			m_reg.clear();

			//m_sprite_render_system->~sprite_update_system();
			//m_sprite_animation_system->~sprite_animation_system();
			//m_sprite_light_render_system->~sprite_light_update_system();
		}

		entt::entity create_entity();
		
		void initialize();
		bool start();
		void stop();

	protected:
		application_create_info* m_applicationCreateInfo;

		sharkspirit::render::render_graph* m_render_graph;
		sharkspirit::render::device m_device;

		sharkspirit::input::input_processor m_input;
		sharkspirit::assets::assets_manager m_assets;
		sharkspirit::imgui::imgui_manager m_imgui;

		sprite_update_system* m_sprite_render_system;
		sprite_animation_system* m_sprite_animation_system;
		sprite_light_update_system* m_sprite_light_render_system;

		fps_manager m_fps;
		Timer m_timer;
		bool m_isRunning;

		entt::registry m_reg;
		std::vector<entt::entity> m_entities;

		void show_window()
		{
			m_applicationCreateInfo->m_window_info->m_window->show();
		}

		void hide_window()
		{
			m_applicationCreateInfo->m_window_info->m_window->hide();
		}

		virtual void on_initialize()
		{

		}

		virtual void on_update()
		{

		}
	};
}