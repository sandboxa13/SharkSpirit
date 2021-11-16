#pragma once
#include <Render/DirectX/Texture.h>
#include <Render/DirectX/GraphicsManager.h>
#include <map>
#include "Logger/Logger.h"
#include <Render/DirectX/Shaders.h>
#include <Render/DirectX/VertexBuffer.h>
#include <Render/DirectX/IndexBuffer.h>

namespace SharkSpirit 
{
	enum shader_type
	{
		PIXEL,
		VERTEX,
		COMPUTE,
		GEOMETRY
	};

	class assets_manager
	{
	public:
		assets_manager() : m_textures_map(textures_map())
		{
		}
		~assets_manager()
		{
			for (auto i : m_textures_map)
			{
				i.first.~basic_string();
				i.second->~Texture();
			}

			for (auto i : m_vertex_shaders_map)
			{
				i.first.~basic_string();
				//i.second->~();
			}

			for (auto i : m_pixel_shaders_map)
			{
				i.first.~basic_string();
				//i.second->~Texture();
			}
		}

		void initialize_default_shaders(shark_spirit::render::device* device);

		void load_texture(shark_spirit::render::device* device, const std::string name, const std::string& path);
		Texture* get_texture(const std::string& name);

		void load_shader(shark_spirit::render::device* device, const std::string name, const std::wstring& path, shader_type type);
		pixel_shader* get_pixel_shader(const std::string& name);
		vertex_shader* get_vertex_shader(const std::string& name);

		vertex_buffer<vertex>* get_verticies(const std::string& name);
		index_buffer* get_indicies(const std::string& name);
	private:
		typedef std::map<const std::string, Texture*> textures_map;
		typedef std::map<const std::string, vertex_shader*> vertex_shaders_map;
		typedef std::map<const std::string, pixel_shader*> pixel_shaders_map;
		typedef std::map<const std::string, vertex_buffer<vertex>*> verticies_map;
		typedef std::map<const std::string, index_buffer*> indicies_map;

		textures_map m_textures_map;
		vertex_shaders_map m_vertex_shaders_map;
		pixel_shaders_map m_pixel_shaders_map;
		verticies_map m_verticies_map;
		indicies_map m_indicies_map;
	};
}