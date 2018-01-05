#include <iostream>       // std::cout
#include <vector>
#include <sstream>      // std::stringstream

using namespace std;

/*
	STACK IS LISTED BOTTOM TO TOP
	A, B, C 
	
	= 
	
	C
	B
	A

*/



void printOrder(vector<char> orderLayout[3])
{
	int numCones = 3;
	string test;
	stringstream ss;
	int value = 0;

	printf("Number of cones: %i\n", numCones);

	for (int cone = 0; cone < numCones; cone++)
	{
		for (int scoop = 0; scoop < orderLayout[cone].size(); scoop++)
		{
		
			printf("%c, ", orderLayout[cone][scoop]);
			ss << orderLayout[cone][scoop]; 
			value = value *10 + (orderLayout[cone][scoop]-'0');
		}
			
		cout << "\n";
		ss << '0';
		value = value*10;
		
	}
	 
 	test = ss.str();
	cout << test << "\n";
	cout << value << "\n";
	
	
	
}

int main()
{
	vector<char> orderLayout[3];
	
	orderLayout[0].push_back('1');
	orderLayout[0].push_back('2');

	orderLayout[1].push_back('4');
	
	orderLayout[2].push_back('5');	
	orderLayout[2].push_back('6');
	
		orderLayout[0].push_back('3');
	
	
	printOrder(orderLayout);
		
	printf ("\n SWAPPING! \n");
	
	orderLayout[1].push_back(orderLayout[0].back());
		orderLayout[0].pop_back();


	printOrder(orderLayout);

	
}


