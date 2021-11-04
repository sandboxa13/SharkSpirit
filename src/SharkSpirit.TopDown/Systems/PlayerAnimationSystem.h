#pragma once
#include "Core/ECS/Systems/ISystem.h"
#include <Input/InputProcessor.h>
#include <Render/DirectX/GraphicsManager.h>
#include <Core/ECS/Components/Components.h>
#include "../Components/PlayerInputComponent.h"

namespace SharkSpirit
{
	class player_animation_system : public ISystem
	{
	public:
		player_animation_system(
			entt::registry* reg,
			input_processor* input,
			graphics_manager* graphics,
			assets_manager* assets) : ISystem(reg, input, graphics, assets)
		{

		}
		~player_animation_system();

		void run() override
		{
			auto spriteView = m_reg->view<player_input_component, sprite_animation_component>();

			for (auto entity : spriteView)
			{
				auto& spriteAnim = spriteView.get<sprite_animation_component>(entity);

				if (spriteAnim.has_playing_anim())
				{
					return;
				}

				if (m_input->m_keyboard.KeyIsPressed('R'))
				{
					spriteAnim.set_current_key("reload");
					continue;
				}

				if (m_input->m_keyboard.KeyIsPressed('W') ||
					m_input->m_keyboard.KeyIsPressed('S') ||
					m_input->m_keyboard.KeyIsPressed('A') ||
					m_input->m_keyboard.KeyIsPressed('D'))
				{
					spriteAnim.set_current_key("move");
					continue;
				}

				spriteAnim.set_current_key("idle");
			}
		}
	};
}