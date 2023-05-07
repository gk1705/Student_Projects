#pragma once
#include "stdafx.h"
#include "GameObject.h"
#include "PlayerStatsComponent.h"
#include "PlayFieldManager.h"
#include "GameObjectManager.h"
#include "AController.h"
#include "MovementComponent.h"
#include "SoundManager.h"

PlayerStatsComponent::PlayerStatsComponent(GameObject& gameObject, const int maxHP)
	: IComponent(gameObject)
	, m_MAX_HP(maxHP)
	, m_MAX_attackspeed(0.7)
	, m_MAX_movementspeed(3.5f)
	, m_HP(maxHP)
	, m_movementspeed(3.5f)
{
	//set to higher value so that players are not detected as slowed or damaged at the beginning of the game
	m_slowedTimer = 10;
	m_invincibleTimer = 10;
}

void PlayerStatsComponent::Update(float fDeltaTime)
{
	if (m_slowedTimer > 3) {
		m_movementspeed = m_MAX_movementspeed;
	}
	else {
		m_slowedTimer += fDeltaTime;
	}

	if (m_receiveHealing && m_HP < m_MAX_HP) {
		m_receiveHealingTimer += fDeltaTime;
	}

	if (m_receiveHealingTimer >= 0.5f) {
		m_HP += 5;
		notifyObservers();
		m_receiveHealingTimer = 0;
	}

	m_receiveHealing = false;
	m_invincibleTimer += fDeltaTime;
}

void PlayerStatsComponent::registerObserver(shared_ptr<IObserverComponent> observer)
{
	m_observer.push_back(observer);
}

int PlayerStatsComponent::getHP() const
{
	return m_HP;
}

int PlayerStatsComponent::getMaxHP() const
{
	return m_MAX_HP;
}

int PlayerStatsComponent::getMaxUltimate() const
{
	return m_MAX_ultimate;
}

float PlayerStatsComponent::getMaxAttackSpeed() const
{
	return m_MAX_attackspeed;
}

float PlayerStatsComponent::getMovementSpeed() const
{
	return m_movementspeed;
}

int PlayerStatsComponent::getUltimate() const
{
	return m_ultimate;
}

bool PlayerStatsComponent::UltimateReady() const
{
	return m_ultimate >= m_MAX_ultimate;
}

bool PlayerStatsComponent::getIsDead() const
{
	return m_isDead;
}

float PlayerStatsComponent::getProjectileSpeed() const
{
	return m_projectileSpeed;
}

bool PlayerStatsComponent::IsDamaged() const
{
	return m_invincibleTimer < 0.2f; //Show for a short time that player got damaged
}

bool PlayerStatsComponent::IsSlowed() const
{
	return m_slowedTimer < 3;
}

void PlayerStatsComponent::getDamage(const int damage)
{
	if (m_invincibleTimer > 1.5f || m_gameObject.getType() == "Enemy") {
		m_HP -= damage;
		notifyObservers();
		m_invincibleTimer = 0;

		// player was hit
		if (m_gameObject.getType() == "PlayerObject") 
		{
			SoundManager::getInstance().PlaySound("Player_hit" + std::to_string(generateIntFromTo(1, 8)), "second");
		}
		// enemy was hit
		else if (m_gameObject.getType() == "Enemy") 
		{
			SoundManager::getInstance().PlaySound("Enemy_hit" + std::to_string(generateIntFromTo(1, 2)), "first");
		}
	}

	if (m_gameObject.getType() == "PlayerObject" && !(m_gameObject.getComponent<PlayerStatsComponent>()->getHP() <= 0))
	{
		m_gameObject.getMovementComponent()->getStrategy().ChangeState("hit");
	}	
}

void PlayerStatsComponent::getUltimatePoints(const int value)
{
	m_ultimate += value;
	notifyObservers();
}

void PlayerStatsComponent::getSlowed(const float percentage)
{
	m_slowedTimer = 0.0f;
	if (m_movementspeed >= m_MAX_movementspeed * 0.2 && m_movementspeed <= m_MAX_movementspeed * 2.5) {
		m_movementspeed = m_movementspeed * percentage;
	}

}

void PlayerStatsComponent::setMovementSpeed(const float movementspeed)
{
	m_MAX_movementspeed = movementspeed;
	m_movementspeed = movementspeed;

}

void PlayerStatsComponent::setAttackSpeed(const float attackspeed)
{
	m_MAX_attackspeed = attackspeed;
}

void PlayerStatsComponent::resetUltimate()
{
	m_ultimate = 0;

	notifyObservers();
}

void PlayerStatsComponent::setIsDead(const bool isDead)
{
	m_isDead = isDead;
}

void PlayerStatsComponent::receiveHealing()
{
	m_receiveHealing = true;
}

void PlayerStatsComponent::setProjectileSpeed(const float speed)
{
	m_projectileSpeed = speed;
}

void PlayerStatsComponent::notifyObservers()
{
	for (auto obj : m_observer) {
		obj->notify(m_gameObject);
	}
}

void PlayerStatsComponent::resetStats()
{
	m_HP = m_MAX_HP;
	notifyObservers();
}