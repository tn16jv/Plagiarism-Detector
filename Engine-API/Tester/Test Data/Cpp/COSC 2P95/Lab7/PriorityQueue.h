// This is a struct to represent a node in a linked PriorityQueue
// node stores items of type T
template <typename T>
struct node {
	T item;     //item stored in node
	long time;  //priority of node in queue
	node *next; //next node in PriorityQueue
};
// This class is a generic type PriorityQueue dynamic structure
template <typename T>
class PriorityQueue {
private:
	node<T>* tail;   //least prioritized element in queue
	int 	 length; //length of queue
public:
	// This is a default constructor that initializes field values to null
	PriorityQueue() : tail(0),length(0){};
	
	// This destructor deletes all remaining nodes in the queue from head to tail
	~PriorityQueue() {
		while(length != 0){
			node<T> *p=tail;   //node to traverse to head of queue
			node<T> *q=0;      //node to follow node p
			while(p->next!=0){ 
				q=p;
				p=p->next;
			}
			delete p;
			length--;
			if (q!=0){q->next=0;}
		}
	}
	
	/** This method returns true if the queue is empty & false if the queue 
	  * is not.
	  *
	  * @return bool true when queue is empty, false when not empty
	  */
	bool isEmpty() {
		return length==0;
	}
	//isEmpty
	
	/** This method returns the highest priority item within the queue
	  * 
	  * @return T  the item at the front of the queue 
	  */
	T peek(){
		T it;
		node<T> *p=tail;
		while(p->next != 0){
			p=p->next;
		}
		it=p->item;
		return it;
	}
	//peek
	
	/** This method inserts an item into the queue based on its priotity
	  * 
	  * @param i  	 the item being added
	  * @param prio  the priority of the item being added
	  */
	void enq(const T i, long prio) {
		if(isEmpty()){tail=0;}
		length++;
		node<T> *nn=new node<T>;  //new node to be added 
		nn->item=i;nn->time=prio;nn->next=tail;
		if (nn->next==0 || prio >= nn->next->time){tail=nn;}
		else{
			node<T> *p;   //node to traverse queue for insertion
			node<T> *q=0; //node that follows p during insertion
			while(nn->next !=0 && prio < nn->next->time){
				p=nn->next;
				nn->next=p->next;
				p->next=nn;
				if (q!=0){q->next=p;}
				q=p;
			}
		}
	}
	//enq
	
	/** This method removes the highest priority item (head) within the queue
	  * 
	  * @return T  the item being removed 
	  */
	T deq() {
		length--;
		T it;            //item at the head of the queue
		node<T> *p=tail; //to traverse to front of queue 
		node<T> *q=0;    //to traverse to the second item in queue
		while(p->next != 0){
			q = p;
			p=p->next;
		}
		it=p->item;      
		if (q!=0){q->next=0;}
		delete p;
		return it;
	}
	//deq
};
//PriorityQueue.h