#include <iostream>
using namespace std;
//Let's learn the difference between pointers and references~

void report(string state, int a, int b) {
	cout<<state<<"\ta: "<<a<<"\tb: "<<b<<endl;
}

void veryBadSwap(int a, int b) {
	int temp;
	temp=a;a=b;b=temp;
}

//Note: this is still pass-by-value
void stillBadSwap(int *a, int *b) {
	int *temp;
	temp=a;a=b;b=temp;
}

void pointerSwap(int *a, int *b) {
	int temp;
	temp=*a; //This is called 'dereferencing' the pointer (following to where the pointer actually points)
	*a=*b;
	*b=temp;
}

//This is pass-by-reference
//This 'a' actually uses the same place in memory as the original passed variable
void refSwap(int &a, int &b) {
	int temp;
	temp=a;
	a=b;
	b=temp;
}

//If you want to protect against changes, you can declare a 'const reference' (read-only reference)
void note(int &a, const int &b) {
	a=0;
	//b=0;
}
//Why not just pass-by-value, then? Records tend to be passed via copying. That can be expensive!

int main() {
	int a=5,b=8; //I want to swap these two values. How?
	report("Initial:",a,b);
	veryBadSwap(a,b);
	report("I'm sure this worked:",a,b);
	stillBadSwap(&a,&b); //Note that prepending an ampersand yields the variable's address
	report("Why pass-by-value isn't helpful:",a,b);
	pointerSwap(&a,&b);
	report("Using pointers:",a,b);
	refSwap(a,b);
	report("Swapped back via reference:",a,b);
	note(a,b);
	report("Read-only(ish):",a,b);
}
