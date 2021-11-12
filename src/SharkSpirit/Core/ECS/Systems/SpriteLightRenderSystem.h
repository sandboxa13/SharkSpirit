#pragma once
#include "Core/ECS/Systems/ISystem.h"
#include <Input/InputProcessor.h>
#include <Render/DirectX/GraphicsManager.h>
#include <Core/ECS/Components/Components.h>
#include <Render/RenderPipeline.h>

namespace SharkSpirit
{
	class sprite_light_render_system : public ISystem
	{
	public:
		sprite_light_render_system(
			entt::registry* reg,
			input_processor* input,
			graphics_manager* graphics,
			assets_manager* assets) : ISystem(reg, input, graphics, assets)
		{
			m_sprite_render_pipiline = sprite_light_render_pipeline();
		}
		~sprite_light_render_system();

		void run() override
		{
			using namespace DirectX;

			auto spriteView = m_reg->view<sprite_light_component, transform_component>();

			float blend_factor[4] = { 0.f, 0.f, 0.f, 0.f };
			m_graphics->get_device_context()->OMSetRenderTargets(1, m_graphics->m_pLightMapRTV.GetAddressOf(), nullptr);
			m_graphics->clear_light_rt();
			m_graphics->get_device_context()->OMSetBlendState(m_graphics->m_blend_state.Get(), nullptr, 0xffffffff);

			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_light_component>(entity);
				auto& transform = spriteView.get<transform_component>(entity);

				sprite.m_world_matrix =
					DirectX::XMMatrixScaling(transform.m_scale.x * 7, transform.m_scale.y * 7, 1.0f) *
					DirectX::XMMatrixRotationRollPitchYaw(DirectX::XMConvertToRadians(transform.m_rotation.x), DirectX::XMConvertToRadians(transform.m_rotation.y), DirectX::XMConvertToRadians(transform.m_rotation.z)) *
					DirectX::XMMatrixTranslation(transform.m_pos.x + transform.m_scale.x / 2.0f, transform.m_pos.y + transform.m_scale.y / 2.0f, transform.m_pos.z);
				auto ort = m_graphics->m_camera_2d.GetWorldMatrix() * m_graphics->m_camera_2d.GetOrthoMatrix();
				DirectX::XMMATRIX wvpMatrix = sprite.m_world_matrix * ort;
				sprite.cb_vs_vertexshader_2d->data.wvpMatrix = wvpMatrix;
				sprite.cb_vs_vertexshader_2d->ApplyChanges();

				m_sprite_render_pipiline.execute(m_graphics, sprite);
			}

			m_graphics->get_device_context()->OMSetBlendState(nullptr, nullptr, 0xffffffff);
		}

		sprite_light_render_pipeline m_sprite_render_pipiline;
	};
}