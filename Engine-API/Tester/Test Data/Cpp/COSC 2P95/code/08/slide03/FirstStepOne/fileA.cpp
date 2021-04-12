#include <iostream>

//Do we actually need to use fileB's header to make use of its features?
int bebe();

int main() {
	std::cout<<"Can we use fileB's bebe? "<<bebe()<<std::endl;
}
