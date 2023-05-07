#include "stdafx.h"
#pragma once
#include "PointCountingObserver.h"
#include "GameObject.h"
#include "RigidbodyComponent.h"

void PointCountingObserver::notify(GameObject& gameObject)
{
	if (gameObject.getType() == "BallObject") {
		printf("%s's point counter: %i\n", m_playerName.c_str(), ++m_counter);
		gameObject.resetPosition();
	}
}

PointCountingObserver::PointCountingObserver(std::string playerName_)
{
	m_playerName = playerName_;
}