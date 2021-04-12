#include <iostream>
#include "funky.h"
using namespace std;

int max(int a, int b) {
	return b>a?b:a;
}

int min(int a, int b) {
	return b<a?b:a;
}

int main() {
	cout<<max(3,2)<<endl;
	cout<<min(3,2.5)<<endl; //Note the coercion here
	cout<<min(3.0,2.5)<<endl; //Seriously... be careful with types in C++...
	cout<<random<<endl;
}
