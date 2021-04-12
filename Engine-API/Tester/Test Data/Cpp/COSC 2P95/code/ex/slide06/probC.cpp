#include <iostream>


class Reliable {
public:
	
	int *data;
	int size;
	Reliable(int size):size(size){data=new int[size];}
	Reliable(const Reliable &r):size(r.size){data=new int[size];for (int i=0;i<size;i++) data[i]=r.data[i];}
	
	~Reliable(){delete[] data;}
};

void hmm(Reliable r) {
}

void whatcouldpossiblighgowrong() {
	Reliable r2(5);
	Reliable r3(5);
	r2=r3;
}

int main() {
	Reliable r(5);
	hmm(r);
	std::cout<<"Does... does it work now?"<<std::endl;
	hmm(r);
	std::cout<<"Holy screeching daffodils! Finally!"<<std::endl;
	std::cout<<"Now, to do some work..."<<std::endl;
	//whatcouldpossiblighgowrong();
	//When fixing this, beware of some of the nuances that can arise. It's pretty easy to not notice a problem.
}