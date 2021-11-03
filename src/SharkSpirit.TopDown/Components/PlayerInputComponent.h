#pragma once

struct player_input_component
{
	player_input_component(float walkSpeed, float acceleration)
		: m_walk_speed(walkSpeed), m_acceleration(acceleration)
	{
	}

	float m_walk_speed;
	float m_acceleration;
};