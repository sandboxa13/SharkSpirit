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
			graphics_manager* graphics) : ISystem(reg, input, graphics)
		{
			m_sprite_render_pipiline = sprite_render_pipeline();
		}
		~sprite_render_system();

		void run() override 
		{
			auto spriteView = m_reg->view<sprite_component, transform_component>();
			
			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_component>(entity);
				auto& transform = spriteView.get<transform_component>(entity);

				sprite.m_world_matrix = DirectX::XMMatrixScaling(transform.m_scale.x, transform.m_scale.y, 1.0f) * DirectX::XMMatrixRotationRollPitchYaw(transform.m_rotation.x, transform.m_rotation.y, transform.m_rotation.z) * DirectX::XMMatrixTranslation(transform.m_pos.x + 256 / 2.0f, transform.m_pos.y + 256 / 2.0f, transform.m_pos.z);
				DirectX::XMMATRIX wvpMatrix = sprite.m_world_matrix * DirectX::XMMatrixOrthographicOffCenterLH(0.0f, 1280, 720, 0.0f, 0.0f, 1.0f);
				sprite.cb_vs_vertexshader_2d->data.wvpMatrix = wvpMatrix;
				sprite.cb_vs_vertexshader_2d->ApplyChanges();

				m_sprite_render_pipiline.execute(m_graphics, sprite);
			}
		}
	private:
		sprite_render_pipeline m_sprite_render_pipiline;
	};
}
