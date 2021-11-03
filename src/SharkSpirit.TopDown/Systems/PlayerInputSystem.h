#pragma once

#include "Core/ECS/Systems/ISystem.h"
#include <Core/ECS/Components/Components.h>
#include "../Components/PlayerInputComponent.h"
#include <Input/InputProcessor.h>

namespace SharkSpirit 
{
	class player_input_system : public ISystem
	{
	public:
		player_input_system(
			entt::registry* reg, 
			input_processor* input, 
			graphics_manager* graphics) : ISystem(reg, input, graphics)
		{

		}
		~player_input_system();


		void run() override
		{
			auto inputView = m_reg->view<player_input_component, transform_component>();
			for (auto entity : inputView)
			{
				auto& playerInput = inputView.get<player_input_component>(entity);
				auto& playerTransform = inputView.get<transform_component>(entity);

				auto speed = playerInput.m_walk_speed;

				if (m_input->m_keyboard.KeyIsPressed('\x10'))
				{
					speed += playerInput.m_acceleration;
				}

				if (m_input->m_keyboard.KeyIsPressed('S'))
				{
					playerTransform.m_pos.y += speed;
				}
				if (m_input->m_keyboard.KeyIsPressed('W'))
				{
					playerTransform.m_pos.y -= speed;
				}
				if (m_input->m_keyboard.KeyIsPressed('D'))
				{
					playerTransform.m_pos.x += speed;
				}
				if (m_input->m_keyboard.KeyIsPressed('A'))
				{
					playerTransform.m_pos.x -= speed;
				}

				auto angle = std::atan2(m_input->m_mouse.GetPosY() - playerTransform.m_pos.y, m_input->m_mouse.GetPosX() - playerTransform.m_pos.x) * 180.0 / 3.14f;

				m_graphics->m_camera_2d.SetPosition(playerTransform.m_pos);
				playerTransform.m_rotation.z = angle;
			}
		}
	};

	player_input_system::~player_input_system()
	{
	}
}