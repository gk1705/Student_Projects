#include "stdafx.h"
#include "IGraphicsComponent.h"


float IGraphicsComponent::getWidth() const
{
	return rectangle.getSize().x;
}

float IGraphicsComponent::getHeight() const
{
	return rectangle.getSize().y;
}

void IGraphicsComponent::setHeight(float height)
{
	rectangle.setSize(Vector2f(getWidth(), height));
}
