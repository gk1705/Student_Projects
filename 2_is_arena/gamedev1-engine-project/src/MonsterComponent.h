#pragma once
#include "stdafx.h"
#include "IComponent.h"

class MonsterComponent : public IComponent
{
public:
	MonsterComponent(GameObject& gameObject, std::string targetObject)
		:IComponent(gameObject), 
		m_targetObject(targetObject)
	{

	}

	void Update(float fDeltaTime) override;

private:
	std::string m_targetObject;
};