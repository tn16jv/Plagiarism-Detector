#include <iostream>
#include <string>
using namespace std;

class Simple {
public:
	int abba;
	Simple(int a):abba(a){}
	void huhNeat() {
		cout<<abba<<endl;
	}
};



int main() {
	Simple s1=8;
	cout<<s1.abba<<endl;
	s1.huhNeat();
	Simple s2='a';
	cout<<s2.abba<<endl;
	cout<<"Less neat. Why?"<<endl;
}
