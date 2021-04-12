#include <iostream>
using namespace std;

//Note: this example's just plain silly.

class Bitfield {
private:
	unsigned long field;
public:
	Bitfield():Bitfield(0){} //Remember: C++11 for this one
	Bitfield(unsigned long initial):field(initial){}
	
	bool getFlag(int pos) {
		return !!((field>>pos)&0x01);
	}
	
	void setFlag(int pos) {
		field|=1UL<<pos;
	}
	
	void clearFlag(int pos) {
		field&=~(1UL<<pos);
	}
	
	Bitfield operator+(const Bitfield &other) {
		return Bitfield(field|other.field);
	}
	
	Bitfield& operator+(int p) { //Sooo bad
		setFlag(p);
		return *this;
	}
};


int main() {
	Bitfield a,b;
	a.setFlag(3);
	b.setFlag(4);
	Bitfield c(a+b);
	for (int i=7;i>=0;i--)
		cout<<(c.getFlag(i)?'Y':'N');
	cout<<endl;
	
	c+1; //<-This is a big part of why it's sooo bad (plus + signs can often be confusing for mutable types)
	//1+c; //Not even close to legal
	for (int i=7;i>=0;i--)
		cout<<(c.getFlag(i)?'Y':'N');
	cout<<endl;
	
	c+2+0+5+6+7; //Do we understand what this means?
	for (int i=7;i>=0;i--)
		cout<<(c.getFlag(i)?'Y':'N');
	cout<<endl;
}
