#include "AssetsManager.h"

namespace SharkSpirit
{
	void assets_manager::load_texture(graphics_manager* graphicsManager, const std::string name, const std::string& path)
	{
		auto pTexture = new Texture(graphicsManager, path);

		m_textures_map.emplace(name, pTexture);
	}
	Texture* assets_manager::get_texture(const std::string& name)
	{
		return m_textures_map[name];
	}
}
