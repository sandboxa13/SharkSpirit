#pragma once
#include "../../external/entt/entt.hpp"

namespace SharkSpirit
{
	class ISystem
	{
	public:
		ISystem(
			entt::registry* reg,
			input_processor* input,
			graphics_manager* graphics,
			assets_manager* assets) : m_reg(reg), m_input(input), m_graphics(graphics), m_assets(assets)
		{
			
		}
		~ISystem();

		virtual void run(){}

	protected:
		entt::registry* m_reg;
		input_processor* m_input;
		graphics_manager* m_graphics;
		assets_manager* m_assets;
	};

	ISystem::~ISystem()
	{
		m_reg->clear();
	}
}