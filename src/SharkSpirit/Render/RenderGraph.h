#pragma once
#include <Core/ECS/Components/Components.h>
#include <Render/Device.h>

namespace shark_spirit::render
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
		std::vector<SharkSpirit::base_render_component*> m_render_components = {0};
		std::vector<SharkSpirit::base_render_component*> m_light_components = {0};
		SharkSpirit::base_render_component* m_curent_renderable;
		application_state m_application_state;
		camera_state m_camera_state;
	};

	class render_pass
	{
	public:
		render_pass(
			SharkSpirit::vertex_shader* vertexShader,
			SharkSpirit::pixel_shader* pixelShader,
			device* device)
			:
			m_vertex_shader(vertexShader),
			m_pixel_shader(pixelShader),
			m_device(device)
		{}

		virtual void bind(render_graph_state* state) {}
		virtual void update(render_graph_state* state){}
		virtual void draw(render_graph_state* state){}
		virtual void un_bind(render_graph_state* state){}
		virtual void initialize(){}

		SharkSpirit::vertex_shader* m_vertex_shader;
		SharkSpirit::pixel_shader* m_pixel_shader;
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
			SharkSpirit::vertex_shader* vertexShader,
			SharkSpirit::pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{
			m_vertex_shader = vertexShader;
			m_pixel_shader = pixelShader;
		}

		void bind(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_screen_buffer);
			m_device->clear(state->m_screen_buffer, DirectX::Colors::Black);

			m_device->ps_set_shader_resources(0, 1, state->m_color_buffer->m_shader_resource_view.GetAddressOf());
			m_device->ps_set_shader_resources(1, 1, state->m_light_buffer->m_shader_resource_view.GetAddressOf());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.GetAddressOf());

			bind_shaders();
			bind_input_assembler(state);
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(6);
		}

		void un_bind(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr, nullptr };
			m_device->ps_set_shader_resources(0, 1, null);
		}
	};

	class light_pass : public render_pass
	{
	public:
		light_pass(
			SharkSpirit::vertex_shader* vertexShader,
			SharkSpirit::pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{}

		void bind(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_light_buffer);
			m_device->om_set_blend_state(m_blend_state.Get(), nullptr, 0xffffffff);

			m_device->clear(state->m_light_buffer, DirectX::Colors::Black);

			bind_shaders();
			bind_input_assembler(state);

			m_device->ps_set_shader_resources(0, 1, state->m_curent_renderable->m_texture->GetTextureResourceViewAddress());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.GetAddressOf());
		}

		void update(render_graph_state* state) override
		{
			m_device->vs_set_constant_buffers(0, 1, state->m_curent_renderable->m_world_view_proj->GetAddressOf());
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(state->m_curent_renderable->m_indices->IndexCount());
		}

		void un_bind(render_graph_state* state) override
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
			SharkSpirit::vertex_shader* vertexShader,
			SharkSpirit::pixel_shader* pixelShader,
			device* device)
			:
			render_pass(vertexShader, pixelShader, device)
		{
			m_vertex_shader = vertexShader;
			m_pixel_shader = pixelShader;
		}

		void bind(render_graph_state* state) override
		{
			m_device->om_set_render_targets(1, state->m_color_buffer);
			m_device->clear(state->m_color_buffer, DirectX::Colors::Black);

			bind_input_assembler(state);
			bind_shaders();
		}

		void update(render_graph_state* state) override
		{
			m_device->ps_set_shader_resources(0, 1, state->m_curent_renderable->m_texture->GetTextureResourceViewAddress());
			m_device->ps_set_samplers(state->m_curent_renderable->m_sampler->m_start_slot, 1u, state->m_curent_renderable->m_sampler->m_sampler_state.GetAddressOf());
			m_device->vs_set_constant_buffers(0, 1, state->m_curent_renderable->m_world_view_proj->GetAddressOf());
		}

		void draw(render_graph_state* state) override
		{
			m_device->draw_indexed(state->m_curent_renderable->m_indices->IndexCount());
		}

		void un_bind(render_graph_state* state) override
		{
			ID3D11ShaderResourceView* null[] = { nullptr };
			m_device->ps_set_shader_resources(0, 1, null);
		}
	};

	class render_graph
	{
	public:
		render_graph(
			SharkSpirit::assets_manager* assetsManager,
			device* device)
			: 
			m_assets_manager(assetsManager),
			m_color_pass(nullptr),
			m_light_pass(nullptr),
			m_screen_pass(nullptr),
			m_device(device)
		{
		}

		void prepare_state(entt::registry* reg)
		{
			auto sprites = reg->view<SharkSpirit::sprite_component>();
			auto lights = reg->view<SharkSpirit::sprite_light_component>();

			for (auto spriteEnt : sprites)
			{
				SharkSpirit::sprite_component* sprite = &sprites.get<SharkSpirit::sprite_component>(spriteEnt);

				m_current_state->m_render_components.push_back(sprite);
			}

			m_current_state->m_render_components.erase(m_current_state->m_render_components.begin());

			for (auto lightEnt : lights)
			{
				SharkSpirit::sprite_light_component* light = &lights.get<SharkSpirit::sprite_light_component>(lightEnt);

				m_current_state->m_light_components.push_back(light);
			}

			m_current_state->m_light_components.erase(m_current_state->m_light_components.begin());
		}

		void render()
		{
			m_current_state->m_curent_renderable = m_current_state->m_render_components[0];

			// color pass
			{
				m_color_pass->bind(m_current_state);
				
				{
					for (auto sprite : m_current_state->m_render_components)
					{
						m_current_state->m_curent_renderable = sprite;

						m_color_pass->update(m_current_state);
						m_color_pass->draw(m_current_state);
					}
				}

				// unbind render target
				m_color_pass->un_bind(m_current_state);
			}

			m_current_state->m_curent_renderable = m_current_state->m_light_components[0];

			// light pass
			{
				m_light_pass->bind(m_current_state);

				{
					for (auto light : m_current_state->m_light_components)
					{
						m_current_state->m_curent_renderable = light;

						m_light_pass->update(m_current_state);
						m_light_pass->draw(m_current_state);
					}
				}

				m_light_pass->un_bind(m_current_state);
			}

			// screen pass
			{
				m_screen_pass->bind(m_current_state);

				m_screen_pass->draw(m_current_state);

				m_screen_pass->un_bind(m_current_state);
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

			m_current_state->m_color_buffer = m_device->create_render_target(&colorDesc);

			render_target_desc lightDesc = render_target_desc();
			lightDesc.m_format = DXGI_FORMAT_R8G8B8A8_UNORM;
			lightDesc.m_array_size = 1u;
			lightDesc.m_mip_levels = 1u;
			lightDesc.m_usage = D3D11_USAGE_DEFAULT;
			lightDesc.m_sample_desc.Count = 1u;
			lightDesc.m_sample_desc.Quality = 0u;
			lightDesc.m_width = 1280;
			lightDesc.m_height = 720;

			m_current_state->m_light_buffer = m_device->create_render_target(&lightDesc);

			m_current_state->m_screen_buffer = m_device->create_swap_chain_render_target();

			m_color_pass = new color_pass(m_assets_manager->get_vertex_shader("vs_simple"), m_assets_manager->get_pixel_shader("ps_sprite_color"), m_device);
			m_light_pass = new light_pass(m_assets_manager->get_vertex_shader("vs_simple"), m_assets_manager->get_pixel_shader("ps_sprite_light"), m_device);
			m_screen_pass = new screen_pass(m_assets_manager->get_vertex_shader("vs_full_screen"), m_assets_manager->get_pixel_shader("ps_sprite_screen_out"), m_device);

			//m_color_pass->initialize();
			//m_light_pass->initialize();
			//m_screen_pass->initialize();
		}

	private:

		color_pass* m_color_pass;
		light_pass* m_light_pass;
		screen_pass* m_screen_pass;

		render_graph_state* m_current_state;
		device* m_device;
		SharkSpirit::assets_manager* m_assets_manager;
	};
}