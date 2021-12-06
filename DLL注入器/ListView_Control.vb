Module ListView_Control
    Public Sub ListView1_AddNewItem(ByVal Name As String, ByVal PID As String, ByVal PATH As String)
        Dim item As New ListViewItem
        item.Text = Name
        item.SubItems.Add(PID)
        item.SubItems.Add(PATH)
        Form1.ListView1.Items.Add(item)
    End Sub

    Public Sub ListView1_DeleteItems()
        '把所有選取的項目都刪除
        For i As Integer = Form1.ListView1.SelectedIndices.Count - 1 To 0 Step -1
            Form1.ListView1.Items.RemoveAt(Form1.ListView1.SelectedIndices(i))
        Next
    End Sub

    Public Sub ListView1_SelectAll()
        '全選
        For Each item As ListViewItem In Form1.ListView1.Items
            item.Selected = True
        Next
    End Sub

    Public Sub ListView1_MoveUp()
        '檢查有沒有選取項目
        If Form1.ListView1.SelectedIndices.Count > 0 Then
            '用for迴圈由小到大去巡覽
            For i As Integer = 0 To Form1.ListView1.SelectedIndices.Count - 1
                Dim index As Integer = Form1.ListView1.SelectedIndices(i)
                '如果index為第一項就不需要上移
                If index > 0 Then
                    '如果index-1被選取就不進行上移
                    If Form1.ListView1.SelectedIndices.Contains(index - 1) Then
                        Continue For
                    End If
                    '進行換位置的動作
                    Dim tmp As ListViewItem = Form1.ListView1.Items(index)
                    Form1.ListView1.Items.RemoveAt(index)
                    Form1.ListView1.Items.Insert(index - 1, tmp)
                    Form1.ListView1.Items(index - 1).Focused = True

                End If
            Next
        End If
    End Sub

    Public Sub ListView1_MoveDown()
        '檢查有沒有選取項目
        If Form1.ListView1.SelectedIndices.Count > 0 Then
            '用for迴圈由大到小去巡覽
            For i As Integer = Form1.ListView1.SelectedIndices.Count - 1 To 0 Step -1
                Dim index As Integer = Form1.ListView1.SelectedIndices(i)
                '如果index為最後一項就不需要下移
                If index < Form1.ListView1.Items.Count - 1 Then
                    '如果index+1被選取就不進行下移
                    If Form1.ListView1.SelectedIndices.Contains(index + 1) Then
                        Continue For
                    End If
                    '進行換位置的動作
                    Dim tmp As ListViewItem = Form1.ListView1.Items(index)
                    Form1.ListView1.Items.RemoveAt(index)
                    Form1.ListView1.Items.Insert(index + 1, tmp)
                    Form1.ListView1.Items(index + 1).Focused = True
                End If
            Next
        End If
    End Sub
End Module
