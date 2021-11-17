#pragma once

namespace sharkspirit::core
{
	class IInitializable
	{
	public:
		virtual ~IInitializable() = default;
		virtual void Initialize(){}
	};
}
