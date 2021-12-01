#pragma once
#include "Core/ECS/Systems/ISystem.h"
#include <Input/InputProcessor.h>
#include <Core/ECS/Components/Components.h>

namespace sharkspirit::core
{
	class sprite_light_update_system : public ISystem
	{
	public:
		sprite_light_update_system(
			entt::registry* reg,
			sharkspirit::input::input_processor* input,
			sharkspirit::assets::assets_manager* assets) : ISystem(reg, input, assets)
		{
		}
		~sprite_light_update_system();

		void update() override
		{
			using namespace DirectX;

			auto spriteView = m_reg->view<sprite_light_component, transform_component>();
			auto camPtr = m_reg->try_ctx<sharkspirit::core::camera_component>();
			
			if (camPtr == NULL)
				return;

			for (auto entity : spriteView)
			{
				auto& sprite = spriteView.get<sprite_light_component>(entity);
				auto& transform = spriteView.get<transform_component>(entity);

				sprite.update_world_matrix(&transform);
				sprite.update_world_view_proj_matrix(camPtr);

				sprite.m_world_view_proj->CopyToGPU();
			}
		}
	};
}