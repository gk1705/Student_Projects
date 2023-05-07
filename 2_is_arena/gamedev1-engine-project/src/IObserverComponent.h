#pragma once

class GameObject;

class IObserverComponent
{
public:
	virtual void notify(GameObject& gameObject) = 0;

	virtual ~IObserverComponent()
	{

	}
};