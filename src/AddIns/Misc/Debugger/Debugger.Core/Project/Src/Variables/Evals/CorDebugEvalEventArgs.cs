// <file>
//     <copyright see="prj:///doc/copyright.txt">2002-2005 AlphaSierraPapa</copyright>
//     <license see="prj:///doc/license.txt">GNU General Public License</license>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;
using Debugger.Interop.CorDebug;

namespace Debugger 
{	
	[Serializable]
	class CorDebugEvalEventArgs : DebuggerEventArgs
	{
		ICorDebugEval corDebugEval;
		
		public ICorDebugEval CorDebugEval {
			get {
				return corDebugEval;
			}
		}
		
		public CorDebugEvalEventArgs(NDebugger debugger, ICorDebugEval corDebugEval): base(debugger)
		{
			this.corDebugEval = corDebugEval;
		}
	}
}
