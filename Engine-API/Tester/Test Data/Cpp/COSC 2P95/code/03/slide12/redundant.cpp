#include <iostream>
#include "redundant.h"
using namespace std;
//This is just an example of overloading a function

int max(int a, int b) {
	cout<<"integerizing!\t";
	return b>a?b:a;
}

double max(double a, double b) {
	cout<<"they ALL float down here!\t";
	return b>a?b:a;
}

int main() {
	cout<<max(3,2)<<endl;
	//cout<<max(3,2.5)<<endl; //Good luck with this one!
	cout<<max(3.1,2.5)<<endl;
}
