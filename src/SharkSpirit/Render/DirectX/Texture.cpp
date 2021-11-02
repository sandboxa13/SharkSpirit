#include "Texture.h"
#include <Utils/WICTextureLoader.h>
#include <Utils/StringUtils.h>

namespace SharkSpirit
{
	HRESULT Texture::initialize(graphics_manager* graphicsManager, const std::string& filePath)
	{
		return CreateWICTextureFromFile(graphicsManager->get_device().Get(), graphicsManager->get_device_context().Get(), string_to_wide(filePath).c_str(), texture.GetAddressOf(), textureView.GetAddressOf());
	}

	ID3D11ShaderResourceView* Texture::GetTextureResourceView()
	{
		return this->textureView.Get();
	}

	ID3D11ShaderResourceView** Texture::GetTextureResourceViewAddress()
	{
		return this->textureView.GetAddressOf();
	}
}