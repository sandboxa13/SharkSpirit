#pragma once
#include <Render/DirectX/GraphicsManager.h>
#include <DirectXMath.h>

namespace SharkSpirit
{
	class sprite_render_system 
	{
	public:

		void static render_sprite(graphics_manager* graphics, input_processor* input, sprite_component& sprite, transform_component& transform)
		{
			sprite.worldMatrix = DirectX::XMMatrixScaling(transform.m_scale.x, transform.m_scale.y, 1.0f) * DirectX::XMMatrixRotationRollPitchYaw(transform.m_rotation.x, transform.m_rotation.y, transform.m_rotation.z)* DirectX::XMMatrixTranslation(transform.m_pos.x + 256 / 2.0f, transform.m_pos.y + 256 / 2.0f, transform.m_pos.z);
			DirectX::XMMATRIX wvpMatrix = sprite.worldMatrix * DirectX::XMMatrixOrthographicOffCenterLH(0.0f, 1280, 720, 0.0f, 0.0f, 1.0f);

			graphics->get_device_context()->IASetInputLayout(sprite.vertexshader_2d.GetInputLayout());
			graphics->get_device_context()->PSSetShader(sprite.pixelshader_2d.GetShader(), NULL, 0);
			graphics->get_device_context()->VSSetShader(sprite.vertexshader_2d.GetShader(), NULL, 0);

			graphics->VSSetConstantBuffers(0, 1, sprite.cb_vs_vertexshader_2d->GetAddressOf());
			sprite.cb_vs_vertexshader_2d->data.wvpMatrix = wvpMatrix;
			sprite.cb_vs_vertexshader_2d->ApplyChanges();

			graphics->PSSetShaderResources(0, 1, sprite.m_texture->GetTextureResourceViewAddress());

			const UINT offsets = 0;
			graphics->get_device_context()->IASetVertexBuffers(0, 1, sprite.vertices.GetAddressOf(), sprite.vertices.StridePtr(), &offsets);
			graphics->get_device_context()->IASetIndexBuffer(sprite.indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
			graphics->get_device_context()->DrawIndexed(sprite.indices.IndexCount(), 0, 0);
		}
	};
}
