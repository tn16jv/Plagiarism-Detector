#include <iostream>

using namespace std;

void exampleOne() {
	enum Colour {
		PURPLE, //0
		VIOLET, //1
		PERIWINKLE, //2
		LAVENDAR,
		PLUM,
		LILAC,
		AMETHYST,
		MAUVE
	}; //Yes, the semicolon is actually necessary
	
	Colour c=PURPLE;
	cout<<c<<endl; //Note: pay attention to what this puts out!
	
	//Colour d=VIOLET+1; //Not legal, because VIOLET+1 is an int
	Colour d=(Colour)(VIOLET+1); //Legal, but unpleasant
	//d++; //Both illegal and a terrible idea (what if the colours weren't contiguously-numbered?)
	cout<<d<<endl; //Note that it's still effectively just an int
	Colour e=static_cast<Colour>(3); //we'll get to static_cast very soon
	e=static_cast<Colour>(PLUM-1);
	
	Colour f(PERIWINKLE); //This is actually the syntax we'd typically use in C++
}

//Note: this is an incredibly bad place to define this
//At least, put it at the top.
//Realistically, it may be better in a header file
enum dayOfTheWeek {
	MONDAY=1,
	TUESDAY, //will be 2
	WEDNESDAY, //will be 3
	THURSDAY,
	FRIDAY,
	SATURDAY,
	SUNDAY=0
};
//Negative values are also just fine

void exampleTwo() {
	//Colour c=PURPLE; //<-wouldn't be legal. The Colour type doesn't exist within this scope!
	//int c=PURPLE; //<-also not legal. thankfully
	cout<<"Monday: "<<MONDAY<<"\tTuesday: "<<TUESDAY<<"\t Saturday: "<<SATURDAY<<"\tSunday: "<<SUNDAY<<endl;
	dayOfTheWeek d=static_cast<dayOfTheWeek>(SATURDAY+2);
	cout<<d<<endl; //oh nertz
	
	//Clearly, this 'day of the week' thing couldn't backfire at all, right? Can't think of any other, similar, enumerations
	//	that might interfere with it. Nope. Not at all.
	enum weekday {
		MONDAY,
		TUESDAY,
		WEDNESDAY,
		THURSDAY,
		FRIDAY
	};
	
	cout<<"Monday: "<<MONDAY<<endl; //Oh geez...
	cout<<"Sunday: "<<SUNDAY<<endl; //Oh... consarnit!
	cout<<"Monday and Sunday are totally the same thing: "<<(MONDAY==SUNDAY)<<endl; //at least it warns...
	
	//Note: This was both better and worse in C.
}

int main() {
	exampleOne();
	exampleTwo();
	
}
