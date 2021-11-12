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

		virtual void execute(graphics_manager* graphics, sprite_light_component& spriteComponent)
		{

		}

		virtual void execute(graphics_manager* graphics)
		{

		}
	};

	class light_pass : public render_pipeline
	{
	public:
		void execute(graphics_manager* graphics) override
		{
			graphics->get_device_context()->OMSetRenderTargets(1, graphics->m_pRenderTargetView.GetAddressOf(), nullptr);

			graphics->clear_rt();

			graphics->get_device_context()->IASetInputLayout(graphics->m_vertex_shader.GetInputLayout());
			graphics->get_device_context()->PSSetShader(graphics->m_pixel_shader.GetShader(), NULL, 0);
			graphics->get_device_context()->VSSetShader(graphics->m_vertex_shader.GetShader(), NULL, 0);
			
		/*	graphics->m_cb_vs_vertexshader_2d->data.wvpMatrix = DirectX::XMMatrixIdentity();
			graphics->m_cb_vs_vertexshader_2d->ApplyChanges();
			graphics->vs_set_constant_buffers(0, 1, graphics->m_cb_vs_vertexshader_2d->GetAddressOf());*/

			graphics->ps_set_shader_resources(0, 1, graphics->m_pColorMapSRV.GetAddressOf());
			graphics->ps_set_shader_resources(1, 1, graphics->m_pLightMapSRV.GetAddressOf());

			const UINT offsets = 0;

			graphics->get_device_context().Get()->PSSetSamplers(0, 1u, &graphics->m_sampleState);
			graphics->get_device_context()->IASetVertexBuffers(0, 1, graphics->m_vertices_light_pass.GetAddressOf(), graphics->m_vertices_light_pass.StridePtr(), &offsets);
			graphics->get_device_context()->IASetIndexBuffer(graphics->m_indicies_light_pass.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);

			graphics->get_device_context()->DrawIndexed(6, 0, 0);

			ID3D11ShaderResourceView* null[] = { nullptr, nullptr };
			graphics->get_device_context()->PSSetShaderResources(0, 2, null);
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

			graphics->get_device_context()->IASetVertexBuffers(0, 1, graphics->m_vertices.GetAddressOf(), graphics->m_vertices.StridePtr(), &offsets);
			graphics->get_device_context()->IASetIndexBuffer(graphics->m_indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
			graphics->get_device_context()->DrawIndexed(graphics->m_indices.IndexCount(), 0, 0);

			ID3D11ShaderResourceView* null[] = { nullptr };
			graphics->get_device_context()->PSSetShaderResources(0, 1, null);
		}
	};

	class sprite_light_render_pipeline : public render_pipeline
	{
	public:
		void execute(graphics_manager* graphics, sprite_light_component& sprite) override
		{
			graphics->get_device_context()->IASetInputLayout(sprite.m_vertex_shader.GetInputLayout());
			graphics->get_device_context()->PSSetShader(sprite.m_pixel_shader.GetShader(), NULL, 0);
			graphics->get_device_context()->VSSetShader(sprite.m_vertex_shader.GetShader(), NULL, 0);

			graphics->vs_set_constant_buffers(0, 1, sprite.cb_vs_vertexshader_2d->GetAddressOf());

			graphics->ps_set_shader_resources(0, 1, sprite.m_texture->GetTextureResourceViewAddress());
			graphics->get_device_context().Get()->PSSetSamplers(sprite.m_sampler->m_start_slot, 1u, sprite.m_sampler->m_sampler_state.GetAddressOf());

			const UINT offsets = 0;

			graphics->get_device_context()->IASetVertexBuffers(0, 1, graphics->m_vertices.GetAddressOf(), graphics->m_vertices.StridePtr(), &offsets);
			graphics->get_device_context()->IASetIndexBuffer(graphics->m_indices.Get(), DXGI_FORMAT::DXGI_FORMAT_R32_UINT, 0);
			graphics->get_device_context()->DrawIndexed(graphics->m_indices.IndexCount(), 0, 0);

			ID3D11ShaderResourceView* null[] = { nullptr };
			graphics->get_device_context()->PSSetShaderResources(0, 1, null);
		}
	};
}