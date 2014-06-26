Public Class Product : Inherits ProductBase
#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal productId As Integer)
        MyBase.new(productId)
    End Sub
    Public Sub New(ByVal dr As DataRow)
        MyBase.new()
        ProductId = CType(dr("ProductId"), Integer)
        Name = CType(dr("Name"), String)
        Active = CType(dr("Active"), Integer)
    End Sub
#End Region

#Region "Properties"

    Overloads Shared ReadOnly Property Products() As List(Of Product)
        Get
            Dim DataCon As New AirArena.Data.DataConnector
            Dim mProducts As New List(Of Product)
            Dim req As New AirArena.Data.DataRequest("List_Products")
            req.Parameters.Add("@Active", 1)
            Dim dt As DataTable = DataCon.GetDataTable(req)
            For Each dr As DataRow In dt.Rows
                mProducts.Add(New Product(dr))
            Next dr
            Return mProducts
        End Get
    End Property

#End Region

#Region "Methods"

#End Region

End Class