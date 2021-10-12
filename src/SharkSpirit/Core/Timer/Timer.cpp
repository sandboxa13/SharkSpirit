#include "Timer.h"

namespace SharkSpirit
{
	Timer::Timer()
		: mBaseTime(0), mCurrentTime(0), mPrevTime(0), mDeltaTime(0.0)
	{
		__int64 countsPerSec;
		QueryPerformanceFrequency((LARGE_INTEGER*)&countsPerSec);

		mSecondsPerCount = 1 / (double)countsPerSec;
	}

	float Timer::DeltaTime() const noexcept
	{
		return (float)mDeltaTime;
	}

	float Timer::TotalTime()
	{
		return static_cast<float>((mCurrentTime - mBaseTime) * mSecondsPerCount);
	}

	void Timer::Reset()
	{
		__int64 currentTime;
		QueryPerformanceCounter((LARGE_INTEGER*)&currentTime);

		mPrevTime = currentTime;
		mBaseTime = currentTime;

		Logger::LogInfo("Reset timer");
	}

	void Timer::Tick()
	{
		__int64 currentTime;
		QueryPerformanceCounter((LARGE_INTEGER*)&currentTime);

		mCurrentTime = currentTime;

		mDeltaTime = (mCurrentTime - mPrevTime) * mSecondsPerCount;

		mPrevTime = mCurrentTime;

		if (mDeltaTime < 0.0) {
			mDeltaTime = 0.0;
		}
	}
}