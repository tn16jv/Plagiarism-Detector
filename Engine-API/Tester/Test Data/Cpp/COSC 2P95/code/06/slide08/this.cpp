#include <iostream>
using namespace std;

//Normally, you'd demonstrate something like this via mathematical examples (so you can chain operations), but this is just a simple version
class Pac {
public:
	Pac& wokka() {
		cout<<"wokka";
		return *this; //Note the dereference!
	}
};


int main() {
	Pac man;
	man.wokka().wokka().wokka().wokka().wokka();
	cout<<endl;
}
