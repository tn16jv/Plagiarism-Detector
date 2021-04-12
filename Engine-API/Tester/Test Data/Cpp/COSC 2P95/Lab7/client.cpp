#include <iostream>
#include <string>
#include "PriorityQueue.h"
using namespace std;

/** This method prompts the user to select an option and returns their selection
  * 
  * @return res  the selected option
  */
int prompt(){
	double op;  //user option input
	int res;    //final option result
	bool valid; //validates input type
	do{
		valid=true;
		cerr<<"\nSelect an option:"<<endl;
		cerr<<"1. Add Entry"<<endl;
		cerr<<"2. Peek at Next Element"<<endl;
		cerr<<"3. Remove Next Element"<<endl;
		cerr<<"0. Quit"<<endl;
		cin>>op;
		res = op;
		if (res<0 || res>3||cin.fail()){
			cerr<<"Please enter a value between 0 & 3.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}
//prompt

/** This method prompts the user to input the priority of the item being added from (10-0)
  *
  * @return res  the priority of the item
  */
int prioPrompt(){
	double op;  //user option input
	int res;    //final option result
	bool valid; //validates input type
	do{
		valid=true;
		cerr<<"What is the priority? high(0 - 10)low : ";
		cin>>op;
		res = op;
		if (res<0 || res>10||cin.fail()){
			cerr<<"Please enter a value between 0 & 10.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}
//prioPrompt

/** This method prompts the user to input a string for the item being added to the queue
  * 
  * @return cont  the content inputed for item
  */
string textPrompt(){
	string cont;  //user option input
	bool valid; //validates input type
	do{
		valid=true;
		cerr<<"Enter a word for entry: ";
		cin>>cont;
		if (cin.fail()){
			cerr<<"Invalid entry\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return cont;
}
//textPrompt

/** This program prompts the user to add, remove or peek at an element within the
  * PriorityQueue until the user chooses to quit
  */
int main(){
	PriorityQueue<string> pq; //PriorityQueue dynamic structure to store string elements
	int op; //Queue functions,0=quit, 1=add element & priority, 2=peek at head of queue, 3=remove element from head
	do{
		op = prompt();
		if(op==1){
			int prio;
			prio = prioPrompt();
			string cont;
			cont = textPrompt();
			pq.enq(cont, prio);
		}
		else if(op==2){
			if(!pq.isEmpty()){
				cout<<"Word: "<<pq.peek()<<endl;
			}
			else{
				cerr<<"The queue is empty."<<endl;
			}
		}
		else if(op==3){
			if (!pq.isEmpty()){
				cout<<"Removed "<<pq.deq()<<" from the queue."<<endl;
			}
			else{
				cerr<<"The queue is empty."<<endl;
			}
		}
	}while(op != 0);
}
//main