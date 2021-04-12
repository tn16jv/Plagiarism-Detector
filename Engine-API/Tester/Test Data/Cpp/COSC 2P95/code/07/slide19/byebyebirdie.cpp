#include <iostream>
#include <string>

/*An abstract class is a *partial implementation*. It contains everything that you know you'll need across all
 *derived types, but leaves placeholders for functionalities that will vary from subtype to subtype.
 *
 *In this case, we know that all birds will have a wingspan, that all birds will sing *somehow*, and that all birds
 *have some method of locomotion. As such, we can define a Bird as an abstract base class.
 *
 *If you're interested in a more practical example, IO isn't unheard of, but also consider chess pieces:
 *	*Each piece has a side (white or black)
 *	*Each piece either has or hasn't moved yet
 *	*Each piece has the ability to move, but the behaviour for moving varies
 *	*Each piece needs to be representable on the board, but the specifics for rendering also vary
 *
 *If one of these had absolutely no implementation or member variables whatsoever, then it would be an
 *interface class. Such classes are primarily used for specifications (e.g. defining the abstract concept of a list),
 *but can occasionally also be used as a fully abstract supertype (particularly if all concrete implementations are
 *expected to vary wildly, negating the benefit in sharing any substantial common code)
 */

//abstract base class
class Bird {
private:
	float wingspan;
public:
	Bird(float wingspan=15.0f):wingspan(wingspan) {};
	
	virtual void locomote()=0;
	
	virtual std::string sing()=0;
	
	float getWingspan() {return wingspan;}
};

class Duck : public Bird {
public:
	Duck(float wingspan=20.0f):Bird(wingspan) {};
	
	virtual void locomote() {
		std::cout<<"flapple flapple flapple. duckle duckle duckle. swimmle swimmle swimmle."<<std::endl;
	}
	
	virtual std::string sing() {
		return "QUACK!";
	}
};

class Penguin : public Bird {
public:
	Penguin(float wingspan=130.0f):Bird(wingspan){};
	
	virtual void locomote() {
		std::cout<<"waddle waddle waddle"<<std::endl;
	}
	
	virtual std::string sing() {
		return "...";
	}
};


//I'm pretty sure a magic carpet is a bird, since it can fly...
//But I don't know how it sings, so it's *still* an abstract class!
class MagicCarpet : public Bird {
public:
	virtual void locomote() {
		std::cout<<"strictly speaking, aren't *I* the one showing her the world? I don't see you flying."<<std::endl;
	}
	
	//note the lack of a sing()
};

int main() {
	//Bird b; //We can't use this anymore, since an abstract base class can't be instantiated
	Duck d;
	Penguin p;
	std::cout<<"The duck says, \""<<d.sing()<<"\"."<<std::endl;
	std::cout<<"The penguin says, \""<<p.sing()<<"\". I guess."<<std::endl;
	
	std::cout<<"\nLet's try flying."<<std::endl;
	d.locomote();
	p.locomote();
	
	Bird &br=d;//A Bird reference... hmm
	Bird *bp=&p;//A Bird pointer (or possibly British Petroleum)
	
	std::cout<<"\nHmm, let's let the BirdDuck singquack? "<<br.sing()<<std::endl;
	std::cout<<"Can we get the BirdPenguin to move?"<<std::endl;
	bp->locomote();
	
}
