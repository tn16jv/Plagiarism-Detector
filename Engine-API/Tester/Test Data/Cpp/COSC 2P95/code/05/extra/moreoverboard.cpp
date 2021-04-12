#include <iostream>
using namespace std;

template <typename ItemType>
struct node {
	ItemType item;
	node<ItemType> *next;
};

template <typename ItemType>
void push(const ItemType &i, node<ItemType>* &n) {
	node<ItemType> *nn=new node<ItemType>;
	nn->item=i;nn->next=n;
	n=nn;
}

template <typename ItemType>
ItemType pop(node<ItemType>* &n) {
	//Of course, there really should be some safety checking here, but we haven't covered exceptions yet
	ItemType fr;
	node<ItemType> *ptr=n;
	n=n->next;
	fr=ptr->item;
	delete ptr;
	return fr;
}


int main() {
	node<int> *head;
	push(13,head);
	push(10,head);
	push(18,head);
	cout<<head->item<<"\t"<<head->next->item<<"\t"<<head->next->next->item<<endl;
	cout<<pop(head)<<"\t"<<pop(head)<<"\t"<<pop(head)<<endl;
}
