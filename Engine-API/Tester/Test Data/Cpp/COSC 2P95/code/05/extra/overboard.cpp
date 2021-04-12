#include <iostream>
using namespace std;

template <typename ItemType>
struct node {
	ItemType item;
	node<ItemType> *next;
};

template <typename ItemType>
node<ItemType> *newNode(ItemType item, node<ItemType> *next) {
	node<ItemType> *forReturn = new node<ItemType>;
	forReturn->item=item;forReturn->next=next;
	return forReturn;
}



int main() {
	node<int> *head=newNode<int>(13,static_cast<node<int>*>(0));
	head=newNode<int>(10,head);
	head=newNode<int>(18,head);
	cout<<head->item<<"\t"<<head->next->item<<"\t"<<head->next->next->item<<endl;
	
}
