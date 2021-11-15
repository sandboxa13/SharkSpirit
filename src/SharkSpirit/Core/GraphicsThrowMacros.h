#pragma once

#include "Render/DirectX/GraphicsManager.h"

// HRESULT hr should exist in the local scope for these macros to work

#define GFX_EXCEPT_NOINFO(hr) shark_spirit::render::device::HrException( __LINE__,__FILE__,(hr) )
#define GFX_THROW_NOINFO(hrcall) if( FAILED( hr = (hrcall) ) ) throw shark_spirit::render::device::HrException( __LINE__,__FILE__,hr )

#ifdef _DEBUG
#define GFX_EXCEPT(hr) shark_spirit::render::device::HrException( __LINE__,__FILE__,(hr),m_info_manager.GetMessages() )
#define GFX_THROW_INFO(hrcall) m_info_manager.Set(); if( FAILED( hr = (hrcall) ) ) throw GFX_EXCEPT(hr)
#define GFX_DEVICE_REMOVED_EXCEPT(hr) shark_spirit::render::device::DeviceRemovedException( __LINE__,__FILE__,(hr),m_info_manager.GetMessages() )
#define GFX_THROW_INFO_ONLY(call) m_info_manager.Set(); (call); {auto v = m_info_manager.GetMessages(); if(!v.empty()) {throw shark_spirit::render::device::InfoException( __LINE__,__FILE__,v);}}
#else
#define GFX_EXCEPT(hr)  shark_spirit::render::device::HrException( __LINE__,__FILE__,(hr) )
#define GFX_THROW_INFO(hrcall) GFX_THROW_NOINFO(hrcall)
#define GFX_DEVICE_REMOVED_EXCEPT(hr)  shark_spirit::render::device::DeviceRemovedException( __LINE__,__FILE__,(hr) )
#define GFX_THROW_INFO_ONLY(call) (call)
#endif

// macro for importing infomanager into local scope
// this.GetInfoManager(Graphics& gfx) must exist
#ifdef _DEBUG
#define INFOMAN(gfx) HRESULT hr
#else
#define INFOMAN(gfx) HRESULT hr; DxgiInfoManager& infoManager = GetInfoManager((gfx))
#endif

#ifdef _DEBUG
#define INFOMAN_NOHR(gfx)
#else
#define INFOMAN_NOHR(gfx) DxgiInfoManager& infoManager = GetInfoManager((gfx))
#endif