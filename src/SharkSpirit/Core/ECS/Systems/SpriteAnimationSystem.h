#pragma once
#include "Core/ECS/Systems/ISystem.h"
#include <Input/InputProcessor.h>
#include <Core/ECS/Components/Components.h>

namespace sharkspirit::core
{
	class sprite_animation_system : public ISystem
	{
	public:
		sprite_animation_system(
			entt::registry* reg,
			sharkspirit::input::input_processor* input,
			sharkspirit::assets::assets_manager* assets) : ISystem(reg, input, assets)
		{
			
		}
		~sprite_animation_system();

		void run() override
		{
			auto spriteView = m_reg->view<sprite_component, sprite_animation_component>();

			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_component>(entity);
				auto& spriteAnim = spriteView.get<sprite_animation_component>(entity);

				auto currentFrame = spriteAnim.get_current_frame();
				sprite.m_texture = m_assets->get_texture(currentFrame);
				spriteAnim.update();
			}
		}
	};
}