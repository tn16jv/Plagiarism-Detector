#include <iostream>
#include "top.h" //For wisdom
#include "left.h" //For the left
#include "right.h" //For the right


int main() {
	std::cout<<"Because politics are definitely a safe topic to talk about..."
		<<std::endl<<"Let's get some wisdoms from both sides of the aisle!"<<std::endl;
	
	std::cout<<"Left: "<<askTheLeft().msg<<std::endl;
	std::cout<<"Right: "<<askTheRight().msg<<std::endl;
}