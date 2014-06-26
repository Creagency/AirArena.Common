Namespace EmailTest.EmailTestResults

    Public Class EmailSplitTestResult

        Protected DataCon As AirArena.Data.DataConnector
#Region " Constructors "
        Public Sub New()
            DataCon = New AirArena.Data.DataConnector
        End Sub


        'Public Sub New(ByVal EmailSplitTestId As Integer)
        '    Me.New()
        'End Sub
#End Region


        Public Sub Save()


            Dim req As New AirArena.Data.DataRequest("AU_EmailSplitTestResult")
            req.Parameters.Add("@EmailSplitTestId", EmailSplitTestId())
            req.Parameters.Add("@EmailResultType", EmailResultType())
            req.Parameters.Add("@SingupCode", SingupCode())
            req.Parameters.Add("@VariationId", VariationId())

            Dim dt As DataTable = DataCon.GetDataTable(req)
            If dt.Rows.Count > 0 Then
                EmailResultId = CType(dt.Rows(0)("EmailResultId"), Integer)
            End If


        End Sub

        Public Property EmailResultId() As Integer
        Public Property EmailSplitTestId() As Integer
        Public Property EmailResultType() As EmailSplitTestResultType
        Public Property SingupCode() As Integer
        Public Property dt_Result As DateTime
        Public Property VariationId() As Integer



        Public Enum EmailSplitTestResultType
            EmailSend = 1
            EmailOpen
            EmailClick
            EmailConversion
        End Enum
    End Class

End Namespace
