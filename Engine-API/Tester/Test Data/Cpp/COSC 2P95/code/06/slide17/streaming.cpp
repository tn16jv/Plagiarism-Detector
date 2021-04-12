#include <iostream>
using namespace std;

//Note: the actual class design's still silly here. This is just for the sake of the << overload

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
	
	//If not a member function, use: ostream& operator<<(ostream &out, const Bitfield &field)
	friend std::ostream& operator<<(std::ostream &out, Bitfield &bf) {
		for (int i=sizeof(bf.field)-1;i>=0;i--)
			out<<bf.getFlag(i);
		return out;
	}
	
	//We can overload the IO >> operator in a similar fashion, but that's more of a nuisance to show off
};


int main() {
	Bitfield a,b;
	a.setFlag(3);
	b.setFlag(4);
	cout<<a<<"\t"<<b<<endl;//Note how I can just cascade these
	Bitfield c(a+b);
	cout<<c<<endl;
	
	c+1;
	cout<<c<<endl;
	c+2+0+5+6+7;
	cout<<c<<endl;
}
