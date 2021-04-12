#include <iostream>
#include "PRecord.h"
using namespace std;
struct node {
	PRecord item;
	node *next;
};
void enq(PRecord &i, node* &n);
PRecord deq(node* &n);
