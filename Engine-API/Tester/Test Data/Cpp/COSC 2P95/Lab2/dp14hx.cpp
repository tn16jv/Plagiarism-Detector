//Dylan Pavao 5841721
#include <iostream>
using namespace std;
int main() {
        int n = 5841721;
        int m = 1000000;
        int d, i;
        while (n > 0){
                d = n/m;
                i = d;
                if (d%2==0){
                        d = d/2;
                }
                else {
                        d = d*2;
                }
                if (d >= 10){
                        d = d-10;
                }
                cout<<d;
                n = n -(i*m);
                m = m/10;
        }
        cout<<"\n"<<endl;
        double monk;
        cout<<"How many monkeys do I have? ";cin>>monk;
        cout<<"I have "<<(monk>=1000000?monk/1000000:monk);cout<<(monk>=1000000?" million monkeys":monk>1||monk==0?" monkeys":" monkey")<<endl;
        int c1, c2;
        cout<<"Do you have room for tiramisu? (1:yes, 0:no) ";cin>>c1;
        cout<<"Do you actually like tiramisu? (1:yes, 0:no) ";cin>>c2;
        if (c1==1){
                if (c2==1){
                        cout<<"Ready, willing, and able to enjoy tiramisu!\n";
                }
                else {
                        cout<<"I am not a big fan, but I am hungry!\n";
                }
        }
        else{
                cout<<"It doesn't matter if I like it or not, I don't have room for dessert!\n";
        }
}
