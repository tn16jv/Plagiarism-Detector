#include <iostream>
using namespace std;

class Simple {
public:
	int abba,betta;
	Simple(int a, int b):abba(a),betta(b){}
	
	//If you want to prevent copying, simply make the constructor private
	Simple(const Simple &original):abba(original.abba),betta(original.betta){
		cout<<"STOP! CopyTime!"<<endl;
	}
};

void doNothing(Simple s) {
	
}


int main() {
	Simple s1(3,8);
	Simple s2(s1); //C++ comes with a 'copy' operator. But how often is it called? And how safe is it to use?
	cout<<s2.abba<<"\t"<<s2.betta<<endl;
	//doNothing(s1);
	cout<<"Hmm... let's see what \"copy initialization\" is..."<<endl;
	Simple s3=Simple(1,2);
	cout<<"Odd.\nOkay... one more test..."<<endl;
	Simple s4(Simple(4,9)); //Ain't copy elision grand?
	cout<<"SERIOUSLY?!? What the heck is \"eliding\"?!?"<<endl;
	
	//Try compiling with -fno-elide-constructors to see a different outcome for those last two
}
