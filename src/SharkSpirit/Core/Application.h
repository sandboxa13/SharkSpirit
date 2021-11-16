#pragma once

#include "Logger/Logger.h"
#include "Input/InputProcessor.h"
#include "Core/Timer/Timer.h"
#include "IInitializable.h"
#include "Platform/Window/Window.h"
#include "Render/DirectX/GraphicsManager.h"
#include "Scene.h"
#include <Assets/AssetsManager.h>
#include <ImGui/ImGuiManager.h>
#include <Core/FpsManager.h>
#include <Render/Device.h>
#include <Render/RenderGraph.h>

namespace SharkSpirit 
{
	class application_create_info 
	{
	public:
		application_create_info(window_info* windowInfo) 
			: m_window_info(windowInfo)
		{

		}

		window_info* m_window_info;
	};

	class application
	{
	public:
		application(application_create_info* applicationCreateInfo)
			: m_input(input_processor(applicationCreateInfo->m_window_info)),
			  m_timer(Timer()),
			  m_isRunning(false),
			  m_applicationCreateInfo(applicationCreateInfo),
			  m_reg(entt::registry()),
			  m_assets(assets_manager()),
			  m_imgui(imgui_manager()),
			  m_fps(fps_manager()),
			  m_device(shark_spirit::render::device()),
			  m_render_graph(nullptr)
		{
			
		}

		~application()
		{
			m_input.~input_processor();
			m_timer.~Timer();
			m_reg.clear();
		}

		entt::entity create_entity()
		{
			auto entity = m_reg.create();

			m_entities.push_back(entity);

			return entity;
		}

		virtual void show_window() 
		{
			m_applicationCreateInfo->m_window_info->m_window->show();
		}

		virtual void hide_window()
		{
			m_applicationCreateInfo->m_window_info->m_window->hide();
		}

		virtual bool run()
		{
			m_isRunning = true;

			m_device.initialize(m_applicationCreateInfo->m_window_info->m_window_handle);

			m_assets.initialize_default_shaders(&m_device);

			m_imgui.InitImgui(this->m_applicationCreateInfo->m_window_info->m_window_handle, m_device.get_device(), m_device.get_device_context());
			m_timer.Reset();

			m_camera_entity = m_reg.create();
			auto &camera = m_reg.emplace<camera_component>(m_camera_entity);
			camera.SetProjectionValues(m_applicationCreateInfo->m_window_info->m_width, m_applicationCreateInfo->m_window_info->m_height, 0.0f, 1000.0f);

			m_render_graph = new shark_spirit::render::render_graph(&m_assets, &m_device);
			m_render_graph->initialize();

			on_create();

			while (m_isRunning)
			{
				m_imgui.BeginFrame();
				m_imgui.SetStyle();

				on_update();

				m_isRunning = m_input.process_input();

				m_render_graph->prepare_state(&m_reg);
				m_render_graph->render();
				
				ImGui::End();
				ImGui::EndFrame();

				m_device.present();

				m_timer.Tick();

				m_fps.calculate(m_timer.TotalTime());
			}

			return m_isRunning;
		}

		virtual void stop()
		{
			m_isRunning = false;
		}

	protected:
		application_create_info* m_applicationCreateInfo;

		shark_spirit::render::render_graph* m_render_graph;
		shark_spirit::render::device m_device;

		input_processor m_input;
		assets_manager m_assets;
		imgui_manager m_imgui;
		fps_manager m_fps;
		Timer m_timer;
		bool m_isRunning;

		entt::registry m_reg;
		entt::entity m_camera_entity;
		std::vector<entt::entity> m_entities;

		virtual void on_create()
		{

		}

		virtual void on_update()
		{

		}
	};
}