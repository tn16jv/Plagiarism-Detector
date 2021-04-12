#include <iostream>
#include <string>

/*This just has a single 'virtual function' example.
 *For tweeting, the virtual modifier lets it know to look for derived versions (thus restoring our 'quack').
 *However, this doesn't help for the flying, because the virtual keyword doesn't tell it to 'give up after trying to find
 *derived versions'. It simply defaults to the version it can find.
 */

class Bird {
private:
	float wingspan;
public:
	Bird(float wingspan=15.0f):wingspan(wingspan){};
	
	virtual void fly() { //This one doesn't help us
		std::cout<<"I can fly! I can flyyy!\nSoaring so majestically!"<<std::endl;
	}
	
	virtual std::string tweet() {
		return"Tweet tweet!";
	}
	
	float getWingspan() {return wingspan;}
};

class Duck : public Bird {
public:
	virtual std::string tweet() { //This 'virtual' does NOTHING (but is a popular hint, and good for future derived types)
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
