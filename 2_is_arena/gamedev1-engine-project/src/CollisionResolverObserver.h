#include "stdafx.h"
#include "IObserverComponent.h"
#pragma once

class GameObject;

class CollisionResolverObserver : public IObserverComponent
{
public:
	CollisionResolverObserver(std::string playername);

	void notify(GameObject& gameObject) override;

private:
	std::string m_playerName;
	int m_counter = 0;
};
