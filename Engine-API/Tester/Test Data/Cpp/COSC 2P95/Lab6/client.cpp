#include <iostream>
#include "Set.h"
using namespace std;
int main() {
	Set s1(10);
	cout<<"First set ({x,y,z}): ";
	cin>>s1;
	cout<<"A: "<<s1<<endl;
	Set s2(6);
	cout<<"Second set: ";
	cin>>s2;
	cout<<"B: "<<s2<<endl;
	cout<<"A+B: "<<s1+s2<<endl;
	cout<<"+A: "<<+s1<<endl;
	cout<<"+B: "<<+s2<<endl;
}