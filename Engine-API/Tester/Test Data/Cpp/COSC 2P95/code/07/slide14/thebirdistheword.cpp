#include <iostream>
#include <string>

class Bird {
private:
	float wingspan;
public:
	Bird(float wingspan=15.0f):wingspan(wingspan){};
	
	void fly() {
		std::cout<<"I can fly! I can flyyy!\nSoaring so majestically!"<<std::endl;
	}
	
	std::string tweet() {
		return"Tweet tweet!";
	}
	
	float getWingspan() {return wingspan;}
};

class Duck : public Bird {
public:
	std::string tweet() {
		return "QUACK!";
	}
};

class Penguin : public Bird {
private:
	using Bird::fly;
};


int main() {
	Bird b;
	Duck d;
	Penguin p;
	std::cout<<"The bird says, \""<<b.tweet()<<"\"."<<std::endl;
	std::cout<<"The duck says, \""<<d.tweet()<<"\"."<<std::endl;
	std::cout<<"The penguin says, \""<<p.tweet()<<"\". I guess."<<std::endl;
	
	std::cout<<"\nLet's try flying."<<std::endl;
	b.fly();
	d.fly();
	//p.fly();//won't compile, as Penguin's 'fly' is inaccessible
	
	Bird &br=d;//A Bird reference... hmm
	Bird *bp=&p;//A Bird pointer (or possibly British Petroleum)
	
	std::cout<<"\nHmm, let's let the BirdDuck tweetquack? "<<br.tweet()<<std::endl;
	std::cout<<"Can we get the BirdPenguin to fly?"<<std::endl;
	bp->fly();
	
	
}
