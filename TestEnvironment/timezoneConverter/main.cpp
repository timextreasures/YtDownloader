#include "stdincludes.h"
using namespace std;

enum zone
{
	GMT,
	UTC,
	PDT,
	zoneCount,
};
struct ttime
{
	ttime(unsigned hours, unsigned minutes) :hh(hours%24), mm(minutes%60)
	{

	}
	unsigned hh,mm;
};
struct timeZone
{
	timeZone(zone times, int offset, ttime t) :zone(times), offset(offset),t(t)
	{

	}
	timeZone()
	{

	}
	bool operator==(timeZone e)
	{
		if (e.offset == this->offset
			&& e.zone == this->zone)
		{
			return true;
		}
		return false;
	}
	int offset = 0;
	zone zone = zone::GMT;
	ttime t = ttime(0,0);
};
void pause()
{
	cout << "Press any key to exit." << endl;
	getchar();
}

string toString(zone zone)
{
	switch (zone)
	{
	case GMT:
	case UTC:
		return "GMT/UTC";
		break;
	case PDT:
		return "PDT";
		break;
	case zoneCount:
		break;
	default:
		break;
	}
}
/*
* Extracts the timeZone data from a string. isTime determines if we need a time or not.
*/
timeZone extractTimeZone(string input, bool isTime)
{
	timeZone tmpTimeZone;
	int id;

	// UTC == GMT
	if (input.find("GMT") != string::npos)
	{
		id = input.find("GMT");
		
		tmpTimeZone.zone = GMT;

		try
		{
			tmpTimeZone.offset = stoi(input.substr(id + 3));
		}
		catch (const std::exception& e)
		{
			cout << e.what() << endl;
		}
		

	}
	else if (input.find("UTC") != string::npos)
	{
		id = input.find("UTC");
		tmpTimeZone.zone = GMT;
		try
		{
			tmpTimeZone.offset = stoi(input.substr(id + 3));
		}
		catch (const std::exception& e)
		{
			cout << e.what() << endl;
		}
		
	}
	else if (input.find("PDT") != string::npos)
	{
		id = input.find("PDT");
		tmpTimeZone.zone = GMT;
		try
		{
			tmpTimeZone.offset = stoi(input.substr(id + 3)) - 7;
		}
		catch (const std::exception& e)
		{
			cout << e.what() << endl;
		}
		
	}
	else
	{
		cout << "No valid timeZone found! Type \"timezones\" to see a list of valid timezones. " << endl;
		return timeZone();
	}
	if (isTime)
	{
		string tmpTime = input.substr(0, id);
		unsigned hours = stoi(tmpTime.substr(0, tmpTime.find(":")));
		unsigned minutes = stoi(tmpTime.substr(tmpTime.find(":") + 1));

		if (hours > 24 || minutes > 59)
		{
			cout << "No valid time was given! hours between 0 and 24, and minutes between 0 and 59" << endl;
			return timeZone();
		}
		tmpTimeZone.t = ttime(hours, minutes);
	}
	

	return tmpTimeZone;
}

timeZone convert(const timeZone& curr, const timeZone& dest)
{
	// First convert it to GMT and then do the calculations
	if (curr.zone != GMT)
	{

	}
	if (dest.zone != GMT)
	{

	}
	timeZone end;
	end.t = curr.t;
	int zeroOffset = -curr.offset;
	end.t.hh += zeroOffset;
	if ((int)end.t.hh + dest.offset <= 0)
	{
		int offset = dest.offset;

		offset += end.t.hh;

		end.t.hh = 24;
		end.t.hh += offset;
	}
	else
	{
		end.t.hh += dest.offset;
	}
	
	
	end.t.hh %= 24;
	end.t.mm %= 60;


	end.zone = dest.zone;
	end.offset = dest.offset;


	// Then convert back to the specified zone requested
	return end;
}
int main(int argc, char** argvs)
{

	while (true)
	{
		timeZone currentTimeZone, newTimeZone;
		cout << "Please input your time and your time zone(ex 15:00 GMT-1)" << endl;
		string strOwnTimezone;
		getline(cin, strOwnTimezone);

		if (strOwnTimezone == "!Quit")
		{
			break;
		}
		if (strOwnTimezone == "timezones")
		{
			for (size_t i = 0; i < zoneCount; i++)
			{
				cout << toString((zone)i) << endl;
			}
		}
		else
		{
			currentTimeZone = extractTimeZone(strOwnTimezone, true);
			if (currentTimeZone == timeZone())
			{
				continue;
			}
			cout << "To what timezone do you want to convert? (ex GMT+5)" << endl;
			string strNewTimeZone;
			getline(cin, strNewTimeZone);

			newTimeZone = extractTimeZone(strNewTimeZone, false);
			if (newTimeZone == timeZone())
			{
				continue;
			}
			timeZone endZone = convert(currentTimeZone, newTimeZone);
			cout << setw(2) << setfill('0') << endZone.t.hh << ":" << setw(2) << setfill('0') << endZone.t.mm << endl;
		}
		
	}
	
	
	return 0;
}