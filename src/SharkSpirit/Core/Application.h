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
			  m_graphics(graphics_manager(applicationCreateInfo->m_window_info->m_window_handle)),
			  m_isRunning(false),
			  m_applicationCreateInfo(applicationCreateInfo),
			  m_reg(entt::registry()),
			  m_assets(assets_manager()),
			  m_imgui(imgui_manager())
		{
			
		}

		~application()
		{
			m_input.~input_processor();
			m_graphics.~graphics_manager();
			m_timer.~Timer();
			m_reg.clear();
		}

		entt::entity create_entity()
		{
			return m_reg.create();
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

			m_imgui.InitImgui(this->m_applicationCreateInfo->m_window_info->m_window_handle, m_graphics.get_device(), m_graphics.get_device_context());
			m_timer.Reset();

			on_create();

			while (m_isRunning)
			{
				m_imgui.BeginFrame();
				m_imgui.SetStyle();

				m_graphics.clear_rt();
				on_update();

				m_isRunning = m_input.process_input();
				
				ImGui::End();
				ImGui::EndFrame();

				m_graphics.present();

				m_timer.Tick();
			}

			return m_isRunning;
		}

		virtual void stop()
		{
			m_isRunning = false;
		}

	protected:
		application_create_info* m_applicationCreateInfo;

		input_processor m_input;
		graphics_manager m_graphics;
		assets_manager m_assets;
		imgui_manager m_imgui;
		Timer m_timer;
		bool m_isRunning;

		entt::registry m_reg;

		virtual void on_create()
		{

		}

		virtual void on_update()
		{

		}
	};
}