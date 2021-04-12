#include <iostream>
#include <fstream>
#include <string>
using namespace std;

//Let's read in every word we can, but omit the final character from everything!
int main() {
	string term;
	while (cin) {//This'll stop the moment we hit an EOF
		cin>>term;
		term[term.length()-1]='\0';//This is sooo wrong...
		cout<<term<<endl;
	}
}
