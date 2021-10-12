#pragma once

namespace SharkSpirit 
{
	class IInitializable
	{
	public:
		virtual ~IInitializable() = default;
		virtual void Initialize(){}
	};
}
