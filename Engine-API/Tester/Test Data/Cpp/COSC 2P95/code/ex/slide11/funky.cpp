#include <iostream>

double doubler(int val) {return 2.0*val;}

double halver(int val) {return 0.5*val;}

double lazy(int val) {return (double)val;}


int main() {
	double (*ptr)(int)=&doubler;
	std::cout<<ptr(3)<<std::endl;
	ptr=&lazy;
	std::cout<<ptr(3)<<std::endl;
	ptr=&halver;
	std::cout<<ptr(3)<<std::endl;
	
	double (*arr[3])(int)={&halver,&lazy,&doubler};
	int choice;
	while (true) {
		std::cin>>choice;
	if (choice<0) break;
		std::cout<<arr[choice](5)<<std::endl;
	}
}