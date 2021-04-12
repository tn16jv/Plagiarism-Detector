#include <iostream>


class Leaky {
public:
	int *data;
	int size;
	Leaky(int size):size(size){data=new int[size];}
	~Leaky() {delete[] data;}
};

void hmm() {
	Leaky drip(5);
}


int main() {
	hmm();
	std::cout<<"For this program, we'd never notice, but there's definitely a leak."<<std::endl;
	std::cout<<"By all means, verify with valgrind!"<<std::endl;
	hmm();
}