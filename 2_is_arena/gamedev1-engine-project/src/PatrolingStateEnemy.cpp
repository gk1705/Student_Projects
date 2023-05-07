#include "stdafx.h"
#include "PatrolingStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"
#include "BoxColliderComponent.h"

bool PatrolingStateEnemy::isDead(EnemyController& controller)
{
	// enemy dies
	if (controller.isHpZero())
	{
		controller.ChangeState("dead");
		return true;
	}
	return false;
}

bool PatrolingStateEnemy::isCollidingBorder(EnemyController& controller)
{
	// enemy is stunned
	// only used by boar
	// if boar runs into a border, trigger this state;
	if (controller.isCollidingBorder())
	{
		controller.ChangeState("stunned");
		return true;
	}
	return false;
}

void PatrolingStateEnemy::checkSightOverlap(EnemyController& controller, shared_ptr<GameObject>& target)
{
	target = PlayFieldManager::getInstance().getPlayerCurrentlyAt(PlayFieldManager::getInstance().objectCurrentlyAt(controller.getGObject().getID()));

	// get boxcollider -> check intersection
	auto box1 = target->getCollisionComponents()[0]->GetShape();
	auto box2 = controller.getGObject().getCollisionComponents()[0]->GetShape();

	// intersection on x axis?
	// intersection on y axis?
	if (box1.left >= box2.left && box1.left <= box2.left + box2.width)
	{
		m_enemyInSight = true;
	}
	else if (box1.left + box1.width <= box2.left + box2.width && box1.left + box1.width >= box2.left)
	{
		m_enemyInSight = true;
	}
	else if (box1.top >= box2.top && box1.top <= box2.top + box2.height)
	{
		m_enemyInSight = true;
	}
	else if (box1.top + box1.height <= box2.top + box2.height && box1.top + box1.height >= box2.top)
	{
		m_enemyInSight = true;
	}
}

bool PatrolingStateEnemy::isInSight(EnemyController& controller, shared_ptr<GameObject> target)
{
	// if yes -> change state to chase
	if (m_enemyInSight) 
	{
		auto enemyToPlayerVec = target->GetPosition() - controller.getGObject().GetPosition();
		normalizeVec(enemyToPlayerVec);
		controller.getGObject().getRigidbody()->velocity = enemyToPlayerVec;
		controller.ChangeState("chase");
		return true;
	}
	return false;
}

void PatrolingStateEnemy::checkDirectionChange(EnemyController& controller, const Vector2i& normDirection)
{
	// change animation based on new direction
	if (m_lastNormDir != normDirection)
	{
		onEnter(controller, normDirection);
	}
}

void PatrolingStateEnemy::move(EnemyController& controller)
{
	controller.getGObject().getRigidbody()->velocity = Vector2f(m_lastNormDir) * controller.getSpeed() * m_speedModifier;
}

void PatrolingStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	m_enemyInSight = false;
	shared_ptr<GameObject> target;

	if (isDead(controller)) return;
	if (isCollidingBorder(controller)) return;

	checkSightOverlap(controller, target);
	if (isInSight(controller, target)) return;

	checkDirectionChange(controller, normDirection);
	move(controller);
}

void PatrolingStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
{
	// suggestion: randomize direction on entry
	playDirectionalAnimation(controller, normDirection);
	m_lastNormDir = normDirection;
}
