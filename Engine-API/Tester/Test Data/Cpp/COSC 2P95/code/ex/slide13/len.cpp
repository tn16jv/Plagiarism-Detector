#include <iostream>



int len(char str[80]) {
	int idx=0;
	while (idx<80 && str[idx]!='\0') {
		++idx;
	}
	return idx;
}


int main() {
	char str[80];
	std::cin>>str;
	std::cout<<str<<len(str)<<std::endl;
}