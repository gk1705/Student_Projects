#pragma once
#include "InputManager.h"
#include "DirectionalAnimator.h"

class EnemyController;

class AEnemyState
{
public:
	virtual void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) = 0;
	virtual void onEnter(EnemyController& controller, const Vector2i& normDirection) = 0;

	virtual ~AEnemyState() = default;

	AEnemyState(const std::string& animName, const float frameDuration)
		: m_dirAnimator(std::make_shared<DirectionalAnimator>(DirectionalAnimator(animName, frameDuration)))
	{

	}

protected:
	void playDirectionalAnimation(const AController& controller, const Vector2i& currentMovementDirection) const;

private:
	shared_ptr<DirectionalAnimator> m_dirAnimator;
};