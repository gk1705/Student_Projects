#include "stdafx.h"
#pragma once
#include "CameraComponent.h"
#include "GameObject.h"
#include "RigidbodyComponent.h"

void CameraComponent::Update(float deltatime) {
	sf::View view = m_window.getView();

	if (m_window.getDefaultView().getSize().x == m_window.getView().getSize().x) {
		view.zoom(m_zoom);
	}

	view.setCenter(m_target->getRigidbody()->getPosition() + sf::Vector2f(m_target->getWidth()/2.f, m_target->getHeight()/2.f));
	
	m_window.setView(view);
}

CameraComponent::CameraComponent(GameObject& gameObject, GameObject* target, sf::RenderWindow& window)
	: IComponent(gameObject)
	, m_window(window)
	, m_target(target)
{

}

void CameraComponent::Init(sf::RenderWindow& window) {
	sf::View view = window.getView();
	view.zoom(m_zoom);
	window.setView(view);
}