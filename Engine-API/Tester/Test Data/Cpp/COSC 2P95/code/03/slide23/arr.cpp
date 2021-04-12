#include <iostream>
#include <iomanip>
using namespace std;

void inspectArrGadget(int arr[]) {
	cout<<"Size of received array:"<<sizeof(arr)<<endl;
	cout<<"Third element received: "<<arr[2]<<endl;
}

void inspectPointer(int *arr) {
	cout<<"Size of received array:"<<sizeof(arr)<<endl;
	cout<<"Third element received: "<<arr[2]<<endl;
}

void mutate(int arr[]) {
	arr[2]=99;
}

int main() {
	//int arr1[]; //Don't do this in C++
	//int *arr2; //This is fine, though we aren't at how to use it yet
	int arr3[3];
	cout<<"Size of the array: "<<sizeof(arr3)<<endl;
	
	//Remember how we keep telling you to initialize variables before use?
	//This is why!
	cout<<"Contents of array[1]: "<<arr3[1];
	
	int arr4[]={2,4,6,10};
	cout<<"Size of the array: "<<sizeof(arr4)<<endl;
	cout<<"Print the array: "<<arr4<<endl;
	cout<<"Dereference the array: "<<*arr4<<endl;
	
	/*This one isn't legal, because, while although you can defer determining
	 *the number of rows, that's as far as you can take it. It needs the rest of
	 *the geometry
	 */
	//int arr5[][]={{1,2,3,4},{5,6,7,8}}; //Not legal
	
	int arr6[][3]={{1,2,3},{4,5,6},{7,8,9}}; //Entirely legal
	cout<<"Size of the array: "<<sizeof(arr6)<<endl;
	
	inspectArrGadget(arr4);
	inspectPointer(arr4);
	
	int *pleaseAvoidThis=arr4+1;
	cout<<"First element of... something: "<<pleaseAvoidThis[0]<<endl;
	
	cout<<"Third element of arr4 before call: "<<arr4[2]<<endl;
	mutate(arr4);
	cout<<"Third element of arr4 after call: "<<arr4[2]<<endl;
	
	unsigned int value=0x12345678;
	
	//We haven't gotten to the 'hex' part yet
	cout<<"Value: "<<hex<<value<<endl;
	
	//Be EXTREMELY careful before trying something like this:
	unsigned char *ptr=(unsigned char*)&value;
	cout<<"Value, bit by bit:"; //(not literally, of course)
	for (int i=0;i<4;i++)
		cout<<" "<<(int)ptr[i];
	cout<<endl;
	cout<<dec;//Again, we haven't covered this yet
}
