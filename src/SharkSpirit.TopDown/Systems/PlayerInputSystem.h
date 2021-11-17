#pragma once

#include "Core/ECS/Systems/ISystem.h"
#include <Core/ECS/Components/Components.h>
#include "../Components/PlayerInputComponent.h"
#include <Input/InputProcessor.h>

namespace SharkSpirit 
{
	class player_input_system : public sharkspirit::core::ISystem
	{
	public:
		player_input_system(
			entt::registry* reg, 
			sharkspirit::input::input_processor* input,
			sharkspirit::assets::assets_manager* assets) : ISystem(reg, input, assets)
		{

		}
		~player_input_system();


		void run() override
		{
			auto inputView = m_reg->view<player_input_component, sharkspirit::core::transform_component>();
			auto camView = m_reg->view<sharkspirit::core::camera_component>();
			sharkspirit::core::camera_component* camera = nullptr;
			for (auto cam : camView)
			{
				camera = &camView.get<sharkspirit::core::camera_component>(cam);
			}

			for (auto entity : inputView)
			{
				auto& playerInput = inputView.get<player_input_component>(entity);
				auto& playerTransform = inputView.get<sharkspirit::core::transform_component>(entity);

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

				auto mouseX = m_input->m_mouse.GetPosX();
				auto mouseY = m_input->m_mouse.GetPosY();

				DirectX::XMMATRIX projection = camera->GetOrthoMatrix();
				DirectX::XMMATRIX view = camera->GetWorldMatrix();

				auto tmp = DirectX::XMMatrixDeterminant(view * projection);
				DirectX::XMMATRIX invProjectionView = DirectX::XMMatrixInverse(&tmp, (view * projection));

				float x = (((2.0f * mouseX) / camera->m_width) - 1);
				float y = -(((2.0f * mouseY) / camera->m_height) - 1);

				DirectX::XMVECTOR mousePosition = DirectX::XMVectorSet(x, y, 1.0f, 0.0f);

				auto mouseInWorldSpace = DirectX::XMVector3Transform(mousePosition, invProjectionView);

				DirectX::XMFLOAT4 v2F;    //the float where we copy the v2 vector members
				DirectX::XMStoreFloat4(&v2F, mouseInWorldSpace);

				auto angle = std::atan2(v2F.y - playerTransform.m_pos.y, v2F.x - playerTransform.m_pos.x) * 180.0 / 3.14f;

				camera->SetPosition(playerTransform.m_pos);
				playerTransform.m_rotation.z = angle;

				if (ImGui::Begin("Player statistics :"))
				{
					ImGui::Text("Position X - %f, Y - %f", playerTransform.m_pos.x, playerTransform.m_pos.y);
					ImGui::Text("Mouse X - %f, Y - %f", v2F.x, v2F.y);
					ImGui::Text("Angle %f", playerTransform.m_rotation.z);
					ImGui::Text("Speed %f", speed);
				}

				ImGui::End();
			}
		}
	};

	player_input_system::~player_input_system()
	{
	}
}