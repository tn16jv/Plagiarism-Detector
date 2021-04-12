#include <iostream>
#include <string>
//Hey! Let's use a comment!
/*Assuming you compile this to 'c', try running this like:
 * ./c
 *then:
 * ./c 1> temp
 *then:
 * ./c 2> temp
 *and maybe even:
 * ./c >temp
 *
 * I wonder what'd happen if you tried >> instead of >, or even tried using a <?
 */
using std::cout; using std::endl; using std::cin; using std::cerr; using std::clog;
int main() {
	int k;
	cout<<"Please enter a value!"<<std::endl;
	std::cin>>k;
	if (k>0)
		cout<<"You picked: "<<k<<"!\n";
	else if (k<0) {
		cerr<<"Why do you always have to be so negative?"<<endl;
	}
	else
		clog<<"How very... neutral of you.\n";
}
