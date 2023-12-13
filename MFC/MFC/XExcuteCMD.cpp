#include "pch.h"
#include "XExcuteCMD.h"


bool XExcuteCMD::start(string cmd)
{
	Cmd = cmd;

	char* cmd_char = const_cast<char*>(cmd.c_str());
	// 创建进程
	CreateProcessA(
		nullptr,
		cmd_char,
		Security_Attributes,
		Thread_Attributes,
		InheritHandles,
		CreationFlags,
		Environment,
		nullptr,
		&si,
		&pi);
	return true;
}

bool XExcuteCMD::start(CString cmd)
{
	Cmd = CT2A(cmd);
	return start(Cmd);
}

bool XExcuteCMD::end()
{
	WaitForSingleObject(pi.hProcess, INFINITE);
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
	return true;
}

void XExcuteCMD::initial()
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
