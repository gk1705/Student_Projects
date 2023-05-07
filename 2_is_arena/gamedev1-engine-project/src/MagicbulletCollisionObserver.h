#pragma once
#include "IObserverComponent.h"

class GameObject;

class MagicBulletCollisionObserver : public IObserverComponent
{
public:
	MagicBulletCollisionObserver(std::string thisID, std::string ownerID);

	void notify(GameObject& gameObject) override;

private:
	std::string m_thisID;
	std::string m_ownerID;
};