#pragma once

struct player_input_component
{
	player_input_component(float walkSpeed)
	{
		m_walk_speed = walkSpeed;
	}

	float m_walk_speed;
};