#include <iostream>
using namespace std;

union temp {
	double celsius;
	double fahrenheit;
}; //again, the semicolon's necessary

union strung {
	char s[5]; //C-style string
	char c;
};

union questionable {
	int i;
	double d;
	char c[8];
};

int main() {
	//union temp t; //C-style declaration. Still legal
	temp t; //We can also use this
	t.celsius=37.0; //Note: use . to access members
	//If we use C++11, we could use an initializer list instead:
	//temp t{37.0};
	cout<<t.fahrenheit<<endl;
	
	strung s={"ABCD"}; //Also a fine way to initialize
	//cout<<s<<endl; //Not going to work
	cout<<s.s<<endl;
	cout<<s.c<<endl;
	cout<<sizeof(s)<<endl;
	
	questionable q={65};
	cout<<q.i<<endl;
	cout<<q.d<<endl;
	cout<<q.c<<endl;
	
	//If we want to define a single union, rather than a type:
	union {
		char c;
		int i;
	} var={'c'};
	cout<<var.i<<endl;
}
