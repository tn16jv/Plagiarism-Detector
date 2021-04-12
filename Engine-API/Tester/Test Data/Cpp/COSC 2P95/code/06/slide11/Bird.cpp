#include <iostream>
#include "Bird.h"

//Implementation for Bird class
Bird::Bird(double wingspan) {
	this->wingspan=wingspan;
	std::cout<<"Caw!"<<std::endl;
}

void Bird::flap() {
	std::cout<<"flap flap flappity flap!"<<std::endl;
}

void Bird::tweet() {
	std::cout<<"honk."<<std::endl;
}

void Bird::transformIntoMechaBird() {
	std::cout<<"t... tweet? ?"<<std::endl;
}
