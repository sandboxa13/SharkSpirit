#pragma once
#include "../../external/entt/entt.hpp"

namespace sharkspirit::core
{
	class ISystem
	{
	public:
		ISystem(
			entt::registry* reg,
			sharkspirit::input::input_processor* input,
			sharkspirit::assets::assets_manager* assets) : m_reg(reg), m_input(input), m_assets(assets)
		{
			
		}

		virtual void update(){}

	protected:
		entt::registry* m_reg;
		sharkspirit::input::input_processor* m_input;
		sharkspirit::assets::assets_manager* m_assets;
	};
}