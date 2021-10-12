#pragma once

namespace SharkSpirit 
{
	class RenderEngine
	{
	public:
		RenderEngine() {};
		virtual ~RenderEngine() = default;

		void Render();
	};
}
