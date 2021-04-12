#include <iostream>
#include <string>
using namespace std;

class Simple {
public:
	int abba;
	
	Simple(char)=delete; //(C++11 and above)
	explicit Simple(int a):abba(a){}
	
	void huhNeat() {
		cout<<abba<<endl;
	}
};



int main() {
	//Simple s('A');//Won't compile anymore! (which is a good thing, in this case)
	Simple s1=8;
	cout<<s1.abba<<endl;
}
