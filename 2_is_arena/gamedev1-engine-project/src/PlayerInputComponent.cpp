#include "stdafx.h"
#pragma once
#include "InputManager.h"
#include "GameObject.h"
#include "RigidbodyComponent.h"
#include <string>
#include "PlayerInputComponent.h"

void PlayerInputComponent::Update(float fDeltaTime) {
	if (InputManager::getInstance().IsKeyPressed("moveUp", m_id))
	{
		if (m_gameObject.getRigidbody())
		{
			m_gameObject.getRigidbody()->velocity.y = -m_speed;
		}
	}
	else if (InputManager::getInstance().IsKeyPressed("moveDown", m_id))
	{
		if (m_gameObject.getRigidbody())
		{
			m_gameObject.getRigidbody()->velocity.y = m_speed;
		}
	}
	else
	{
		m_gameObject.getRigidbody()->velocity.y = 0;
	}
}
