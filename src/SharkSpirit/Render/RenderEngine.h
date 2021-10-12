#pragma once

class RenderEngine 
{
public :
	RenderEngine() {};
	virtual ~RenderEngine() = default;

	void Render();
};