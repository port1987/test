-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------

Option Compare Database

Private Sub cmd_exl_exp_Click()
    Dim db As Database
    Dim oxls As Object
    Dim stsql As String
    Dim rownum As Integer
    Dim colnum As Integer
    Dim from_ym As Long
    Dim to_ym As Long
    Dim i As Integer
    Dim months As Integer
    Dim tstChk As Integer
    Dim mainChk As Integer

    from_ym = [Forms]![form1]![txt_from_ym]
    to_ym = [Forms]![form1]![txt_to_ym]
    rownum = 0
    colnum = 5
    i = 1
    tstChk = 0
    mainChk = 0

    If from_ym > to_ym Or from_ym < 202001 Then
        Call MsgBox("入力した日付が正しくありません。", vbOKOnly)
        Exit Sub
    End If

    '何か月分か計算　終了月－開始月　来年にまたがる場合の処理を含む
    months = to_ym - from_ym
    If months > 11 Then
        months = months - (87 + (Mid(to_ym, 3, 2) - Mid(from_ym, 3, 2) - 1) * 88)
    Else
        months = months + 1
    End If

    db = CurrentDb

    oxls = CreateObject("Excel.application")
    oxls.Visible = True
    oxls.workbooks.Open(FileName:="c:\MAINEXE\mlit.xltx")

    '列を挿入する
    Do Until (months = i)
        oxls.worksheets("mlit").Columns("E").Insert()
        oxls.worksheets("mlit").Columns(13 + i).Insert()
        i = i + 1
    Loop

    '月ごとに各会場に件数を挿入
    Do Until (from_ym > to_ym)
        'mainDB分
        rownum = 18
        stsql = ""
        stsql = stsql & "SELECT maindbt_i16_mlit1.i16_center_cd AS 支店コード, Count(maindbt_i16_mlit1.i16_req_dt) AS 件数 "
        stsql = stsql & "FROM maindbt_i16_mlit1 "
        stsql = stsql & "WHERE (((maindbt_i16_mlit1.i16_req_dt) Between ( '" & from_ym & "01') And ('" & from_ym & "31') "
        stsql = stsql & "And (maindbt_i16_mlit1.i16_length > '1'))) "
        stsql = stsql & "GROUP BY maindbt_i16_mlit1.i16_center_cd "

        If SETNUM(stsql, oxls, DB, rownum, colnum) = True And mainChk = 0 Then
            mainChk = mainChk + 1
        End If

        'testDB分
        rownum = rownum + 3
        stsql = ""
        stsql = stsql & "SELECT maindbt_i16_mlit.i16_center_cd AS 会場コード, Count(maindbt_i16_mlit.i16_req_dt) AS 件数 "
        stsql = stsql & "FROM maindbt_i16_mlit "
        stsql = stsql & "WHERE (((maindbt_i16_mlit.i16_req_dt) Between ( '" & from_ym & "01') And ('" & from_ym & "31') "
        stsql = stsql & "And (maindbt_i16_mlit.i16_length > '1'))) "
        stsql = stsql & "GROUP BY maindbt_i16_mlit.i16_center_cd "

        If SETKENSU(stsql, oxls, DB, rownum, colnum) = True And tstChk = 0 Then
            tstChk = tstChk + 1
        End If

        '来年以降にまたがる場合の処理
        If Right(from_ym, 2) = 12 Then
            from_ym = from_ym + 89
        Else
            from_ym = from_ym + 1
        End If
        colnum = colnum + 1
    Loop

    rownum = 3
    colnum = months + 4

    to_ym = Right(to_ym, 2)

    oxls.worksheets("mlit").Cells(rownum, colnum).Value = to_ym & "月"
    oxls.worksheets("mlit").Cells((rownum + 14), colnum).Value = to_ym & "月"
    oxls.worksheets("mlit").Cells((rownum + 1), (months * 2 + 11)).Value = to_ym & "月"

    If months > 1 Then
        'フィルハンドル操作(金額表、作業用の表)
        Do Until (rownum = 26)
            oxls.worksheets("mlit").Cells(rownum, colnum).AutoFill( _
            Destination:=oxls.worksheets("mlit").Range(oxls.worksheets("mlit").Cells(rownum, colnum), oxls.worksheets("mlit").Cells(rownum, (colnum - months + 1))))
            rownum = rownum + 1

            If rownum = 13 Then
                rownum = rownum + 4
            ElseIf rownum = 18 Then
                rownum = rownum + 6
            End If
        Loop
        'フィルハンドル操作(件数表)
        rownum = 4
        colnum = months * 2 + 11
        Do Until (rownum = 10)
            oxls.worksheets("mlit").Cells(rownum, colnum).AutoFill( _
            Destination:=oxls.worksheets("mlit").Range(oxls.worksheets("mlit").Cells(rownum, colnum), oxls.worksheets("mlit").Cells(rownum, (colnum - months + 1))))
            rownum = rownum + 1
        Loop
    End If

    oxls.worksheets("mlit").Range("B2").Value = "mlit　支店別費用配布 " & [Forms]![form1]![txt_from_ym] & " ～ " & [Forms]![form1]![txt_to_ym] & " 分"

    oxls.worksheets("mlit").Columns("D").Delete()
    oxls.worksheets("mlit").Columns(10 + months).Delete()

    If tstChk = 0 Then
        Call MsgBox("テスト対象データ無しでした。", vbOKOnly)
    End If
    If mainChk = 0 Then
        Call MsgBox("本番対象データ無しでした。", vbOKOnly)
    End If

    Call MsgBox("作業完了", vbOKOnly)

End Sub

Private Function SETNUM(ByVal stsql As String, ByVal oxls As Object, ByVal DB As Database, ByVal rownum As Integer, ByVal colnum As Integer) As Boolean
    Dim rs As Recordset

    rs = DB.OpenRecordset(stsql)
    If rs.EOF Then
        rs.Close()
        rs = Nothing
        SETKENSU = False
        Exit Function
    End If

    '月別、支店別の件数を作業用の表に出力
    rs.MoveFirst()
    Do Until (rs.EOF)
        Select Case rs![支店コード]
            Case "30"
                oxls.worksheets("mlit").Cells(rownum, colnum).Value = rs![件数]
            Case "50"
                oxls.worksheets("mlit").Cells((rownum + 1), colnum).Value = rs![件数]
            Case "70"
                oxls.worksheets("mlit").Cells((rownum + 2), colnum).Value = rs![件数]
        End Select
        rs.MoveNext()
    Loop

    rs = Nothing
    SETKENSU = True
End Function
