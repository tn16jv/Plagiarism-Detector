//#pragma once isn't actually supported by the C++ standard, but it's *widely* supported by compilers
#pragma once
#include "feathers.h"
//Ornithology library
//If there's any loose ornithological behaviours, put them here
//Also declares Bird class

namespace ornithology {
	class Bird : public feathers::Feathered {
	public:
		void sing();
		void locomote();
	};
}
