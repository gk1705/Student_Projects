#include "stdafx.h"
#include "AEnemyState.h"

void AEnemyState::playDirectionalAnimation(const AController& controller,
	const Vector2i& currentMovementDirection) const
{
	m_dirAnimator->playDirectionalAnimation(controller, currentMovementDirection);
}