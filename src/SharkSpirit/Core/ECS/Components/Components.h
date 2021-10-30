#pragma once

#include <DirectXMath.h>

namespace SharkSpirit
{
	struct SpriteComponent
	{

	};


	struct TransformComponent
	{
		DirectX::XMFLOAT3 m_pos;
		DirectX::XMFLOAT3 m_rotation;
	};
}