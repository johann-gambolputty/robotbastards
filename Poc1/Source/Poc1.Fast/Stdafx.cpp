// stdafx.cpp : source file that includes just the standard includes
// Poc1.Fast.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

//
//	NOTE: AP:
//	Compiling this without the C4945 warning removed at a project level will generate a lot of warnings of the
//	following type:
//		C4945: 'Type': cannot import symbol from 'assemblyPath': as 'type' has already been imported from another assembly 'assemblyName'
//	They are harmless, and there may be a hotfix on the way.
//	Follow this link for more information:
//		http://forums.microsoft.com/MSDN/rss.aspx?siteid=1&postID=244850&forumID=27
//
//
