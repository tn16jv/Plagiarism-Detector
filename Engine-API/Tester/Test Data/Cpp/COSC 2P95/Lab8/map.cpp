#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
using namespace std;

string course[20]; //Course Codes
bool am[20][20];   //Adjacency Matrix
bool visited[20];  //Was Visited array for Courses
int indeg[20];     //Indegree array for Courses
int nCourse;	   //Number of Courses
int nEdges;		   //Number of Edges	
 
int main(){
		//Get file name & try to open file
		char fileName[50];  //name of file
		cout<<"Graph filename: ";
		cin>>fileName;
		cout<<"Using "<<fileName<<endl;
		const char* fn = fileName;
		ifstream data(fn);  //file
		if (data.is_open()){
			cout<<"File loaded."<<endl;
			//Fill courses array with course codes
			data >> nCourse;
			string id;	//course code
			for(int i=0; i<nCourse; i++){
				data >> id;
				course[i] = id;
			}
			
			//Initialize the Was Visited array to false;
			for (int i=0; i<nCourse; i++){
				visited[i]=0;
			}
			
			//Initialize the Adjacency Matrix to false;
			for (int i=0; i<nCourse; i++){
				for (int j=0; j<nCourse; j++){
					am[i][j]=0;
				}
			}
			
			//Fill the Adjacency Matrix with edges
			data >> nEdges;
			int u, v;	//adjacent vertices
			for (int i=0; i<nEdges; i++){
				data >> u >> v;
				am[u][v]=1;
			}
			
			//Load the Indegree array
			for (int i=0; i<nCourse; i++){
				for (int j=0; j<nCourse; j++){
					if (am[i][j]==1){
						indeg[j]++;
						
					}
				}
			}
			cout<<"Loaded graph.\n"<<endl;
			
			//Output Vertices/Course Codes
			cout<<"Vertices:"<<endl;
			for (int i=0; i<nCourse; i++){
				cout<<"["<<i<<":"<<course[i]<<"]";
				if (i != nCourse-1)cout<<", ";
				if ((i+1)%6 == 0)cout<<"\n";
			}
			
			//Output Edges/Prerequisites
			cout<<"\nEdges:"<<endl;
			for (int i=0; i<nCourse; i++){
				cout<<course[i]<<" -> ";
				int e=0;
				for (int j=0; j<nCourse; j++){
					if (am[i][j]==1){
						if (e!=0)cout<<",";
						cout<<course[j];
						e++;
					}
				}
				cout<<"\n";
			}
			
			//Topological Sort
			stringstream ss;  //sorted graph output
			bool more=true;   //more valid vertices for topological sort
			while(more){
				//Search for valid vertice's to traverse 
				for(int i=0; i<nCourse; i++){
					//Process vertice, delete edge(s) to sucessor(s) 
					if(indeg[i]==0 && visited[i] == 0){
						visited[i]=true;
						ss << course[i] << ' ';
						for(int k=0; k<nCourse; k++){
							if (am[i][k]==1) indeg[k]--;
						}
					}
				}
				//Check for more valid vertices
				more=false;
				for (int i=0; i<nCourse; i++){
					if(visited[i]==0 && indeg[i]==0)more=true;
				}
			}
			
			//Check if Topological Sort occured & print results if applicable
			bool valid=true;	//all vertices visited
			for(int i=0; i<nCourse; i++){
				if(visited[i]==0)valid=false;
			}
			if(valid){
				cout<<"\nTopological Sort found!"<<endl;
				cout<<ss.str()<<endl;
			}
			else {
				cout<<"\nCyclic dependencies; no topological sort possible."<<endl;
			}
		}
		else cout<<"Error: File not found."<<endl;
}