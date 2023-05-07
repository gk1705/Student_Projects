#include "stdafx.h"
#include "DeathStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"
#include "BoxColliderComponent.h"
#include "SoundManager.h"

void DeathStateEnemy::switchPlayfield(EnemyController& controller, string& playField)
{
	playField = PlayFieldManager::getInstance().objectCurrentlyAt(controller.getGObject().getID());
	// swap to other playfield
	playField = playField == "Field1" ? "Field2" : "Field1";
	PlayFieldManager::getInstance().objectSwitchPlayfield(controller.getGObject().getID());
}

void DeathStateEnemy::setSpawnPosition(EnemyController& controller, string playField)
{
	// calculate new spawnPosition -> unfair for player if enemy spawns too close
	const auto newSpawnPos = PlayFieldManager::getInstance().returnSpawnPosition(playField, controller.getGObject().GetPosition());
	// assign new position to enemy
	controller.getGObject().setPosition(Vector2f(newSpawnPos));
}

void DeathStateEnemy::resetObject(EnemyController& controller)
{
	// in order to move the boxCollider to the other field, we need to call the update function of the collision component
	// we enable the collider again when we call the changestate function of the enemyController
	// though this happens before we change the position of the collider via update, therefor the player would sustain a hit on the last frame
	// if we update beforehand though, this doesn't happen
	controller.getGObject().getComponent<BoxColliderComponent>()->Update(0);
	// reset the enemy's hp when teleporting to the other playfield
	controller.resetHP();

	// enemy should be facing down when re spawning -> therefor we set velocity accordingly
	// change state evaluates the current velocity of the attached object
	// this is how animation direction is determined
	controller.getGObject().getRigidbody()->velocity = Vector2f(0, 1);
	controller.ChangeState("idle");
}

void DeathStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	m_timePassed += deltaTime;

	if (m_timePassed >= m_switchTime)
	{
		string playField;
		switchPlayfield(controller, playField);
		setSpawnPosition(controller, playField);
		resetObject(controller);

		m_timePassed = 0;
	}
}

void DeathStateEnemy::triggerDeathSound(EnemyController& controller)
{
	// play death sound ( currently only bat )
	string enemyType = controller.getGObject().getID().substr(0, controller.getGObject().getID().size() - 1);
	if (enemyType == "bat")
	{
		SoundManager::getInstance().PlaySound("bat_death", "first");
	}
	else if (enemyType == "boar") 
	{
		SoundManager::getInstance().PlaySound("boar_death", "first");
	}
}

void DeathStateEnemy::onEnter(EnemyController& controller, const Vector2i& normDirection)
{
	playDirectionalAnimation(controller, normDirection);
	controller.getGObject().getRigidbody()->velocity = Vector2f(0, 0);
	triggerDeathSound(controller);

	// disable collider on entry
	// we don't want the player to get hit by a defeated enemy
	controller.getGObject().getCollisionComponents()[0]->SetDisabled(true);
}
