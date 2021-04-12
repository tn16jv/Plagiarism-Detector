#include <iostream>
#include <string>

using namespace std;

int main() {
	bool a,b,c;
	a=true;
	b=false;
	cout<<"a: "<<a<<"\tb: "<<b<<"\t!a: "<<!a<<"\t!b: "<<!b<<endl;
	cout<<"~a: "<<~a<<"\t~b: "<<~b<<endl;
	cout<<"~a==~b: "<<(~a==~b)<<endl;
	cout<<"!~a==!~b: "<<(!~a==!~b)<<endl;
}
