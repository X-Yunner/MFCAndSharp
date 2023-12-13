#pragma once
#include <string>

using namespace std;
class XExcuteCMD
{

public:
	STARTUPINFOA si;
	PROCESS_INFORMATION pi;

	string ApplicationName;
	string Cmd;
	SECURITY_ATTRIBUTES* Security_Attributes;
	SECURITY_ATTRIBUTES* Thread_Attributes;
	bool InheritHandles;
	DWORD CreationFlags;
	void* Environment;
	string CurrentDirectory;

public:

	XExcuteCMD()
	{
		ZeroMemory(&si, sizeof(si));
		si.cb = sizeof(si);
		ZeroMemory(&pi, sizeof(pi));
		ApplicationName = "";
		Security_Attributes = nullptr;
		Thread_Attributes = nullptr;
		InheritHandles = false;
		CreationFlags = 0;
		Environment = nullptr;
		CurrentDirectory = "";
	}
	~XExcuteCMD()
	{
		CloseHandle(pi.hProcess);
		CloseHandle(pi.hThread);
		pi.hProcess = nullptr;
		pi.hThread = nullptr;
		ZeroMemory(&si, sizeof(si));
		ZeroMemory(&pi, sizeof(pi));
	}


	void initial();
	bool start(string cmd);
	bool start(CString cmd);
	bool end();
};

