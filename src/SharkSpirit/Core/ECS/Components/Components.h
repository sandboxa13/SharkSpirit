#pragma once

#include <DirectXMath.h>

namespace SharkSpirit
{
	struct SpriteComponent
	{
		DirectX::XMFLOAT4 m_color;
	};

	struct TransformComponent
	{
		DirectX::XMFLOAT3 m_pos;
		DirectX::XMFLOAT3 m_rotation;
	};
}