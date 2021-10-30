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
			graphics_manager* graphics) : m_reg(reg), m_input(input), m_graphics(graphics)
		{
			
		}
		~ISystem();

		virtual void run(){}

	protected:
		entt::registry* m_reg;
		input_processor* m_input;
		graphics_manager* m_graphics;
	};

	ISystem::~ISystem()
	{
		m_reg->clear();
	}
}