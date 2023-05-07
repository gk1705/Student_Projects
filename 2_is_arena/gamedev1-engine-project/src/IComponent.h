#pragma once

#include <memory>

class GameObject;

class IComponent
{
public:
	typedef std::shared_ptr<IComponent> Ptr;

	IComponent(GameObject& gameObject)
		: m_gameObject(gameObject)
	{
	}

	virtual ~IComponent() = default;

	virtual void Update(float fDeltaTime) = 0;

protected:
	GameObject& m_gameObject;
};