#include "stdafx.h"
#include "IObserverComponent.h"
#pragma once

class GameObject;

class PointCountingObserver : public IObserverComponent
{
public:
	PointCountingObserver(std::string playerName);

	void notify(GameObject& gameObject) override;

private:
	std::string m_playerName;
	int m_counter = 0;
};