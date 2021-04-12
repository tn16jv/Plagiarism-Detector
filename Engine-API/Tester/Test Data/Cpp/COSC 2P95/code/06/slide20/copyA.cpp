#include <iostream>
using namespace std;

class Simple {

public:
	int abba,betta;
	Simple(int a, int b):abba(a),betta(b){}
};


int main() {
	Simple s1(3,8);
	Simple s2(s1); //C++ comes with a 'copy' operator. But how often is it called? And how safe is it to use?
	cout<<s2.abba<<"\t"<<s2.betta<<endl;
}
