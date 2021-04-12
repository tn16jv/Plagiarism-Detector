#include <iostream>

/*This is just a simplified form of the diamond problem.
 *In this case, there technically isn't any harm; the point is simply to demonstrate what runs, and what's allocated.
 *
 *Then, we can see the solution for simplifying.
 */

class NaiveSharedBase {
public:
	NaiveSharedBase() {
		std::cout<<"\t\tConstructing NaiveSharedBase"<<std::endl;
	}
};

class NaiveIntermediateA : public NaiveSharedBase {
public:
	NaiveIntermediateA() {
		std::cout<<"\tConstructing NaiveIA"<<std::endl;
	}
};

class NaiveIntermediateB : public NaiveSharedBase {
public:
	NaiveIntermediateB() {
		std::cout<<"\tConstructing NaiveIB"<<std::endl;
	}
};

class NaiveFinalDerived : public NaiveIntermediateA, public NaiveIntermediateB {
public:
	NaiveFinalDerived() {
		std::cout<<"Constructing NaiveDerived"<<std::endl;
	}
};


class VirtualSharedBase { //Notice that there's nothing special to do here. The trick is in how you inherit.
public:
	VirtualSharedBase() {
		std::cout<<"\t\tConstructing VirtualSharedBase"<<std::endl;
	}
};

class VirtualIntermediateA : virtual public VirtualSharedBase {
public:
	VirtualIntermediateA() {
		std::cout<<"\tConstructing VirtualIA"<<std::endl;
	}
};

class VirtualIntermediateB : virtual public VirtualSharedBase {
public:
	VirtualIntermediateB() {
		std::cout<<"\tConstructing VirtualIB"<<std::endl;
	}
};

class VirtualFinalDerived : public VirtualIntermediateA, public VirtualIntermediateB {
public:
	VirtualFinalDerived() {
		std::cout<<"Constructing VirtualDerived"<<std::endl;
	}
};

int main() {
	std::cout<<"=====First, let's create a single NaiveFinalDerived..."<<std::endl;
	NaiveFinalDerived nfd;
	
	std::cout<<"\n=====Next, let's try using a virtual base class..."<<std::endl;
	VirtualFinalDerived vfd;
}
