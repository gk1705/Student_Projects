#include "stdafx.h"
#include "APlayerState.h"

void APlayerState::playDirectionalAnimation(const AController& controller, const Vector2i& currentMovementDirection) const
{
	m_dirAnimator->playDirectionalAnimation(controller, currentMovementDirection);
}
