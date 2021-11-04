#pragma once
#include <Render/DirectX/GraphicsManager.h>
#include <DirectXMath.h>
#include "Core/ECS/Systems/ISystem.h"
#include <Core/ECS/Components/Components.h>
#include <Input/InputProcessor.h>
#include <Render/RenderPipeline.h>

namespace SharkSpirit
{
	class sprite_render_system : public ISystem
	{
	public:
		sprite_render_system(
			entt::registry* reg,
			input_processor* input,
			graphics_manager* graphics,
			assets_manager* assets) : ISystem(reg, input, graphics, assets)
		{
			m_sprite_render_pipiline = sprite_render_pipeline();
		}
		~sprite_render_system();

		void run() override 
		{
			using namespace DirectX;

			auto spriteView = m_reg->view<sprite_component, transform_component>();

			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_component>(entity);
				auto& transform = spriteView.get<transform_component>(entity);

				sprite.m_world_matrix = 
					DirectX::XMMatrixScaling(transform.m_scale.x, transform.m_scale.y, 1.0f) * 
					DirectX::XMMatrixRotationRollPitchYaw(DirectX::XMConvertToRadians(transform.m_rotation.x), DirectX::XMConvertToRadians(transform.m_rotation.y), DirectX::XMConvertToRadians(transform.m_rotation.z)) *
					DirectX::XMMatrixTranslation(transform.m_pos.x + transform.m_scale.x / 2.0f, transform.m_pos.y + transform.m_scale.y / 2.0f, transform.m_pos.z);
				auto ort = m_graphics->m_camera_2d.GetWorldMatrix() * m_graphics->m_camera_2d.GetOrthoMatrix();
				DirectX::XMMATRIX wvpMatrix = sprite.m_world_matrix * ort;
				sprite.cb_vs_vertexshader_2d->data.wvpMatrix = wvpMatrix;
				sprite.cb_vs_vertexshader_2d->ApplyChanges();

				m_sprite_render_pipiline.execute(m_graphics, sprite);
			}
		}
	private:
		sprite_render_pipeline m_sprite_render_pipiline;
	};
}
