#include <iostream>
#include "wonky.h"
using namespace std;
/*Technically, you don't HAVE to actually have a return 
*/
int max(int a, int b) {
}

int min(int a, int b) {
}
void complain() {
	cout<<"This is just screwy."<<endl;
	return;
	cout<<"This line is entirely unreachable."<<endl;
}

int main() {
	cout<<max(3,2)<<endl;
	cout<<min(3,2.5)<<endl; //Note the coercion here
	//cout<<min(3.0,2.5)<<endl; //Seriously... be careful with types in C++...
	cout<<random<<endl;
	complain();
}
