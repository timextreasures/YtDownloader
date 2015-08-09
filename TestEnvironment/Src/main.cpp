#include <iostream>
#include <string>
#include <cstdlib>
#include <vector>
using namespace std;
void pause()
{
	cout << "Press any key to continue..." << endl;
	getchar();

}
bool compare(int *arr1, int *arr2, int sz)
{
	for (int i = 0; i < sz; ++i)
	{
		if (*(arr1+i) != *(arr2+i))
		{
			return false;
		}
	}
	return true;
}
bool compare(std::vector<int>&arr1, std::vector<int>&arr2)
{
	if (arr1.size() != arr2.size())
	{
		return false;
	}
	for (int i = 0; i < arr1.size(); ++i)
	{
		if (arr1[i] != arr2[i])
		{
			return false;
		}
	}
	return true;
}
int main(int argc, char** argvs)
{
	constexpr size_t sz = 5;
	int arr[sz] = {1,2,3,4,5};
	int arr2[sz] = {1,2,3,4,5 };
	
	

	if (compare(arr,arr2,sz))
	{
		cout << "Arrays are the same!" << endl;
	}
	else
	{
		cout << "Arrays are not the same!" << endl;
	}
	using intV = std::vector<int>;
	intV vector = { 1,2,3,4,5 };
	intV vector2 = { 1,2,3,4,3,2 };
	if (compare(vector,vector2))
	{
		cout << "Vectors are the same!" << endl;
	}
	else
	{
		cout << "Vectors are not the same!" << endl;
	}

	char ca[] = { 'C', '+','+' , '\0'};
	cout << strlen(ca) << endl;

	const char da[] = { 'h', 'e','l','l','o','\0' };
	const char *cp = da;
	while (*cp)
	{
		cout << *cp << endl;
		++cp;
	}

	string str1 = "Yoyoyo";
	string str2 = "Yoy";
	if (str1 == str2)
	{
		cout << str1 << " equals " << str2 << endl;
	}
	else if (str1 < str2)
	{
		cout << str1 << " is smaller than " << str2 << endl;
	}
	else
	{
		cout << str1 << " is bigger than " << str2 << endl;
	}

	char cstr1[] = "Yoyoyo";
	char cstr2[] = "yoyo";
	int value = strcmp(cstr1, cstr2);
	if (value == 0)
	{
		cout << cstr1 << " equals " << cstr2 << endl;
	}
	else if(value > 0)
	{
		cout << cstr1 << " is smaller than " << cstr2 << endl;
	}
	else
	{
		cout << cstr1 << " is bigger than " << cstr2 << endl;
	}

	// Copying a string
	char *added = (char*)malloc(sizeof(char)*(strlen(cstr1)+strlen(cstr2)+1));
	strcpy(added, cstr1);
	strcat(added, cstr2);
	cout << added << endl;
	
	// Initializing array of a vectorl
	int arr1[] = { 1,2,3,4,5 };
	std::vector<int>v(begin(arr1), end(arr1));


	pause();
	free(added);
	return 0;
}