#pragma once
#include <Core/ECS/Components/Components.h>
#include <Render/Device.h>
#include "../../external/entt/entt.hpp"

namespace sharkspirit::render
{
	class application_state
	{

	};
	class camera_state
	{

	};
	class render_graph_state
	{
	public:
		render_target* m_color_buffer;
		render_target* m_light_buffer;
		render_target* m_screen_buffer;
		sharkspirit::core::base_render_component* m_curent_renderable;
		sharkspirit::core::base_render_component* m_full_screen_quad_renderable;
		application_state m_application_state;
		camera_state m_camera_state;
	};

	class render_pass
	{
	public:
		render_pass(
			vertex_shader* vertexShader,
			pixel_shader* pixelShader,
			device* device)
			:
			m_vertex_shader(vertexShader),
			m_pixel_shader(pixelShader),
			m_device(device)
		{}

		virtual void bind_render_target(render_graph_state* state) {}
		virtual void update(render_graph_state* state) {}
		virtual void draw(render_graph_state* state) {}
		virtual void unbind_render_target(render_graph_state* state) {}
		virtual void initialize() {}

		void process(render_graph_state* state)
		{
			update(state);
			draw(state);
		}

		vertex_shader* m_vertex_shader;
		pixel_shader* m_pixel_shader;
		device* m_device;

	protected:
		void bind_shaders()
		{
			m_device->ps_set(m_pixel_shader->GetShader());
			m_device->vs_set(m_vertex_shader->GetShader());
		}

		void bind_input_assembler(render_graph_state* state)
		{
			const UINT offsets = 0;

			m_device->ia_set_input_layout(m_vertex_shader->GetInputLayout());
			m_device->ia_set_vertex_buffer(0, 1, state->m_curent_renderable->m_vertices->GetAddressOf(), state->m_curent_renderable->m_vertices->StridePtr(), &offsets);
			m_device->ia_set_index_buffer(state->m_curent_renderable->m_indices->Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
		}
	};

	class screen_pass : public render_pass
	{
	public:
		screen_pass(
			vertex_shader* vertexShader,
			pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{
			m_vertex_shader = vertexShader;
			m_pixel_shader = pixelShader;
		}

		void bind_render_target(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_screen_buffer);
			m_device->clear(state->m_screen_buffer, DirectX::Colors::Black);

			m_device->ps_set_shader_resources(0, 1, state->m_color_buffer->m_shader_resource_view.GetAddressOf());
			m_device->ps_set_shader_resources(1, 1, state->m_light_buffer->m_shader_resource_view.GetAddressOf());
			m_device->ps_set_samplers(state->m_full_screen_quad_renderable->m_sampler->m_start_slot, 1u, state->m_full_screen_quad_renderable->m_sampler->m_sampler_state.GetAddressOf());

			bind_shaders();
			const UINT offsets = 0;

			m_device->ia_set_input_layout(m_vertex_shader->GetInputLayout());
			m_device->ia_set_vertex_buffer(0, 1, state->m_full_screen_quad_renderable->m_vertices->GetAddressOf(), state->m_full_screen_quad_renderable->m_vertices->StridePtr(), &offsets);
			m_device->ia_set_index_buffer(state->m_full_screen_quad_renderable->m_indices->Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(6);
		}

		void unbind_render_target(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr, nullptr };
			m_device->ps_set_shader_resources(0, 2, null);
		}
	};

	class light_pass : public render_pass
	{
	public:
		light_pass(
			vertex_shader* vertexShader,
			pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{}

		void bind_render_target(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_light_buffer);
			m_device->om_set_blend_state(m_blend_state.Get(), nullptr, 0xffffffff);

			m_device->clear(state->m_light_buffer, DirectX::Colors::Black);
		}

		void update(render_graph_state* state) override
		{
			bind_shaders();
			bind_input_assembler(state);

			m_device->ps_set_shader_resources(0, 1, state->m_curent_renderable->m_texture->GetTextureResourceViewAddress());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.GetAddressOf());

			m_device->vs_set_constant_buffers(0, 1, state->m_curent_renderable->m_world_view_proj->GetAddressOf());
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(state->m_curent_renderable->m_indices->IndexCount());
		}

		void unbind_render_target(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr };
			m_device->ps_set_shader_resources(0, 1, null);
			m_device->om_set_blend_state(nullptr, nullptr, 0xffffffff);
		}

		void initialize() override
		{
			m_blend_state = m_device->create_blend_sate();
		}

	private:
		ComPtr<ID3D11BlendState> m_blend_state;

	};

	class color_pass : public render_pass
	{
	public:
		color_pass(
			vertex_shader* vertexShader,
			pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{
			m_vertex_shader = vertexShader;
			m_pixel_shader = pixelShader;
		}

		void bind_render_target(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_color_buffer);
			m_device->clear(state->m_color_buffer, DirectX::Colors::Black);
		}

		void update(render_graph_state* state) override
		{
			bind_input_assembler(state);
			bind_shaders();

			m_device->vs_set_constant_buffers(0, 1, state->m_curent_renderable->m_world_view_proj->GetAddressOf());
			m_device->ps_set_shader_resources(0, 1, state->m_curent_renderable->m_texture->GetTextureResourceViewAddress());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.GetAddressOf());
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(state->m_curent_renderable->m_indices->IndexCount());
		}

		void unbind_render_target(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr };
			m_device->ps_set_shader_resources(0, 1, null);
		}
	};

	class render_graph
	{
	public:
		render_graph(
			sharkspirit::assets::assets_manager* assetsManager,
			device* device)
			:
			m_assets_manager(assetsManager),
			m_color_pass(nullptr),
			m_light_pass(nullptr),
			m_screen_pass(nullptr),
			m_device(device)
		{
		}

		void render(entt::registry* reg)
		{
			// color pass
			auto sprites = reg->view<sharkspirit::core::sprite_component>();
			{
				m_color_pass->bind_render_target(m_current_state);
				for (const auto spriteEntity : sprites)
				{
					m_current_state->m_curent_renderable = &sprites.get<sharkspirit::core::sprite_component>(spriteEntity);
					m_color_pass->process(m_current_state);
				}
				m_color_pass->unbind_render_target(m_current_state);
			}
			
			
			// light pass
			auto lights = reg->view<sharkspirit::core::sprite_light_component>();
			{
				m_light_pass->bind_render_target(m_current_state);
				for (const auto spriteEntity : lights)
				{
					m_current_state->m_curent_renderable = &lights.get<sharkspirit::core::sprite_light_component>(spriteEntity);
					m_light_pass->process(m_current_state);
				}
				m_light_pass->unbind_render_target(m_current_state);
			}

			
			// screen pass
			{
				m_screen_pass->bind_render_target(m_current_state);
				m_screen_pass->draw(m_current_state);
				m_screen_pass->unbind_render_target(m_current_state);
			}
		}

		void initialize()
		{
			m_current_state = new render_graph_state();

			render_target_desc colorDesc = render_target_desc();
			colorDesc.m_format = DXGI_FORMAT_R8G8B8A8_UNORM;
			colorDesc.m_array_size = 1u;
			colorDesc.m_mip_levels = 1u;
			colorDesc.m_usage = D3D11_USAGE_DEFAULT;
			colorDesc.m_sample_desc.Count = 1u;
			colorDesc.m_sample_desc.Quality = 0u;
			colorDesc.m_width = 1280;
			colorDesc.m_height = 720;


			render_target_desc lightDesc = render_target_desc();
			lightDesc.m_format = DXGI_FORMAT_R8G8B8A8_UNORM;
			lightDesc.m_array_size = 1u;
			lightDesc.m_mip_levels = 1u;
			lightDesc.m_usage = D3D11_USAGE_DEFAULT;
			lightDesc.m_sample_desc.Count = 1u;
			lightDesc.m_sample_desc.Quality = 0u;
			lightDesc.m_width = 1280;
			lightDesc.m_height = 720;

			m_current_state->m_color_buffer = m_device->create_render_target(&colorDesc);
			m_current_state->m_light_buffer = m_device->create_render_target(&lightDesc);
			m_current_state->m_screen_buffer = m_device->create_swap_chain_render_target();

			m_color_pass = new color_pass(m_assets_manager->get_vertex_shader("vs_simple"), m_assets_manager->get_pixel_shader("ps_sprite_color"), m_device);
			m_light_pass = new light_pass(m_assets_manager->get_vertex_shader("vs_simple"), m_assets_manager->get_pixel_shader("ps_sprite_light"), m_device);
			m_screen_pass = new screen_pass(m_assets_manager->get_vertex_shader("vs_full_screen"), m_assets_manager->get_pixel_shader("ps_sprite_screen_out"), m_device);

			m_current_state->m_full_screen_quad_renderable = new sharkspirit::core::base_render_component();
			m_current_state->m_full_screen_quad_renderable->m_sampler = new sampler(m_device);
			m_current_state->m_full_screen_quad_renderable->m_world_view_proj = new constant_buffer<world_view_proj>();
			m_current_state->m_full_screen_quad_renderable->m_vertices = m_assets_manager->get_verticies("full_screen_vertex");
			m_current_state->m_full_screen_quad_renderable->m_indices = m_assets_manager->get_indicies("full_screen_index");
			m_current_state->m_full_screen_quad_renderable->m_world_view_proj->Initialize(m_device->get_device().Get(), m_device->get_device_context().Get());
			//m_color_pass->initialize();
			m_light_pass->initialize();
			//m_screen_pass->initialize();
		}

	private:

		color_pass* m_color_pass;
		light_pass* m_light_pass;
		screen_pass* m_screen_pass;

		render_graph_state* m_current_state = nullptr;
		device* m_device;
		sharkspirit::assets::assets_manager* m_assets_manager;
	};
}