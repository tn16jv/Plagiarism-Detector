#include <iostream>

int cheese=12;//This will stick around for the execution



int temp(int aug) {
	int ferris=3;//This will last only while within the function
	ferris+=aug;
	std::cout<<"This time: "<<ferris<<std::endl;
}

int endure(int aug) {
	static int maris=3;//This time, the extent is the same as for the file scope (the execution)
	maris+=aug;
	std::cout<<"Perpetuity: "<<maris<<std::endl;
	//Particularly good for things like counters
}

class Basic {
public:
	std::string id;
	Basic(std::string id){std::cout<<"Constructing "<<id<<"!\n";}
};

void tempC() {
	Basic b("temp");
}

void endureC() {
	static Basic b("endure");
	//Of course, we could use this for things like continuing data structures, encapsulated counters, etc.
}

/*
//I don't feel like the compiler warnings just to show why this is bad.
//We KNOW why, right?
int* ohPleaseNo() {
	int temp;
	return &temp;
}
*/

int main() {
	std::cout<<"First, using primitives:"<<std::endl;
	temp(4);
	endure(4);
	temp(5);
	endure(5);
	temp(2);
	endure(2);
	
	std::cout<<std::endl<<"Now, let's look at objects:"<<std::endl;
	tempC();
	endureC();
	tempC();
	endureC();
	tempC();
	endureC();
	
	std::cout<<std::endl<<"Note that adding static members to a class isn't related to access,"<<std::endl;
	std::cout<<"and, though related to extent, that isn't normally the primary point."<<std::endl;
}