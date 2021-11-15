#include "Texture.h"
#include <Utils/WICTextureLoader.h>
#include <Utils/StringUtils.h>

namespace SharkSpirit
{
	HRESULT Texture::initialize(shark_spirit::render::device* device, const std::string& filePath)
	{
		return CreateWICTextureFromFile(device->get_device().Get(), device->get_device_context().Get(), string_to_wide(filePath).c_str(), texture.GetAddressOf(), textureView.GetAddressOf());
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