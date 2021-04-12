#include <iostream>
#include <string>
using namespace std;

class Simple {
public:
	int abba;
	
	explicit Simple(int a):abba(a){}
	
	void huhNeat() {
		cout<<abba<<endl;
	}
};



int main() {
	//Simple s1=8; //Even this won't compile anymore. An explicit constructor isn't eligible for implicit conversion
	Simple s2('A');//Ah nertz...
	cout<<s2.abba<<endl;
	//Adding a private 'char' constructor would help somewhat, but would still be vulnerable for internal use
}
