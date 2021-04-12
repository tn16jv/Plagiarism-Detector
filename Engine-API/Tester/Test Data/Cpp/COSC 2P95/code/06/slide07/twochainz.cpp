#include <iostream>
using namespace std;

//Compile using the C++11 (or higher) standard

class Stuff {
private:
	int abba;
	int bebe;
public:
	Stuff(): Stuff(1) {cout<<"I'm a 'default' kind of guy..."<<endl;}
	Stuff(int a): Stuff(a,1) {cout<<"There's only one constructor for me!"<<endl;}
	Stuff(int a, int b): abba(a), bebe(b){cout<<"Received "<<a<<" and "<<b<<"."<<endl;}
};

int main() {
	Stuff s1(7,8);
	cout<<endl;
	Stuff s2(6);
	cout<<endl;
	Stuff s3;
	cout<<endl;
}
