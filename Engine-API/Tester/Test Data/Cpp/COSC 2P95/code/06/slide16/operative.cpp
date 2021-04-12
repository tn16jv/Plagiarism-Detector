#include <iostream>
using namespace std;

class Dealie {
private:
	int value;
public:
	Dealie(int value):value(value){}
	Dealie add(const Dealie &other) {
		return Dealie(value+other.value);
	}
	Dealie operator+(const Dealie &other) {
		return add(other);
	}
	friend Dealie operator-(const Dealie &a, const Dealie &b);
	
	int getValue() const {return value;}
};

Dealie operator-(const Dealie &a, const Dealie &b) {
	return Dealie(a.value-b.value);
}


int main() {
	Dealie a(3);
	Dealie b(4);
	cout<<a.add(b).getValue()<<endl;
	cout<<(a+b).getValue()<<endl;
	cout<<(a-b).getValue()<<endl;
}
