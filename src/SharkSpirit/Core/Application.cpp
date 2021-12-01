#include "Application.h"

namespace sharkspirit::core
{
	entt::entity application::create_entity()
	{
		auto entity = m_reg.create();

		m_entities.push_back(entity);

		return entity;
	}

	void application::initialize()
	{
		m_device.initialize(m_applicationCreateInfo->m_window_info->m_window_handle);

		m_assets.initialize_default_shaders(&m_device);

		m_imgui.InitImgui(this->m_applicationCreateInfo->m_window_info->m_window_handle, m_device.get_device(), m_device.get_device_context());
		m_timer.Reset();

		auto& camera = m_reg.set<camera_component>();
		camera.SetProjectionValues(m_applicationCreateInfo->m_window_info->m_width, m_applicationCreateInfo->m_window_info->m_height, 0.0f, 1000.0f);

		m_render_graph = new sharkspirit::render::render_graph(&m_assets, &m_device);
		m_render_graph->initialize();

		m_sprite_render_system = new sharkspirit::core::sprite_update_system(&m_reg, &m_input, &m_assets);
		m_sprite_animation_system = new sharkspirit::core::sprite_animation_system(&m_reg, &m_input, &m_assets);
		m_sprite_light_render_system = new sharkspirit::core::sprite_light_update_system(&m_reg, &m_input, &m_assets);

		on_initialize();
	}

	bool application::start()
	{
		m_isRunning = true;
		m_applicationCreateInfo->m_window_info->m_window->show();

		while (m_isRunning)
		{
			m_imgui.BeginFrame();
			m_imgui.SetStyle();

			on_update();

			m_sprite_render_system->update();
			m_sprite_animation_system->update();
			m_sprite_light_render_system->update();

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

	void application::stop()
	{
		m_isRunning = false;

	}
}
