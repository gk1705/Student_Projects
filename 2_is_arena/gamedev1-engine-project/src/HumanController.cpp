#include "stdafx.h"
#include "HumanController.h"
#include "InputManager.h"
#include "RenderManager.h"
#include "GameObjectManager.h"
#include "UpdateManager.h"
#include "PlayerStatsComponent.h"
#include "ProjectileSpawner.h"

void HumanController::Update(const float& fDeltaTime)
{
	// player is locked if he/she enters death state
	// no more input (moving/shooting possible)
	if (!lock) {
		unlockedUpdate(fDeltaTime);
	}
	else {
		lockedUpdate(fDeltaTime);
	}
}

void HumanController::unlockedUpdate(const float& fDeltaTime)
{
	handleMovement();
	getRightJoystickDir();

	m_lastDirNormLeft = findNormDirection(m_lastDirLeft);
	m_lastDirNormRight = findNormDirection(m_lastDirRight);
	m_currentState->handleState(*this, InputManager::getInstance().getLeftJoystick(m_id), m_lastDirNormLeft, fDeltaTime);

	m_attackspeed += fDeltaTime;
	shooting();
}

void HumanController::lockedUpdate(const float& fDeltaTime)
{
	m_gameObject.getRigidbody()->velocity = Vector2f(0, 0);
	m_currentState->handleState(*this, InputManager::getInstance().getLeftJoystick(m_id), m_lastDirNormLeft, fDeltaTime);
}

// predefined states for both players
void HumanController::ChangeState(const string& stateName)
{
	const auto keyExists = m_states.find(stateName);
	if (keyExists == m_states.end()) { //does not exist
		throw std::invalid_argument("received wrong stateName");
	}

	m_currentState = keyExists->second;
	m_currentState->onEnter(*this, m_lastDirNormLeft, m_lastDirNormRight);
}

// used to find out in which direction we are attacking
// if right stick was not used during frame last known position is used (feels more natural that way)
// we do not update our direction variable if right stick wasnt used
void HumanController::getRightJoystickDir()
{
	const auto rightStickTilt = InputManager::getInstance().getRightJoystick(m_id);
	if (rightStickTilt != Vector2f(0, 0)) 
	{
		m_lastDirRight = rightStickTilt;
	}
}

// movement; we only update the last normDirection (up, down, left, right) if velocity isn't zero
// this prevents us from snapping to default (right) when we're not moving
// we want to trigger idle in the direction we last moved in
void HumanController::handleMovement()
{
	//Movement
	const auto leftStickTilt = InputManager::getInstance().getLeftJoystick(m_id);
	m_gameObject.getRigidbody()->velocity =  leftStickTilt * m_stats->getMovementSpeed();

	if (leftStickTilt.x != 0 || leftStickTilt.y != 0)
	{
		m_lastDirLeft = leftStickTilt;
	}
}

// currently not using pooling for projectiles
void HumanController::SpawnProjectile(Vector2f velocity)
{
	ProjectileSpawner::spawnProjectile(getGObject(), m_hero, m_count, m_id, *m_stats, velocity);
	m_attackspeed = 0;
}

bool HumanController::checkShooting()
{
	// regular attack
	if (InputManager::getInstance().IsButtonPressed("RB", m_id) 
		&& m_attackspeed >= m_stats->getMaxAttackSpeed()) 
	{
		SpawnProjectile(m_lastDirRight);
		return true;
	}
	return false;
}

bool HumanController::checkUltimate()
{
	// ultimate
	if ((InputManager::getInstance().IsButtonPressed("A", m_id) 
			|| InputManager::getInstance().IsButtonPressed("LB", m_id)) 
		&& m_stats->UltimateReady())
	{
		UseUltimate();
		return true;
	}
	return false;
}

void HumanController::shooting() {

	if (checkShooting()) return;
	if (checkUltimate()) return;
}

void HumanController::UseUltimate()
{
	ProjectileSpawner::spawnUltimate(getGObject(), m_hero, m_count, m_id, *m_stats);
	m_stats->resetUltimate();
}

float HumanController::getAttackSpeed() const
{
	return m_attackspeed;
}

float HumanController::getAttackSpeedMAX() const
{
	return m_attackspeedMAX;
}

void HumanController::LockPlayer()
{
	lock = true;
}