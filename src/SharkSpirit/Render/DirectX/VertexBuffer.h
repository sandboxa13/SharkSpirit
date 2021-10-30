#pragma once
#include <DirectXMath.h>

namespace SharkSpirit
{
	struct vertex_2d 
	{
		vertex_2d() {}
		vertex_2d(float x, float y, float z, float u, float v)
			: pos(x, y, z), texCoord(u, v) {}

		DirectX::XMFLOAT3 pos;
		DirectX::XMFLOAT2 texCoord;
	};

	class vertex_buffer
	{

	};
}