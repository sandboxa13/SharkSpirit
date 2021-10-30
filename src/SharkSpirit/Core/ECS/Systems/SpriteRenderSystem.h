#pragma once
#include <Render/DirectX/GraphicsManager.h>
#include <DirectXMath.h>
#include "Core/ECS/Systems/ISystem.h"
#include <Core/ECS/Components/Components.h>
#include <Input/InputProcessor.h>

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

		}
		~sprite_render_system();

		void run() override 
		{
			auto spriteView = m_reg->view<sprite_component, transform_component>();
			
			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_component>(entity);
				auto& transform = spriteView.get<transform_component>(entity);

				sprite.worldMatrix = DirectX::XMMatrixScaling(transform.m_scale.x, transform.m_scale.y, 1.0f) * DirectX::XMMatrixRotationRollPitchYaw(transform.m_rotation.x, transform.m_rotation.y, transform.m_rotation.z) * DirectX::XMMatrixTranslation(transform.m_pos.x + 256 / 2.0f, transform.m_pos.y + 256 / 2.0f, transform.m_pos.z);
				DirectX::XMMATRIX wvpMatrix = sprite.worldMatrix * DirectX::XMMatrixOrthographicOffCenterLH(0.0f, 1280, 720, 0.0f, 0.0f, 1.0f);

				m_graphics->get_device_context()->IASetInputLayout(sprite.vertexshader_2d.GetInputLayout());
				m_graphics->get_device_context()->PSSetShader(sprite.pixelshader_2d.GetShader(), NULL, 0);
				m_graphics->get_device_context()->VSSetShader(sprite.vertexshader_2d.GetShader(), NULL, 0);

				m_graphics->vs_set_constant_buffers(0, 1, sprite.cb_vs_vertexshader_2d->GetAddressOf());
				sprite.cb_vs_vertexshader_2d->data.wvpMatrix = wvpMatrix;
				sprite.cb_vs_vertexshader_2d->ApplyChanges();

				m_graphics->ps_set_shader_resources(0, 1, sprite.m_texture->GetTextureResourceViewAddress());
				m_graphics->get_device_context().Get()->PSSetSamplers(0u, 1u, sprite.m_pSampler.GetAddressOf());

				const UINT offsets = 0;
				m_graphics->get_device_context()->IASetVertexBuffers(0, 1, sprite.vertices.GetAddressOf(), sprite.vertices.StridePtr(), &offsets);
				m_graphics->get_device_context()->IASetIndexBuffer(sprite.indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
				m_graphics->get_device_context()->DrawIndexed(sprite.indices.IndexCount(), 0, 0);
			}
		}
	};
}
