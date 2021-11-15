#pragma once
#include <Core/ECS/Components/Components.h>
#include <Render/Device.h>

namespace shark_spirit::render
{
	class render_graph
	{
		void prepare_state()
		{

		}

		void render()
		{
			// color pass
			{
				// bind shaders, pipeline, render targets
				m_color_pass.bind(m_current_state);
				
				{
					// update for every renderable
					// state->m_curent_renderable = current

					m_color_pass.update(m_current_state);
					m_color_pass.draw(m_current_state);
				}

				// unbind render target
				m_color_pass.un_bind(m_current_state);
			}

			// light pass
			{
				m_light_pass.bind(m_current_state);

				{
					// update for every light source
					// state->m_curent_renderable = current

					m_light_pass.update(m_current_state);
					m_light_pass.draw(m_current_state);
				}

				m_light_pass.un_bind(m_current_state);
			}

			// screen pass
			{
				m_screen_pass.bind(m_current_state);

				m_screen_pass.update(m_current_state);
				m_screen_pass.draw(m_current_state);

				m_screen_pass.un_bind(m_current_state);
			}
		}

		color_pass m_color_pass;
		light_pass m_light_pass;
		screen_pass m_screen_pass;

		render_graph_state* m_current_state;
		device* m_device;
	};

	class screen_pass : public render_pass
	{

	};

	class light_pass : public render_pass
	{
	public:
		void bind(render_graph_state* state) override
		{

		}
	};

	class color_pass : public render_pass
	{
	public:
		void bind(render_graph_state* state) override
		{
			const UINT offsets = 0;

			m_device->om_set_render_targets(1, state->m_color_buffer);
			m_device->clear(state->m_color_buffer, DirectX::Colors::Black);

			m_device->ps_set(m_pixel_shader.GetShader());
			m_device->vs_set(m_vertex_shader.GetShader());

			m_device->ia_set_input_layout(m_vertex_shader.GetInputLayout());
			m_device->ia_set_vertex_buffer(0, 1, state->m_curent_renderable->m_vertices.Get(), state->m_curent_renderable->m_vertices.StridePtr(), &offsets);
			m_device->ia_set_index_buffer(state->m_curent_renderable->m_indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
		}

		void update(render_graph_state* state)
		{
			m_device->ps_set_shader_resources(0, 1, state->m_curent_renderable->m_texture->GetTextureResourceView());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.Get());

			//todo : map/unmap constant buffer
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(state->m_curent_renderable->m_indices.IndexCount());
		}

		void un_bind(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr };
			m_device->ps_set_shader_resources(0, 1, null);
		}
	};

	class render_pass
	{
	public:
		render_pass(
			SharkSpirit::vertex_shader vertexShader, 
			SharkSpirit::pixel_shader pixelShader,
			device* device)
			:
			m_vertex_shader(vertexShader), 
			m_pixel_shader (pixelShader) ,
			m_device(device)
		{}

		virtual void bind(render_graph_state* state);
		virtual void update(render_graph_state* state);
		virtual void draw(render_graph_state* state);
		virtual void un_bind(render_graph_state* state);

		SharkSpirit::vertex_shader m_vertex_shader;
		SharkSpirit::pixel_shader m_pixel_shader;
		device* m_device;
	};

	class render_graph_state
	{
	public:
		render_target* m_color_buffer;
		render_target* m_light_buffer;
		render_target* m_screen_buffer;

		SharkSpirit::base_render_component* m_curent_renderable;
		application_state m_application_state;
		camera_state m_camera_state;
	};

	class scene_lights
	{

	};
	class scene_renderables
	{

	};
	class application_state
	{

	};
	class camera_state
	{

	};
}