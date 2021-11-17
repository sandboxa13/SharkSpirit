#pragma once
#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include "Logger/Logger.h"
#include "Core/IInitializable.h"

namespace sharkspirit::core
{
	class Timer : public IInitializable
	{
	public:
		Timer();
		~Timer() = default;
		float DeltaTime() const noexcept;
		float TotalTime();

		void Reset();
		void Tick();

		virtual void Initialize()
		{
			sharkspirit::logger::Logger::LogInfo("Initialize Timer");

			Reset();
		}
	private:
		double mSecondsPerCount;
		double mDeltaTime;

		__int64 mBaseTime;
		__int64 mPrevTime;
		__int64 mCurrentTime;
	};
}
