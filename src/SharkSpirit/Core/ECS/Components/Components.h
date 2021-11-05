#pragma once

#include <DirectXMath.h>
#include "Render/DirectX/Texture.h"
#include <Render/DirectX/ConstantBuffer.h>
#include <Render/DirectX/IndexBuffer.h>
#include <Render/DirectX/VertexBuffer.h>
#include <Render/DirectX/GraphicsManager.h>
#include <Render/DirectX/Shaders.h>
#include <Render/DirectX/Sampler.h>
#include <Assets/AssetsManager.h>

namespace SharkSpirit
{
	class base_component 
	{
	public:
		virtual ~base_component()
		{

		}
	};
	class base_render_component : public base_component
	{
	public:
		virtual ~base_render_component()
		{

		}
		sampler* m_sampler;
		Texture* m_texture;
		index_buffer m_indices;
		vertex_shader m_vertex_shader;
		pixel_shader m_pixel_shader;
		DirectX::XMMATRIX m_world_matrix = DirectX::XMMatrixIdentity();
	};

	enum animation_type
	{
		once,
		loop
	};
	class animation_clip
	{
	public:
		animation_clip(int maxAnims, animation_type type) 
			: m_max_animation_frames(maxAnims), m_is_enabled(true), m_is_now_playing(false), m_animation_type(type)
		{
			m_textures = { };
		}

		bool is_now_playing()
		{
			return m_is_now_playing;
		}

		void prepare_play()
		{
			m_is_enabled = true;
			m_current_update_counter = 0;
			m_current_frame = 0;
		}

		void update()
		{
			if (!m_is_enabled && m_animation_type == animation_type::once)
				return;

			m_is_now_playing = true;

			if (m_current_update_counter >= 35)
			{
				m_current_frame++;
				m_current_update_counter = 0;

				if (m_current_frame >= m_max_animation_frames)
				{
					m_current_frame = 0;
					m_is_now_playing = false;
					m_is_enabled = false;
				}

				return;
			}

			m_current_update_counter++;
		}

		void fill_textures_names_map(std::vector<std::string>* names)
		{
			auto count = 0;

			for (auto name : *names)
			{
				m_textures.emplace(count, name);
				count++;
			}
		}

		const std::string get_current_frame()
		{
			return m_textures[m_current_frame];
		}

		animation_type m_animation_type;

	private:
		int m_current_frame;
		int m_current_update_counter;
		int m_max_animation_frames;
		bool m_is_enabled;
		bool m_is_now_playing;

		typedef std::map<int, const std::string> textures_names_map;
		textures_names_map m_textures;
	};
	class sprite_animation_component 
	{
	public:
		sprite_animation_component() 
		{
			m_anim_clips = {};
		}

		void add_animation(const std::string& key, std::vector<std::string>* names, animation_type type)
		{
			animation_clip* clip = new animation_clip(names->size(), type);

			clip->fill_textures_names_map(names);

			m_anim_clips.emplace(key, clip);
		}

		void set_current_key(const std::string& key)
		{
			if (m_current_key == key)
				return;

			m_current_key = key;
			m_anim_clips[m_current_key]->prepare_play();
		}

		const std::string get_current_frame()
		{
			return m_anim_clips[m_current_key]->get_current_frame();
		}

		bool has_playing_anim()
		{
			return m_anim_clips[m_current_key]->is_now_playing() && m_anim_clips[m_current_key]->m_animation_type == animation_type::once;
		}

		void update()
		{
			for(auto clip : m_anim_clips)
			{
				clip.second->update();
			}
		}

	private:
		typedef std::map<const std::string, animation_clip*> animations_clips;

		animations_clips m_anim_clips;
		std::string m_current_key;
	};

	class sprite_component_create_info
	{
	public :
		sprite_component_create_info(
			const std::string& textureName,
			const std::wstring& pixelShaderPath,
			const std::wstring& vertexShaderPath
			) : 
			m_pixel_shader_path(pixelShaderPath),
			m_vertex_shader_path(vertexShaderPath),
			m_texture_name(textureName)
		{

		}
		const std::string& m_texture_name;
		const std::wstring& m_pixel_shader_path;
		const std::wstring& m_vertex_shader_path;
	};
	class sprite_component : public base_render_component
	{
	public:
		sprite_component(
			assets_manager* assetsManager,
			graphics_manager* graphicsManager, 
			sprite_component_create_info* createInfo)
		{
			HRESULT hr = { 0 };

			m_texture = assetsManager->get_texture(createInfo->m_texture_name);
			m_sampler = new sampler(graphicsManager);

			//2d shaders
			D3D11_INPUT_ELEMENT_DESC layout2D[] =
			{
				{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
				{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			};

			UINT numElements2D = ARRAYSIZE(layout2D);

			hr = m_vertex_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_vertex_shader_path, layout2D, numElements2D);
			hr = m_pixel_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_pixel_shader_path);
			
			cb_vs_vertexshader_2d = new constant_buffer<constant_buffer_2d>();
			hr = cb_vs_vertexshader_2d->Initialize(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get());
		}

		virtual ~sprite_component()
		{

		}

		constant_buffer<constant_buffer_2d>* cb_vs_vertexshader_2d = nullptr;
	};


	class sprite_light_component_create_info
	{
	public:
		sprite_light_component_create_info(
			const std::string& textureName,
			const std::wstring& pixelShaderPath,
			const std::wstring& vertexShaderPath
		) :
			m_pixel_shader_path(pixelShaderPath),
			m_vertex_shader_path(vertexShaderPath),
			m_texture_name(textureName)
		{

		}
		const std::string& m_texture_name;
		const std::wstring& m_pixel_shader_path;
		const std::wstring& m_vertex_shader_path;
	};

	class sprite_light_component : public base_render_component
	{
	public:
		sprite_light_component(
			assets_manager* assetsManager,
			graphics_manager* graphicsManager,
			sprite_light_component_create_info* createInfo)
		{
			HRESULT hr = { 0 };

			m_texture = assetsManager->get_texture(createInfo->m_texture_name);
			m_sampler = new sampler(graphicsManager);

			//2d shaders
			D3D11_INPUT_ELEMENT_DESC layout2D[] =
			{
				{"POSITION", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
				{"TEXCOORD", 0, DXGI_FORMAT::DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_CLASSIFICATION::D3D11_INPUT_PER_VERTEX_DATA, 0  },
			};

			UINT numElements2D = ARRAYSIZE(layout2D);

			hr = m_vertex_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_vertex_shader_path, layout2D, numElements2D);
			hr = m_pixel_shader.Initialize(graphicsManager->get_device().Get(), createInfo->m_pixel_shader_path);

			cb_vs_vertexshader_2d = new constant_buffer<constant_buffer_2d>();
			hr = cb_vs_vertexshader_2d->Initialize(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get());
		}

		constant_buffer<constant_buffer_2d>* cb_vs_vertexshader_2d = nullptr;
	};

	struct transform_component
	{
		transform_component(
			DirectX::XMFLOAT3 pos, 
			DirectX::XMFLOAT3 rot, 
			DirectX::XMFLOAT2 scale) 
			: m_pos(pos), m_rotation(rot), m_scale(scale)
		{

		}

		DirectX::XMFLOAT3 m_pos;
		DirectX::XMFLOAT3 m_rotation;
		DirectX::XMFLOAT2 m_scale;
	};
}