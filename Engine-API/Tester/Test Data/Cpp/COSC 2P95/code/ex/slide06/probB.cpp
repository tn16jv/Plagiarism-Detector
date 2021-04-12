#include <iostream>


class Sealed {
public:
	int *data;
	int size;
	Sealed(int size):size(size){data=new int[size];}
	Sealed(const Sealed &s) {
		this->size=s.size;
		this->data=new int[this->size];
		for (int i=0;i<this->size;i++) this->data[i]=s.data[i];
	}
	~Sealed(){delete[] data;}
};

void hmm(Sealed s) {
	
}

//Note that there's a hyuuuge problem here
//But, how do we fix it?
//(There are a couple of completely different ways!)
int main() {
	Sealed s(5);
	//hmm(s);
	std::cout<<"This time, no leaks! Feel free to check!"<<std::endl;
	std::cout<<"And nothing could possibly go wrong by uncommenting..."<<std::endl;
	//hmm(s);
}