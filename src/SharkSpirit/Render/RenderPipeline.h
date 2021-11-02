#pragma once
#include <Core/ECS/Components/Components.h>

namespace SharkSpirit 
{
	class render_pipeline
	{
	public:
		virtual void execute(graphics_manager* graphics, base_render_component& renderComponent) 
		{

		}
		virtual void execute(graphics_manager* graphics, sprite_component& spriteComponent) 
		{

		}
	};

	class sprite_render_pipeline : public render_pipeline
	{
	public:
		void execute(graphics_manager* graphics, sprite_component& sprite) override
		{
			graphics->get_device_context()->IASetInputLayout(sprite.m_vertex_shader.GetInputLayout());
			graphics->get_device_context()->PSSetShader(sprite.m_pixel_shader.GetShader(), NULL, 0);
			graphics->get_device_context()->VSSetShader(sprite.m_vertex_shader.GetShader(), NULL, 0);

			graphics->vs_set_constant_buffers(0, 1, sprite.cb_vs_vertexshader_2d->GetAddressOf());

			graphics->ps_set_shader_resources(0, 1, sprite.m_texture->GetTextureResourceViewAddress());
			graphics->get_device_context().Get()->PSSetSamplers(sprite.m_sampler->m_start_slot, 1u, sprite.m_sampler->m_sampler_state.GetAddressOf());

			const UINT offsets = 0;

			graphics->get_device_context()->IASetVertexBuffers(0, 1, sprite.vertices.GetAddressOf(), sprite.vertices.StridePtr(), &offsets);
			graphics->get_device_context()->IASetIndexBuffer(sprite.m_indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
			graphics->get_device_context()->DrawIndexed(sprite.m_indices.IndexCount(), 0, 0);
		}
	};
}