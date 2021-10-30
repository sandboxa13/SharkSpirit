#pragma once

#include <DirectXMath.h>
#include "Render/DirectX/Texture.h"

namespace SharkSpirit
{
	struct SpriteComponent
	{
		Texture m_texture;
	};

	struct TransformComponent
	{
		DirectX::XMFLOAT3 m_pos;
		DirectX::XMFLOAT3 m_rotation;
	};
}