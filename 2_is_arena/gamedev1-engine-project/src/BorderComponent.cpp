#include "stdafx.h"
#pragma once
#include "BorderComponent.h"
#include "GameObject.h"
#include "BoxColliderComponent.h"

void BorderComponent::horizontalCollisionCheck(shared_ptr<BoxColliderComponent> collisioncomp)
{
	if (m_gameObject.getColliderShape().left >= m_border.left + m_border.width - collisioncomp->GetShape().width)
	{
		m_gameObject.GetPosition().x = m_border.left + m_border.width - (collisioncomp->GetShape().width + collisioncomp->GetOffset().x);
	}
	else if (m_gameObject.GetPosition().x + collisioncomp->GetOffset().x < m_border.left)
	{
		m_gameObject.GetPosition().x = m_border.left - collisioncomp->GetOffset().x;
	}
}

void BorderComponent::verticalCollisionCheck(shared_ptr<BoxColliderComponent> collisioncomp)
{
	if (collisioncomp->GetShape().top >= m_border.top + m_border.height - collisioncomp->GetShape().height)
	{
		m_gameObject.GetPosition().y = m_border.top + m_border.height - (collisioncomp->GetShape().height + collisioncomp->GetOffset().y);
	}
	else if (m_gameObject.GetPosition().y + collisioncomp->GetOffset().y < m_border.top)
	{
		m_gameObject.GetPosition().y = m_border.top - collisioncomp->GetOffset().y;
	}
}

void BorderComponent::Update(float fDeltaTime)
{	
	shared_ptr<BoxColliderComponent> collisioncomp = m_gameObject.getCollisionComponents()[0];

	horizontalCollisionCheck(collisioncomp);

	verticalCollisionCheck(collisioncomp);
}

BorderComponent::BorderComponent(GameObject& gameObject, FloatRect border)
	: IComponent(gameObject)
	, m_border(border)
{
}