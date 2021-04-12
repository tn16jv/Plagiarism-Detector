#include <iostream>
#include "ornithology.h"

namespace ornithology {
	void Bird::sing() {
		std::cout<<"TWEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEET!"<<std::endl;
	}
	
	void Bird::locomote() {
		std::cout<<"\"Taxi!\""<<std::endl;
	}
}
