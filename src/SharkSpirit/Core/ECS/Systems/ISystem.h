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
			assets_manager* assets) : m_reg(reg), m_input(input), m_assets(assets)
		{
			
		}
		~ISystem();

		virtual void run(){}

	protected:
		entt::registry* m_reg;
		input_processor* m_input;
		assets_manager* m_assets;
	};

	ISystem::~ISystem()
	{
		m_reg->clear();
	}
}