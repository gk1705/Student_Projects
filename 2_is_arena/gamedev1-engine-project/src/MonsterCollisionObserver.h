#pragma once
#include "IObserverComponent.h"

class GameObject;

class MonsterCollisionObserver : public IObserverComponent
{
public:
	MonsterCollisionObserver(std::string thisID);

	void notify(GameObject& gameObject) override;

private:
	std::string m_thisID;
};