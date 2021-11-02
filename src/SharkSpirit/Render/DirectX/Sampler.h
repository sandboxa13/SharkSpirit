#pragma once
#include <d3d11.h>
#include <DirectXMath.h>
#include <Render/DirectX/GraphicsManager.h>

namespace SharkSpirit
{
	class Sampler 
	{
	public :
		Sampler(graphics_manager* graphicsManager, unsigned int startSlot = 0)
			: m_start_slot(startSlot)
		{
			D3D11_SAMPLER_DESC samplerDesc = {};
			samplerDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
			samplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
			samplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
			samplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;

			graphicsManager->get_device()->CreateSamplerState(&samplerDesc, &m_sampler_state);
		}

		ComPtr<ID3D11SamplerState> m_sampler_state;
		unsigned int m_start_slot;
	};
}