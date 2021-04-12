#include "node.h"
#include <iostream>
using namespace std;

void enq(PRecord &i, node* &n) {
	node *nn=new node;
	node *q;
	bool first=true;
	nn->item=i;nn->next=n;
	n=nn;
	while(nn->next != 0 && i.time < nn->next->item.time){
		if(first){
			n=nn->next;
			nn->next=n->next;
			n->next=nn;
			first=false;
			q=n;
		}
		else{
			node *p=nn->next;
			nn->next=p->next;
			p->next=nn;
			q->next=p;
			q=p;
		}
	}
}

PRecord deq(node* &n) {
	node *p=n;
	node *q=0;
	PRecord fr;
	while(p->next != 0){
		q = p;
		p=p->next;
	}
	fr=p->item;
	if (q!=0){q->next=0;}
	delete p;
	return fr;
}
int prompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"Select an option:"<<endl;
		cerr<<"1. Add Entry"<<endl;
		cerr<<"2. Remove Next Element"<<endl;
		cerr<<"0. Quit"<<endl;
		cin>>op;
		res = op;
		if (res<0 || res>2||cin.fail()){
			cerr<<"Please enter a value between 0 & 2.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}

int prioPrompt(){
	double op;
	int res;
	bool valid;
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

string textPrompt(){
	string op;
	bool valid;
	do{
		valid=true;
		cerr<<"Enter a word for entry: ";
		cin>>op;
		if (cin.fail()){
			cerr<<"Invalid entry\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return op;
}

int main(){
	int op;
	bool empty=false;
	node *head = 0;
	do{
		op = prompt();
		if(op==1){
			int prio;
			prio = prioPrompt();
			string cont;
			cont = textPrompt();
			if (empty){head=0;empty=false;}
			PRecord pr = {prio, cont};
			enq(pr, head);
		}
		else if(op==2){
			if (!empty){
				if(head->next==0){empty=true;}
				PRecord pr = deq(head);
				cout<<"Word: "<<pr.entry<<" Time: "<<pr.time<<"\n"<<endl;
			}
			else{
				cerr<<"The queue is empty\n"<<endl;
			}
		}
	}while(op != 0);
}
