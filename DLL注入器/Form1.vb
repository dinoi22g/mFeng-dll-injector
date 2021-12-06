Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
    Dim Choose As Integer
    Dim GetDate, hour, min, sec As String
    Dim pname As Process

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Me.Show()
        Delay(0.5)
        Choose = -1
        GetProcess()
        GetTime()
    End Sub

    Sub GetTime()
        On Error Resume Next
        If Now.Hour.ToString.Length <= 1 Then
            hour = "0" & Now.Hour
        Else
            hour = Now.Hour
        End If

        If Now.Minute.ToString.Length <= 1 Then
            min = "0" & Now.Minute
        Else
            min = Now.Minute
        End If

        If Now.Second.ToString.Length <= 1 Then
            sec = "0" & Now.Second
        Else
            sec = Now.Second
        End If
        lblCount.Text = "目前共有 [ " & ListView1.Items.Count & " ] 個進程可被加載的進程 ( " & Now.Date & " " & hour & ":" & min & ":" & sec & " )"
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Try
            Dim see As Integer = Convert.ToInt16(ListView1.Items.Item(Choose).SubItems(1).Text)
            ' textBox1.Text = see.ToString()
            Dim modules As ProcessModuleCollection
            pname = Process.GetProcessById(see)
            modules = pname.Modules
            Dim aModule As ProcessModule
            FormLoadDll.ListBox1.Items.Clear()
            For i As Integer = 0 To modules.Count - 1
                aModule = modules(i)
                FormLoadDll.ListBox1.Items.Add(aModule.FileName)
            Next
            FormLoadDll.Text = "目前選取進程: " & ListView1.Items.Item(Choose).SubItems(0).Text
            FormLoadDll.Show()
        Catch
        End Try



    End Sub


    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        On Error Resume Next
        Dim index As Integer
        For Each index In ListView1.SelectedIndices
            Choose = index '選擇的項目
        Next
    End Sub

    Sub GetProcess()
        ListView1.Items.Clear()
        '   ListBox1.Items.Clear()
        Dim myProcess As Process() = Process.GetProcesses()
        Me.ListView1.SmallImageList = Me.ImageList1
        Dim myICon As System.Drawing.Icon
        For Each p As Process In myProcess
            Try
                If Not ImageList1.Images.ContainsKey(p.MainModule.FileName) Then
                    myICon = System.Drawing.Icon.ExtractAssociatedIcon(p.MainModule.FileName)
                    ImageList1.Images.Add(p.MainModule.FileName, myICon)
                End If
                Dim lvItem As New ListViewItem(p.ProcessName, p.MainModule.FileName)
                lvItem.SubItems.Add(p.Id)
                lvItem.SubItems.Add(p.MainModule.FileName)
                Me.ListView1.Items.Add(lvItem)
                With ListView1
                    .Items(Choose).Selected = True
                    .Items(Choose).Focused = True
                End With
            Catch
            End Try
        Next
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        btnRefresh.Enabled = False
        ListView1.Items.Clear()

        Dim myProcess As Process() = Process.GetProcesses()
        Me.ListView1.SmallImageList = Me.ImageList1
        Dim myICon As System.Drawing.Icon
        For Each p As Process In myProcess
            Try
                If Not ImageList1.Images.ContainsKey(p.MainModule.FileName) Then
                    myICon = System.Drawing.Icon.ExtractAssociatedIcon(p.MainModule.FileName)
                    ImageList1.Images.Add(p.MainModule.FileName, myICon)
                End If
                Dim lvItem As New ListViewItem(p.ProcessName, p.MainModule.FileName)
                lvItem.SubItems.Add(p.Id)
                lvItem.SubItems.Add(p.MainModule.FileName)
                Me.ListView1.Items.Add(lvItem)
                With ListView1
                    .Items(Choose).Selected = True
                    .Items(Choose).Focused = True
                End With
            Catch
            End Try
        Next
        btnRefresh.Enabled = True
        GetTime()
    End Sub

    Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnList.Click
        On Error Resume Next
        FormList.Show()
        Me.Enabled = False
    End Sub


    Private Sub btnInjection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInjection.Click
        On Error Resume Next
        btnInjection.Enabled = False
        Dim DllList As New ListBox
        GetListBox(DllList)
        DllList.SelectedIndex = 0
        For i = 0 To DllList.Items.Count
            '確認指定處理序名之處理序是否存在.
            Dim DllPath As String = DllList.Text
            Me.Text = "MFeng DLL Injection - " & DllPath
            Dim NameProcess As String = ListView1.Items.Item(Choose).SubItems(0).Text
            If (Process.GetProcessesByName(NameProcess).Length = 0) Then
                lblCr.Text = "∴" & "找不到進程"
                btnInjection.Enabled = True
                Exit Sub
            End If
            '取得當前活動中之指定處理序進程句柄.
            Dim TargetHandle As IntPtr = Process.GetProcessesByName(NameProcess)(0).Handle
            If (TargetHandle.Equals(IntPtr.Zero)) Then
                lblCr.Text = "∴" & "該進程無法展開"
                btnInjection.Enabled = True
                Exit Sub
            End If
            '獲取LoadLibraryA的地址(PS:不同進程但同API,地址相同).
            Dim GetAdrOfLLBA As IntPtr = GetProcAddress(GetModuleHandle("Kernel32"), "LoadLibraryA")
            If (GetAdrOfLLBA.Equals(IntPtr.Zero)) Then
                lblCr.Text = "∴" & "LoadLibraryA地址取得失敗"
                btnInjection.Enabled = True
                Exit Sub
            End If
            '將DLL路徑轉為Char()陣列.
            Dim OperaChar As Byte() = System.Text.Encoding.Default.GetBytes(DllPath)
            '在目標進程申請一塊空間存放路徑字串.
            Dim DllMemPathAdr = VirtualAllocEx(TargetHandle, 0&, &H64, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
            If (DllMemPathAdr.Equals(IntPtr.Zero)) Then
                lblCr.Text = "∴" & "虛擬空間提交失敗"
                btnInjection.Enabled = True
                Exit Sub
            End If
            '將申請來的記憶體空間寫入路徑Char()陣列.
            If (WriteProcessMemory(TargetHandle, DllMemPathAdr, OperaChar, OperaChar.Length, 0) = False) Then
                lblCr.Text = "∴" & "記憶體寫入失敗"
                btnInjection.Enabled = True
                Exit Sub
            End If
            '令目標進程呼叫LoadLibraryA加載Char()陣列中存放的路徑.
            CreateRemoteThread(TargetHandle, 0, 0, GetAdrOfLLBA, DllMemPathAdr, 0, 0)
            lblCr.Text = "∴" & "注入成功，請查看"
            DllList.SelectedIndex += 1
        Next
        btnInjection.Enabled = True
    End Sub
End Class
