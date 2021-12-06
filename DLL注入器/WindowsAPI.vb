Imports System.Runtime.InteropServices
Imports System.IO

Module WindowsAPI
    Public Declare Function VirtualAllocEx Lib "kernel32" (ByVal hProcess As Integer, ByVal lpAddress As Integer, ByVal dwSize As Integer, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As Integer
    Public Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByVal lpBuffer As Byte(), ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Public Declare Function GetProcAddress Lib "kernel32" (ByVal hModule As Integer, ByVal lpProcName As String) As Integer
    Public Declare Function GetModuleHandle Lib "Kernel32" Alias "GetModuleHandleA" (ByVal lpModuleName As String) As Integer
    Public Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As Integer, ByVal lpThreadAttributes As Integer, ByVal dwStackSize As Integer, ByVal lpStartAddress As Integer, ByVal lpParameter As Integer, ByVal dwCreationFlags As Integer, ByRef lpThreadId As Integer) As Integer
    Public Const MEM_COMMIT = 4096, PAGE_EXECUTE_READWRITE = &H40

    Public Structure SHFILEINFO
        Public hIcon As IntPtr ' : icon
        Public iIcon As Integer ' : icondex
        Public dwAttributes As Integer ' : SFGAO_ flags
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> Public szTypeName As String
    End Structure
    Public Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr
    Public Const SHGFI_ICON = &H100
    Public Const SHGFI_SMALLICON = &H1
    Public Const SHGFI_LARGEICON = &H0         ' Large icon
    Public nIndex = 0

    Public Function Delay(ByRef Interval As Double)
        Dim Start As Integer = Environment.TickCount()
        Dim TimeLast As Integer = Interval
        Do
            If Environment.TickCount() - Start > TimeLast Then Exit Do
            Application.DoEvents() ' 要記得寫這行，不然都在跑迴圈，畫面可能會不見
        Loop
    End Function

    '//儲存ListBox
    Public Function SaveListBox(ByVal listbox As ListBox)
        On Error Resume Next
        Dim myStreamWriter As System.IO.StreamWriter
        myStreamWriter = New StreamWriter(Application.StartupPath & "\DLL_Path")
        listbox.SelectedIndex = 0
        For i = 1 To listbox.Items.Count
            myStreamWriter.Write(listbox.Text & vbCrLf)
            listbox.SelectedIndex = listbox.SelectedIndex + 1
        Next
        myStreamWriter.Flush()
        myStreamWriter.Close()
    End Function

    '//讀取ListBpx
    Public Function GetListBox(ByVal listbox As ListBox)
        On Error Resume Next
        Dim reader As StreamReader
        reader = New StreamReader(Application.StartupPath & "\DLL_Path", System.Text.Encoding.UTF8)
        listbox.Items.Clear()
        Do
            listbox.Items.Add(reader.ReadLine)
        Loop Until reader.Peek = -1
        reader.Close()
    End Function
End Module
