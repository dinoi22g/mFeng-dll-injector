Public Class FormList

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        On Error Resume Next
        Dim addlst As New OpenFileDialog
        With addlst
            .Title = "添加Dynamic Link Library"
            .Filter = "動態連結資料庫 (*.dll)|*.dll"
            .ShowDialog()

        End With
        If addlst.FileName <> "" Then
            If ListBox1.Items.Count = 0 Then
                ListBox1.Items.Add(addlst.FileName)
            ElseIf ListBox1.Items.Count > 0 Then
                For i = 0 To ListBox1.Items.Count - 1

                    If addlst.FileName <> ListBox1.Items.Item(i).ToString And i = ListBox1.Items.Count - 1 Then
                        ListBox1.Items.Add(addlst.FileName)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub btnDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDel.Click
        On Error Resume Next
        If ListBox1.SelectedIndex > -1 Then ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        '  If ListBox1.Items.Count >= 1 Then 
        SaveListBox(ListBox1)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        On Error Resume Next
        If ListBox1.Items.Count >= 1 Then SaveListBox(ListBox1)
        Me.Hide()
        FormMain.Enabled = True
    End Sub

    Private Sub FormList_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        On Error Resume Next
        If ListBox1.Items.Count >= 1 Then SaveListBox(ListBox1)
        Me.Hide()
        FormMain.Enabled = True
    End Sub

    Private Sub FormList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        GetListBox(ListBox1)
    End Sub
End Class