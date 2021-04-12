#include <iostream>
#include <string>
#include <sstream>
#include "Set.h"

//Implementation for Set class
int Set::cardinality=0;

Set::Set(){
	Set(255);
}
Set::Set(short capacity){
	this->capacity=capacity;
}

Set::Set(bool const* elements, int capacity, int cardinality){
	this->capacity=capacity;
	this->cardinality=cardinality;
	for (int i=0; i<capacity; i++){
		this->elements[i]=elements[i];
	}
}

Set Set::operator+(const Set &other){
	bool arr[255];
	int cap=(other.capacity<this->capacity)?this->capacity:other.capacity;
	int card=0;
	for (int i=0;i<cap;i++){arr[i]=false;}
	for (int i=0; i<other.capacity; i++){
		if(other.elements[i]){
			arr[i]=true;
			card++;
		}
	}
	for (int i=0; i<this->capacity; i++){
		if(this->elements[i]&&!arr[i]){
			arr[i]=true;
			card++;
		}
	}
	return Set(arr,cap,card);
}

Set Set::operator+(const int &other){
	bool arr[255];
	int cap;
	int card=this->cardinality;
	if (other>=this->capacity){cap=other+1;}
	else {cap=this->capacity;}
	for (int i=0;i<cap;i++){
		if (this->elements[i]){
			arr[i]=true;
		}
	}
	if (!arr[other]){
		arr[other]=true;
		card++;
	}
	return Set(arr,cap,card);
}

int Set::operator()(){
		return this->cardinality;
}

Set Set::operator+(){
	bool arr[255];
	int cap=this->capacity;
	int card=cap;
	for (int i=0;i<cap;i++){
		arr[i]=true;
	}
	return Set(arr,cap,card);
}

std::ostream& operator<<(std::ostream &out, const Set &set) {
	int c=0;
	std::cout<<'{';
	for(int i=0;i<set.capacity;i++){
			if (set.elements[i]){
				std::cout<<i;
				c++;
				if (c!=set.cardinality){std::cout<<',';}
			}
	}
	std::cout<<'}'<<std::endl;
}

std::istream& operator>>(std::istream &in, Set &set) {
	bool arr[255];
	int cap=set.capacity;
	int card=0;
	char open;
	in>>open;
	if (in.fail() || open!='{') {
			in.setstate(std::ios::failbit);
			return in;
	}
	for (int i=0;i<cap;i++)
				arr[i]=false;
	std::string buff;
	std::getline(in,buff,'}');
	std::stringstream ss(buff);
	std::string field;
	while (true) {
			std::getline(ss,field,',');
	if (ss.fail()) break;
			int el;
			std::stringstream se(field);
			se>>el;
			if (el>=0&&el<cap){
					arr[el]=true;
					card++;
			}
	}
	set=Set(arr,cap,card);
}

int Set::getCapacity(){
	return this->capacity;
}
