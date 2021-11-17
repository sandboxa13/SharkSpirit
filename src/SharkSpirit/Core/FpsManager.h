#pragma once

namespace sharkspirit::core
{
	class fps_manager {
	private:
		float m_fps;

	public:
		void calculate(float totalTime)
		{
			static int frames_count = 0;
			static float timeElasped = 0.0f;

			frames_count++;

			if (totalTime - timeElasped >= 1.0f)
			{
				m_fps = (float)frames_count / 1;

				frames_count = 0;
				timeElasped += 1.0f;
			}
		}

		float get_fps() { return m_fps; }
	};
}