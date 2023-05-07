#include "stdafx.h"
#include "IObserverComponent.h"
#pragma once

class GameObject;

class ItemPickedUpObserver : public IObserverComponent
{
public:
	ItemPickedUpObserver(std::string playerName);

	void notify(GameObject& gameObject) override;

private:
	std::string m_playerName;
	int m_counter = 0;
};