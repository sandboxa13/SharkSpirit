#pragma once

#include <DirectXMath.h>
#include "Render/DirectX/Texture.h"
#include <Render/DirectX/ConstantBuffer.h>
#include <Render/DirectX/IndexBuffer.h>
#include <Render/DirectX/VertexBuffer.h>
#include <Render/DirectX/Shaders.h>
#include <Render/DirectX/Sampler.h>
#include <Assets/AssetsManager.h>

namespace sharkspirit::core
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
		sharkspirit::render::sampler* m_sampler;
		sharkspirit::render::Texture* m_texture;
		sharkspirit::render::vertex_shader m_vertex_shader;
		sharkspirit::render::pixel_shader m_pixel_shader;
		sharkspirit::render::vertex_buffer<sharkspirit::render::vertex>* m_vertices;
		sharkspirit::render::index_buffer* m_indices;
		DirectX::XMMATRIX m_world_matrix = DirectX::XMMatrixIdentity();
		sharkspirit::render::constant_buffer<sharkspirit::render::world_view_proj>* m_world_view_proj = nullptr;

		void update_world_matrix(transform_component* transform)
		{
			m_world_matrix =
				DirectX::XMMatrixScaling(transform->m_scale.x, transform->m_scale.y, 1.0f) *
				DirectX::XMMatrixRotationRollPitchYaw(DirectX::XMConvertToRadians(transform->m_rotation.x), DirectX::XMConvertToRadians(transform->m_rotation.y), DirectX::XMConvertToRadians(transform->m_rotation.z)) *
				DirectX::XMMatrixTranslation(transform->m_pos.x + transform->m_scale.x / 2.0f, transform->m_pos.y + transform->m_scale.y / 2.0f, transform->m_pos.z);
		}

		void update_world_view_proj_matrix(camera_component* camera)
		{
			auto ort = camera->GetWorldMatrix() * camera->GetOrthoMatrix();
			m_world_view_proj->data.wvpMatrix = m_world_matrix * ort;
		}
	};

	class camera_component : public base_component
	{
	public:
		camera_component()
		{
			pos = DirectX::XMFLOAT3(5.0f, 0.0f, 0.0f);
			rot = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);

			UpdateMatrix();
		}
		virtual ~camera_component()
		{

		}
		void SetProjectionValues(float width, float height, float nearZ, float farZ)
		{
			m_width = width;
			m_height = height;
			m_ortho_matrix = DirectX::XMMatrixOrthographicOffCenterLH(0.0f, width, height, 0.0f, nearZ, farZ);
		}

		const DirectX::XMMATRIX& GetOrthoMatrix() const
		{
			return m_ortho_matrix;
		}
		const DirectX::XMMATRIX& GetWorldMatrix() const
		{
			return m_world_matrix;
		}
		void SetPosition(const DirectX::XMFLOAT3& pos)
		{
			this->pos = pos;
			UpdateMatrix();
		}

		float m_width, m_height;
	private:
		void UpdateMatrix()
		{
			DirectX::XMMATRIX translationOffsetMatrix = DirectX::XMMatrixTranslation(-pos.x + 640, -pos.y + 360, 0.0f);
			DirectX::XMMATRIX camRotationMatrix = DirectX::XMMatrixRotationRollPitchYaw(rot.x, rot.y, rot.z);
			m_world_matrix = camRotationMatrix * translationOffsetMatrix;
		}

		DirectX::XMMATRIX m_world_matrix;
		DirectX::XMMATRIX m_ortho_matrix;

		DirectX::XMFLOAT3 pos;
		DirectX::XMFLOAT3 rot;

		float m_zoom;
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
			sharkspirit::assets::assets_manager* assetsManager,
			sharkspirit::render::device* device,
			sprite_component_create_info* createInfo)
		{
			HRESULT hr = { 0 };

			m_texture = assetsManager->get_texture(createInfo->m_texture_name);
			m_sampler = new sharkspirit::render::sampler(device);
			m_world_view_proj = new sharkspirit::render::constant_buffer<sharkspirit::render::world_view_proj>();
			m_vertices = assetsManager->get_verticies("sprite_vertex");
			m_indices = assetsManager->get_indicies("sprite_index");
			hr = m_world_view_proj->Initialize(device->get_device().Get(), device->get_device_context().Get());
		}

		virtual ~sprite_component()
		{

		}
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
			sharkspirit::assets::assets_manager* assetsManager,
			sharkspirit::render::device* device,
			sprite_light_component_create_info* createInfo)
		{
			HRESULT hr = { 0 };

			m_texture = assetsManager->get_texture(createInfo->m_texture_name);
			m_sampler = new sharkspirit::render::sampler(device);
			m_vertices = assetsManager->get_verticies("sprite_vertex");
			m_indices = assetsManager->get_indicies("sprite_index");
			m_world_view_proj = new sharkspirit::render::constant_buffer<sharkspirit::render::world_view_proj>();

			hr = m_world_view_proj->Initialize(device->get_device().Get(), device->get_device_context().Get());
		}
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