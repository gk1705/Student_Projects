#include "stdafx.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"
#include "BoxColliderComponent.h"

//...
void EnemyController::Update(const float& fDeltaTime)
{
	if (m_gameObject.GetPosition() == Vector2f(-1000, -1000)) return; //Spawnposition, monster should do nothing when on spawnpos

	if (!m_currentState) 
	{
		throw std::invalid_argument("Current state was invalid.");
	}

	if (!(getGObject().getRigidbody()->velocity == Vector2f(0, 0)))
		m_lastDirNorm = findNormDirection(getGObject().getRigidbody()->velocity);

	m_currentState->handleState(*this, m_lastDirNorm, fDeltaTime);
}

// change to registered state
void EnemyController::ChangeState(const string& stateName)
{
	// if collider is disabled due to enemy being slain by player, and subsequently switching field -> enable collider as soon as we switch to idle state
	if (getGObject().getCollisionComponents()[0]->GetDisabled())
	{
		getGObject().getCollisionComponents()[0]->SetDisabled(false);
	}

	const auto keyExists = m_states.find(stateName);
	if (keyExists == m_states.end()) { //does not exist
		throw std::invalid_argument("received wrong stateName");
	}

	// if we're stunned we won't update our norm direction
	// norm direction is used to determine what animation we play on state entry
	// we do not update the norm direction on stunned:
	// when an enemy is stunned its velocity is set to zero, this would mean the norm direction would snap to its default, right
	// we want the enemy facing in the direction it was facing when entering the stunned state, therefor we do not update the norm direction
	if (stateName != "stunned")
		m_lastDirNorm = findNormDirection(getGObject().getRigidbody()->velocity);
	m_currentState = keyExists->second;
	m_currentState->onEnter(*this, m_lastDirNorm);
}

void EnemyController::AddState(const shared_ptr<AEnemyState>& state, const string& stateName)
{
	m_states.insert({ stateName, state });
}

void EnemyController::setNormDir(const Vector2f& inputVector)
{
	m_lastDirNorm = findNormDirection(inputVector);
}

bool EnemyController::horizontalCollisionCheck(Rect<float> borderRect, const shared_ptr<BoxColliderComponent>& collisioncomp) const
{
	// horizontal check
	if (m_gameObject.getColliderShape().left >= borderRect.left + borderRect.width - collisioncomp->GetShape().width)
	{
		m_gameObject.GetPosition().x = borderRect.left + borderRect.width - (collisioncomp->GetShape().width + collisioncomp->GetOffset().x);
		return true;
	}
	else if (m_gameObject.GetPosition().x + collisioncomp->GetOffset().x < borderRect.left)
	{
		m_gameObject.GetPosition().x = borderRect.left - collisioncomp->GetOffset().x;
		return true;
	}
	return false;
}

bool EnemyController::verticalCollisionCheck(const Rect<float> borderRect, const shared_ptr<BoxColliderComponent>& collisioncomp) const
{
	// vertical check
	if (collisioncomp->GetShape().top >= borderRect.top + borderRect.height - collisioncomp->GetShape().height)
	{
		m_gameObject.GetPosition().y = borderRect.top + borderRect.height - (collisioncomp->GetShape().height + collisioncomp->GetOffset().y);
		return true;
	}
	else if (m_gameObject.GetPosition().y + collisioncomp->GetOffset().y < borderRect.top)
	{
		m_gameObject.GetPosition().y = borderRect.top - collisioncomp->GetOffset().y ;
		return true;
	}
	return false;
}

// we check if the enemy is currently colliding with the border of the respective playfield
// query playfield name
// get border rectangle based on playfield name
// get boxcollider of our enemy Object (isCollidingBorder is currently only called by the boar enemy)
bool EnemyController::isCollidingBorder() const
{
	const auto playField = PlayFieldManager::getInstance().objectCurrentlyAt(m_gameObject.getID());
	const auto borderRect = playField == "Field1" 
		? PlayFieldManager::getInstance().getPlayfieldsRect()[0] 
			: PlayFieldManager::getInstance().getPlayfieldsRect()[1];
	
	const shared_ptr<BoxColliderComponent> collisioncomp = m_gameObject.getCollisionComponents()[0];
	return horizontalCollisionCheck(borderRect, collisioncomp) || verticalCollisionCheck(borderRect, collisioncomp);
}

Vector2i EnemyController::getNormDir() const
{
	return m_lastDirNorm;
}