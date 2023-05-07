#include "stdafx.h"
#include "GameObject.h"
#include "HealthObserver.h"
#include "PlayerStatsComponent.h"

HealthObserver::HealthObserver(shared_ptr<HealthBarComponent> healtbarcomponent)
	: m_healthbarcomp(healtbarcomponent)
{

}

void HealthObserver::notify(GameObject& gameObject)
{
	float scale = gameObject.getComponent<PlayerStatsComponent>()->getHP() / 
		static_cast<float>(gameObject.getComponent<PlayerStatsComponent>()->getMaxHP());
	if (scale < 0) scale = 0;
	m_healthbarcomp->setScale(scale);
}