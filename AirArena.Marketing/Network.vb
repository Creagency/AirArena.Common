Public Class Network : Inherits NetworkBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal networkId As Integer)
        MyBase.new(networkId)
    End Sub

#Region "Methods"

    Shared Function ListAllNetworks() As List(Of Network)

        Dim DataCon As New AirArena.Data.DataConnector
        Dim Networks As New List(Of Network)
        Dim req As New AirArena.Data.DataRequest("List_Networks")
        Dim dt As DataTable = DataCon.GetDataTable(req)
        For Each dr As DataRow In dt.Rows
            Networks.Add(New Network(CInt(dr("i_Code"))))
        Next dr

        Return Networks

    End Function

#End Region

End Class