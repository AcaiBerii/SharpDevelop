// <file>
//     <copyright see="prj:///doc/copyright.txt">2002-2005 AlphaSierraPapa</copyright>
//     <license see="prj:///doc/license.txt">GNU General Public License</license>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

namespace Debugger.Interop.CorDebug
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("6DC3FA01-D7CB-11D2-8A95-0080C792E5D8"), InterfaceType((short) 1)]
    public interface ICorDebugEditAndContinueSnapshot
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CopyMetaData([In, MarshalAs(UnmanagedType.Interface)] IStream pIStream, out Guid pMvid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetMvid(out Guid pMvid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetRoDataRVA(out uint pRoDataRVA);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetRwDataRVA(out uint pRwDataRVA);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetPEBytes([In, MarshalAs(UnmanagedType.Interface)] IStream pIStream);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetILMap([In] uint mdFunction, [In] uint cMapSize, [In] ref _COR_IL_MAP map);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetPESymbolBytes([In, MarshalAs(UnmanagedType.Interface)] IStream pIStream);
    }
}
