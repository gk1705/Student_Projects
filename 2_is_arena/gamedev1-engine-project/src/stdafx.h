// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <memory>
#include <string>
#include <list>
#include <vector>
#include <iostream>
#include <random>

#include <SFML/System.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Network.hpp>
#include <SFML/Audio.hpp>

inline void normalizeVec(sf::Vector2f& vector)
{
	float length = sqrt(vector.x * vector.x + vector.y * vector.y);

	if (length != 0) {
		vector.x /= length;
		vector.y /= length;
	}
}

inline float dotProduct(const sf::Vector2f& vec1, const sf::Vector2f& vec2)
{
	return vec1.x * vec2.x + vec1.y * vec2.y;
}

// taken from: https://stackoverflow.com/questions/5008804/generating-random-integer-from-a-range
inline int generateIntFromTo(const int min, const int max) 
{
	std::random_device randomDevice;
	std::mt19937 rng(randomDevice());
	std::uniform_int_distribution<int> uni(min, max);

	return uni(rng);
}