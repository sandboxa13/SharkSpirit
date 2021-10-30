#pragma once
#include <vector>
#include <dxgidebug.h>
#include <string>
#include <wrl.h>

using namespace Microsoft::WRL;

class DxgiInfoManager
{
public:
	DxgiInfoManager();
	~DxgiInfoManager() = default;
	DxgiInfoManager(const DxgiInfoManager&) = delete;
	DxgiInfoManager& operator=(const DxgiInfoManager&) = delete;
	void Set() noexcept;
	std::vector<std::string> GetMessages() const;
private:
	unsigned long long next = 0u;
	ComPtr<IDXGIInfoQueue> pDxgiInfoQueue;
};