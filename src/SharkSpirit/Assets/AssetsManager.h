#pragma once
#include <Render/DirectX/Texture.h>
#include <Render/DirectX/GraphicsManager.h>
#include <map>
#include "Logger/Logger.h"

namespace SharkSpirit 
{
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
		}

		void load_texture(graphics_manager* graphicsManager, const std::string name, const std::string& path);
		Texture* get_texture(const std::string& name);

	private:
		typedef std::map<const std::string, Texture*> textures_map;

		textures_map m_textures_map;
	};
}