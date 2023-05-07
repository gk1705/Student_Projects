#include "stdafx.h"
#include "AController.h"

void AController::setID(int id)
{
	m_id = id;
}

GameObject& AController::getGObject() const
{
	return m_gameObject;
}

float AController::getSpeed() const
{
	return m_stats->getMovementSpeed();
}

float AController::getID() const
{
	return m_id;
}

bool AController::isHpZero() const
{
	return m_stats->getHP() <= 0;
}

void AController::resetHP() const
{
	m_stats->resetStats();
}

// checks what the input vector's direction is closest to in regards to up down left right
Vector2i AController::findNormDirection(const Vector2f& inputVector)
{
	// right as default -> right when input is zero
	Vector2i dirNorm(1, 0);
	float dot_directionComparison = dotProduct(Vector2f(dirNorm), inputVector);
	// left
	if (dotProduct(Vector2f(-1, 0), inputVector) > dot_directionComparison)
	{
		dirNorm = Vector2i(-1, 0);
		dot_directionComparison = dotProduct(Vector2f(-1, 0), inputVector);
	}
	// down
	if (dotProduct(Vector2f(0, 1), inputVector) > dot_directionComparison)
	{
		dirNorm = Vector2i(0, 1);
		dot_directionComparison = dotProduct(Vector2f(0, 1), inputVector);
	}
	// up
	if (dotProduct(Vector2f(0, -1), inputVector) > dot_directionComparison)
	{
		dirNorm = Vector2i(0, -1);
		dot_directionComparison = dotProduct(Vector2f(0, -1), inputVector);
	}

	return dirNorm;
}
